using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawnTransform : NetworkBehaviour
{
    [SerializeField] List<Transform> spawnTransformList = new List<Transform>();

    SpawnerPosition[] spawners;
    SpawnerPosition spawner;
    public override void OnNetworkSpawn()
    {
        if (!IsOwner) { return; }
        spawner = FindAnyObjectByType<SpawnerPosition>();
        spawners = FindObjectsOfType<SpawnerPosition>();

        foreach (var item in spawners)
        {
            item.GetComponent<SpawnerPosition>();
            spawnTransformList.Add(item.transform);
        }

        int index = 0;
        while (index < spawnTransformList.Count)
        {
            if (OwnerClientId.ConvertTo<int>() == index)
            {
                transform.position = spawnTransformList[index].position;
            }
            index++;
        }
       
    }
}
