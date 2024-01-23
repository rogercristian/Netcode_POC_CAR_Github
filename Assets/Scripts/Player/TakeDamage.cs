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
    [SerializeField] private int maxDistance = 10;
    [SerializeField] float raioCapotamento = 1f;
    private bool isCarController;
    Vector3 currentEulerAngles;
    Rigidbody rb;
    //private void Start()
    //{
    //    healthBar = GetComponentInChildren<HealthBar>();
    //    playerStats = GetComponent<PlayerStats>();
    //    currentEnergy = playerStats.initialEnergy;
    //    healthBar.SetMaxHealth(playerStats.initialEnergy);

    //}
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;
        healthBar = GetComponentInChildren<HealthBar>();
        playerStats = GetComponent<PlayerStats>();
        currentEnergy = playerStats.initialEnergy;
        healthBar.SetMaxHealth(playerStats.initialEnergy);
        rb = GetComponent<Rigidbody>();
    }

    //public void ApplyDamage(int amount)
    //{
    //    currentEnergy -= amount;
    //    healthBar.SetHealth(currentEnergy);

    //    rb.GetComponent<NetworkRigidbody>();

    //    rb.AddForce(transform.up * forceToImpulseOnHit, ForceMode.VelocityChange);

    //    if (currentEnergy <= 0)
    //    {
    //        //Destroy(gameObject, 3f);
    //        IsDead();
    //    }
    //}

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
        rb.AddForce(new Vector3(-1, 1, -1) * forceToImpulseOnHit, ForceMode.VelocityChange);
        if (currentEnergy <= 0)
        {
            //Destroy(gameObject, 3f);
            IsDead();
        }
    }
    public void Update()
    {

        //currentEulerAngles += new Vector3(0, transform.rotation.y, 0);
        //RaycastHit hit;
        //bool isOverturned = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity, layerMask);
        //Debug.DrawLine(transform.position, hit.point, Color.magenta);
        //if (!isOverturned)
        //{
        //    StartCoroutine(Resetoverturned());
        //}
        CapotamentoServerRpc();
    }

    IEnumerator Resetoverturned()
    {

        yield return new WaitForSeconds(5f);
        DescapotamentoServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void CapotamentoServerRpc()
    {
        currentEulerAngles += new Vector3(0, transform.rotation.y, 0);

        //bool isNotOverturned = Physics.Raycast(transform.position,
        //    transform.TransformDirection(Vector3.down),
        //    out RaycastHit hit, /*Mathf.Infinity*/ maxDistance, layerMask);

        //Debug.DrawLine(transform.position, hit.point, Color.magenta);
        //isCarController = isNotOverturned;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, raioCapotamento, layerMask);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i] != null)
            {
                Debug.Log("Toquei no chão /" + currentEulerAngles);
                isCarController = true;

            }
            else
            {
                isCarController = false;
                Debug.Log("Não toquei no chão /" + currentEulerAngles);

                // StartCoroutine(Resetoverturned());
            }


        }
       if(!isCarController) {
            StartCoroutine(Resetoverturned());
        }
        //    while (!isCarController)
        //{
        //    Debug.Log("Não toquei no chão /" + currentEulerAngles);

        //    StartCoroutine(Resetoverturned());

        //    break;
        //}
    }
    [ServerRpc(RequireOwnership = false)]
    public void DescapotamentoServerRpc()
    {
        if (!isCarController)
            transform.localEulerAngles = currentEulerAngles;
    }

    void IsDead()
    {
        Debug.Log("Morri");
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireSphere(transform.position, raioCapotamento);
    }

}
