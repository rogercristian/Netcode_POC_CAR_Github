using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI playerVelocimeter;
    CarController carController;
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
       // if (!IsOwner) enabled = false;

    

        VelocimeterServerRpc();
    }

    private void Update()
    {
        VelocimeterServerRpc();
    }

    
    /** */

    [ServerRpc(RequireOwnership = false)]
    public void VelocimeterServerRpc()
    {
        VelocimeterClientRpc();
    }

    [ClientRpc]
    public void VelocimeterClientRpc()
    {
        if (!IsOwner) enabled = false;

        
            carController = FindObjectOfType<CarController>();

            playerVelocimeter.text = carController.Velocimeter().ToString();
        

    }
}
