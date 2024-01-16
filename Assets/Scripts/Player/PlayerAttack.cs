using Unity.Netcode;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private float attackRate;
    [SerializeField] private int damage;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashingTime;
    [SerializeField] private Vector3 origin;
    [SerializeField] private Transform transformOverlapSphere;
    [SerializeField] float hitOpponent = 2.0f;
    [SerializeField] CinemashineShake shake;
    [Header("Efeito visual do Dash")][SerializeField] private VisualEffect vfx;
    [Header("intensidade do shake da camera")][SerializeField] private float intensity = 5f;
    [Header("time do shake da camera")][SerializeField] private float time = .5f;

    private float currentAttackRate;
    private float currentDashingTime;
    private Rigidbody rb;
    private bool isHitOpponent;
    InputManager inputManager;
    //void Start()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    currentAttackRate = attackRate;
    //    currentDashingTime = dashingTime;
    //    vfx.SendEvent("OnStop");
    //    inputManager = GetComponent<InputManager>();
    //}
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;

        rb = GetComponent<Rigidbody>();
        currentAttackRate = attackRate;
        currentDashingTime = dashingTime;
        vfx.SendEvent("OnStop");
        inputManager = GetComponent<InputManager>();
    }

    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        currentAttackRate += Time.deltaTime;
        currentDashingTime += Time.deltaTime;

        Vector2 dir = inputManager.GetAcceleratePressed();
        if (inputManager.GetInteractPressed() && dir.y > 0)
        {
            AttackServerRpc();
            Attack();
        }

    }

    [ServerRpc(RequireOwnership = false)]
    private void AttackServerRpc()
    {
        AttackClientRpc();
        //if (IsAttacking)
        //{
        //    vfx.SendEvent("OnPlay");
        //}
    }
    [ClientRpc]
    private void AttackClientRpc()
    {
        if (!IsOwner) return;
        Attack();

        if (IsAttacking) vfx.SendEvent("OnPlay");

    }

    public void Attack()
    {
        if (currentAttackRate > attackRate)
        {
            currentAttackRate = 0;
            currentDashingTime = 0;

            rb.AddForce(transform.forward * dashForce, ForceMode.VelocityChange);
            vfx.SendEvent("OnPlay");

            Collider[] hitColliders = Physics.OverlapSphere(transformOverlapSphere.position, hitOpponent / 2);

            for (int i = 0; i < hitColliders.Length; i++)
            {
                TakeDamage takeDamage = hitColliders[i].GetComponent<TakeDamage>();

                if (takeDamage != null && IsAttacking)
                {
                    Debug.Log("Toquei no outro takedamage");
                    isHitOpponent = true;
                    takeDamage.ApplyDamageServerRpc(damage);
                    shake.AttackServerRpc(intensity, time);

                }
                else
                {
                    isHitOpponent = false;
                }
            }

        }

    }

    public bool IsAttacking => currentDashingTime < dashingTime;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
        if (!isHitOpponent)
            //Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
            Gizmos.DrawWireSphere(transformOverlapSphere.position, hitOpponent);
    }
}
