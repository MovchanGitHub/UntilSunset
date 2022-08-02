using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ResourceScriptPhoton : ResourceScript
{
    private PhotonView _photonView;

    protected override void Start()
    {
        _photonView = GetComponent<PhotonView>();
        base.Start();
    }
    
    protected override void ObjectDie()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
