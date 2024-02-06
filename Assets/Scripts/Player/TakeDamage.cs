using System.Collections;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class TakeDamage : NetworkBehaviour
{
    int currentEnergy;
    PlayerStats playerStats;
    private HealthBar healthBar;
    [SerializeField] private float forceToImpulseOnHit = 10f;
    [SerializeField] private LayerMask layerMask;
    // [SerializeField] private int maxDistance = 10;
    [SerializeField] float raioCapotamento = 5f;
    [SerializeField] Vector3 point1;
  [HideInInspector]  public bool isCarController;
    Vector3 currentEulerAngles;
    Rigidbody rb;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;
        healthBar = GetComponentInChildren<HealthBar>();
        playerStats = GetComponent<PlayerStats>();
        currentEnergy = playerStats.initialEnergy;
        healthBar.SetMaxHealth(playerStats.initialEnergy);
        rb = GetComponent<Rigidbody>();
    }
    public void Update()
    {
        currentEulerAngles += new Vector3(0, transform.rotation.y, 0);

        CapotamentoServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void ApplyDamageServerRpc(int amount)
    {
        ApplyDamageClientRpc(amount);
    }

    [ClientRpc]
    private void ApplyDamageClientRpc(int amount)
    {
        currentEnergy -= amount;
        healthBar.SetHealth(currentEnergy);

        rb.GetComponent<NetworkRigidbody>();

        // rb.AddForce(transform.up * forceToImpulseOnHit, ForceMode.VelocityChange);
        rb.AddForce(transform.position * forceToImpulseOnHit, ForceMode.VelocityChange);
        if (currentEnergy <= 0)
        {
            //Destroy(gameObject, 3f);
            IsDead();
        }
    }      

    [ServerRpc(RequireOwnership = false)]
    public void CapotamentoServerRpc()
    {
       // Collider[] hitColliders = Physics.OverlapSphere(transform.position + point1, raioCapotamento, layerMask);
        RaycastHit hit;
        Physics.Raycast(transform.localPosition, Vector3.down, out hit, raioCapotamento, layerMask);

        if (hit.collider == null)
        {
            StartCoroutine(Resetoverturned());
            isCarController = false;
            Debug.Log("Capotei /" + currentEulerAngles + " " + isCarController);

        }
        //else
        //{
        //    isCarController = true; 
        //    Debug.Log("Toquei no chão /" + currentEulerAngles + " " + isCarController);
        //}
        //for (int i = 0; i < hitColliders.Length; i++)
        //{
        //    if (hitColliders[i] != null)
        //    {
        //        StartCoroutine(Resetoverturned());
        //    }
        //}
        //for (int i = 0; i < hitColliders.Length; i++)
        //{
        //    if (hitColliders[i] != null)
        //    {
        //        Debug.Log("Não toquei no chão /" + currentEulerAngles);
        //        isCarController = true;


        //        //isCarController = true;

        //    }
        //    if (!hitColliders[i]) 
        //    {
        //        isCarController = false;
        //        //Debug.Log("Não toquei no chão /" + currentEulerAngles);
        //        Debug.Log("Toquei no chão /" + currentEulerAngles + " " + isCarController);

        //    }


        //} 

        //if (!isCarController)
        //{
        //    StartCoroutine(Resetoverturned());
        //}
    }
    [ServerRpc(RequireOwnership = false)]
    public void DescapotamentoServerRpc()
    {
        transform.localEulerAngles = currentEulerAngles;
    }
    void IsDead()
    {
        Debug.Log("Morri");
    }
    IEnumerator Resetoverturned()
    {
        yield return new WaitForSeconds(3f);
        DescapotamentoServerRpc();
    }
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawWireSphere(transform.position + point1, raioCapotamento);
    //}

}
