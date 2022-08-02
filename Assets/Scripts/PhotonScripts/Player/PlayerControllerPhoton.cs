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
        
        base.Awake();
    }

    protected override void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rigidbBody2D);
        }
    }

    protected override void Update()
    {
        if (!photonView.IsMine)
            return;
        
        base.Update();
    }
    
    protected override void FixedUpdate()
    {
        if (!photonView.IsMine)
            return;
        
        base.FixedUpdate();
    }
}
