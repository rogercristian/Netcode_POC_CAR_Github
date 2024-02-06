using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    //[SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private string nameToPlayer = "";
    //private NetworkVariable<FixedString128Bytes> playerNetworkName = new NetworkVariable<FixedString128Bytes>(
    //    "Player 0", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

    public static int gear;
    [HideInInspector] public int initialGear;
    public static int energy;
    public int initialEnergy = 100;
    public static int score;
    [HideInInspector] public int initialScore;
    public static string playerGetName;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;

        //playerNetworkName.Value = nameToPlayer + (OwnerClientId);
        //playerName.text = playerNetworkName.Value.ToString();
        gear = initialGear;
        energy = initialEnergy;
        score = initialScore;
        playerGetName = nameToPlayer;
    }
 
}
