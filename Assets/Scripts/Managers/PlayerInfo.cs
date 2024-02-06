using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;

public class PlayerInfo : NetworkBehaviour
{
    private NetworkVariable<FixedString128Bytes> playerNetworkName = new NetworkVariable<FixedString128Bytes>(
       "Player 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    [SerializeField] private TextMeshProUGUI playerName;
   
    // Start is called before the first frame update
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;

        playerNetworkName.Value = PlayerStats.playerGetName + " " + (OwnerClientId);
        playerName.text = playerNetworkName.Value.ToString();

    }
}
