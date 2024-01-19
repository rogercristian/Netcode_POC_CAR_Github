using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ShowDash : NetworkBehaviour
{
    [SerializeField] GameObject vfxTeste;
  //  [SerializeField] Transform NTObjectTransform;
    [SerializeField] Transform transformOverlapSphere;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAttack playerAttack = GetComponent<PlayerAttack>();
        if (!playerAttack.IsAttacking) {
            HideDashServerRpc();
        }

    }

    [ServerRpc]
    public void ShowDashServerRpc()
    {
      GameObject go = Instantiate(vfxTeste, transformOverlapSphere.position, transformOverlapSphere.rotation);
        //go.GetComponent<DashBehavior>().parent = this;
        go.GetComponent<NetworkObject>().Spawn(true);
       
    }
    [ServerRpc]
    public void HideDashServerRpc()
    {
        vfxTeste.GetComponent<NetworkObject>().Despawn(true);
        Destroy(vfxTeste);
    }
}
