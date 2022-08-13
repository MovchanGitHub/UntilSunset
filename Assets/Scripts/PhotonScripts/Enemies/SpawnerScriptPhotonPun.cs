using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;
using Random = UnityEngine.Random;

public class SpawnerScriptPhotonPun : SpawnerScript
{
    private PhotonView _photonView;
    public List<string> enemiesNames;
    private GameObject enemyObject;
    private void Start()
    {
        if (!PhotonNetwork.IsMasterClient)
            Destroy(this);

        _photonView = GetComponent<PhotonView>();
    }

    protected override void Update()
    {
        spawnTime -= Time.deltaTime;

        if ((currentSpawned < spawnCount) && (spawnTime <= 0))
        {
            int line = Random.Range(0, 3);
            enemyObject = PhotonNetwork.Instantiate(ChooseEnemyName(), 
                new Vector3(transform.position.x, transform.position.y+line+0.25f,
                    transform.position.z), transform.rotation);
            spawnTime = spawnRate;
        }
    }

    private string ChooseEnemyName()
    {
        string res;
        
        int ind = Random.Range(0, usedEnemies.Count); 
        res = enemiesNames[usedEnemies[ind]]; 
        usedEnemies.RemoveAt(ind);
        currentSpawned++;
        
        return Path.Combine("Enemies", res);
    }
}