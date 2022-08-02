using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class StonePhotonPun : Stone
{
    private PhotonView _photonView;

    protected override void Start()
    {
        _photonView = GetComponent<PhotonView>();
        base.Start();
    }
    
    protected override void ObjectDie()
    {
        Debug.Log(PhotonNetwork.IsMasterClient ? "The master client has destroyed a stone."
            : "Somebody (not the master client) has destroyed a stone.");
        
        _photonView.RPC(nameof(RPC_DestroyStone), RpcTarget.MasterClient);
    }

    [PunRPC]
    private void RPC_DestroyStone()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
