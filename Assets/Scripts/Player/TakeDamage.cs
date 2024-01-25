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
    [SerializeField] float raioCapotamento = 1f;
    private bool isCarController;
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
    public void Update()
    {
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
               
            }


        }
        if (!isCarController)
        {
            StartCoroutine(Resetoverturned());
        }

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
        Gizmos.DrawWireSphere(transform.position, raioCapotamento);
    }

}
