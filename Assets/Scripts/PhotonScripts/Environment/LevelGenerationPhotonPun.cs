using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class LevelGenerationPhotonPun : LevelGeneration
{
    protected override void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            return;
        
        base.Start();
    }
    
    protected override void SpawnTree(int seed)
    {
        PhotonNetwork.InstantiateRoomObject(Path.Combine("Environment", "PNTree"), 
            CalculateTreePosition(spots[seed].points[Random.Range(0, 4)]), Quaternion.identity);
    }
    
    protected override void SpawnStone(int seed)
    {
        PhotonNetwork.InstantiateRoomObject(Path.Combine("Environment", "PNStone"), 
            CalculateStonePosition(spots[seed].points[Random.Range(0, 4)]), Quaternion.identity);
    }
    
    protected override void SpawnBush(int seed)
    {
        PhotonNetwork.InstantiateRoomObject(Path.Combine("Environment", "PNBush"), 
            CalculateBushPosition(spots[seed].points[Random.Range(0, 4)]), Quaternion.identity);
    }
    
    protected override void SpawnTombstone(int seed)
    {
        PhotonNetwork.InstantiateRoomObject(Path.Combine("Environment", "PNTombstone"), 
            CalculateTombstonePosition(spots[seed].points[Random.Range(0, 4)]), Quaternion.identity);
    }
    
}
