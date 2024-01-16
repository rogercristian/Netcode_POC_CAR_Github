using Unity.Netcode;
using UnityEngine;

public class BreakingObject : NetworkBehaviour
{
    [SerializeField] private float explosionForce = 1f;
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private GameObject m_Object;

    [Header("Se o objeto a ser destruido for aplicar dano, habilite!")]
    [SerializeField] private bool applyDamage = false;
    [SerializeField] private int damage = 1;

    private Rigidbody m_Rigidbody;

    // [SerializeField] private List<GameObject> fragments = new List<GameObject>();
    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CarController>())
        {
            FragmentsServerRpc();

            TakeDamage takeDamage = collision.gameObject.GetComponent<TakeDamage>();
            if (applyDamage)
            {
                if (takeDamage != null)
                    takeDamage.ApplyDamageServerRpc(damage);
            }

            ObjectDestructibleServerRpc();
        }
    }


    [ServerRpc(RequireOwnership = false)]
    public void ObjectDestructibleServerRpc()
    {
        ObjectDestuctibleClientRpc();

    }
    [ClientRpc]
    private void ObjectDestuctibleClientRpc()
    {
        gameObject.SetActive(false);
        // Destroy(gameObject);
    }



    [ServerRpc(RequireOwnership = false)]
    public void FragmentsServerRpc()
    {
        FragmentseClientRpc();
    }
    [ClientRpc]
    private void FragmentseClientRpc()
    {
        Instantiate(m_Object, transform.position, transform.rotation);
        // fragments.Add(m_Object);
        m_Object.GetComponent<NetworkObject>();
        m_Rigidbody.AddExplosionForce(explosionForce, Vector3.up, explosionRadius);
    }
}
