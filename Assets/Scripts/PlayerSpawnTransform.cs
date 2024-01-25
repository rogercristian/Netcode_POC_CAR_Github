using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSpawnTransform : NetworkBehaviour
{

    ServerRpcParams rpcParams = default;

    public Transform spawn0;
    public Transform spawn01;

 //  readonly NetworkVariable<ulong> clientId = new (default, NetworkVariableReadPermission.Owner,NetworkVariableWritePermission.Server);
    public override void OnNetworkSpawn()
    {
        if(!IsOwner) { return; }
      //  clientId = rpcParams.Receive.SenderClientId;
    
       if(OwnerClientId == 1)
        {
            transform.position = spawn0.position;
        }
       else if(OwnerClientId == 0)
        {
            transform.position = spawn01.position;
        }

    }


   
}
