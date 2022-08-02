using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerControllerPhoton : PlayerController
{
    private PhotonView photonView;
    
    protected override void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
            Destroy(this);
        
        base.Awake();
    }
}
