using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerControllerPhoton : PlayerController
{
    private PhotonView photonView;
    private BoxCollider2D _boxCollider2D;
    
    protected override void Awake()
    {
        photonView = GetComponent<PhotonView>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        
        base.Awake();
    }

    protected override void Start()
    {
        if (!photonView.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rigidbBody2D);
            ChangeOtherPlayerOpacity();
            _boxCollider2D.enabled = false;
        }
        else
        {
            //SetGodSettings();
            nimb.SetActive(true);
        }
    }

    private void ChangeOtherPlayerOpacity()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color color = sprite.material.color;
        color.a = 0.7f;
        sprite.material.color = color;
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
