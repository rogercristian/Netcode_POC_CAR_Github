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
    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<CarController>())
        {
            Instantiate(m_Object, transform.position, transform.rotation);
            m_Rigidbody.AddExplosionForce(explosionForce, Vector3.up, explosionRadius);
            
            TakeDamage takeDamage = collision.gameObject.GetComponent<TakeDamage>();
            if (applyDamage && takeDamage !=null)
            {
                takeDamage.ApplyDamageServerRpc(damage);
            }

            Destroy(gameObject, .1f);
        }
    }
}
