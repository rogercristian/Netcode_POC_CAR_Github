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
        rb.AddForce( new Vector3(-1, 1, -1) * forceToImpulseOnHit, ForceMode.VelocityChange);
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
        currentEulerAngles += new Vector3(0, transform.rotation.y, 0);

        bool isNotOverturned = Physics.Raycast(transform.position,
            transform.TransformDirection(Vector3.down),
            out RaycastHit hit, /*Mathf.Infinity*/ maxDistance, layerMask);

        Debug.DrawLine(transform.position, hit.point, Color.magenta);
        isCarController = isNotOverturned;

        while (!isNotOverturned)
        {
            Debug.Log("Não toquei no chão /" + currentEulerAngles);

            StartCoroutine(Resetoverturned());

            break;
        }
    }

    IEnumerator Resetoverturned()
    {

        yield return new WaitForSeconds(5f);
        if (!isCarController)
            transform.localEulerAngles = currentEulerAngles;
    }

    void IsDead()
    {
        Debug.Log("Morri");
    }

}
