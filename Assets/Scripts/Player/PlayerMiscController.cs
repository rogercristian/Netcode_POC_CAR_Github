using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerMiscController : NetworkBehaviour
{
    //public static PlayerMiscController instance;

    [HideInInspector] public bool isCarController;
    private PlayerAttack attack;
    private PlayerStats stats;
    //private HealthBar healthBar;
    //private int currentEnergy;
    private Vector3 currentEulerAngles;
    [SerializeField] private float maxDistance = 5.0f;
    [SerializeField] private float timeOverturned = 5.0f;
    [SerializeField] private LayerMask layerMask;
    //[SerializeField] private float forceToImpulseOnHit = 10f;

    InputManager inputManager;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            Destroy(this);
        }
    }
    void Start()
    {
        //instance = this;       
        attack = GetComponent<PlayerAttack>();
        stats = GetComponent<PlayerStats>();
        //healthBar = GetComponentInChildren<HealthBar>();
        //currentEnergy = stats.initialEnergy;
        //healthBar.SetMaxHealth(stats.initialEnergy);
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
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
        if (!isNotOverturned) return;

        Vector2 dir = inputManager.GetAcceleratePressed();
        if (inputManager.GetInteractPressed() && dir.y > 0)
        {
            if (isNotOverturned)
            {
                attack.Attack();
                AttackServerRpc();
            }

        }
    }

    [ServerRpc]
    private void AttackServerRpc()
    {
        AttackClientRpc();
    }
    [ClientRpc]
    private void AttackClientRpc()
    {
        if (!IsOwner)
            attack.Attack();
    }


    //public void ApplyDamage(int amount)
    //{
    //    currentEnergy -= amount;
    //    healthBar.SetHealth(currentEnergy);

    //    Rigidbody rb = GetComponent<Rigidbody>();

    //    rb.AddForce(transform.up * forceToImpulseOnHit, ForceMode.VelocityChange);

    //    if (currentEnergy <= 0)
    //    {
    //        Destroy(gameObject, 3f);
    //    }

    //    //if (currentEnergy <= 0)
    //    //{
    //    //    Destroy(gameObject, 3f);
    //    //}
    //}
    //reseta a rotação dos carros apos capotar
    IEnumerator Resetoverturned()
    {
        yield return new WaitForSeconds(timeOverturned);
        transform.localEulerAngles = currentEulerAngles;
    }
}
