using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerStats : NetworkBehaviour
{
    public static int gear;
    public int initialGear;
    public static int energy;
    public int initialEnergy = 100;
    public static int score;
    public int initialScore;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;
        gear = initialGear;
        energy = initialEnergy;
        score = initialScore;
    }
     
}
