using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Photon.Pun;
using UnityEngine;

public class BuildingPlacePhotonPun : BuildPlace_1
{
    private PhotonView _photonView;

    private void Awake()
    {
        _photonView = GetComponent<PhotonView>();
    }

    protected override void InstantiateStruct()
    {
        _photonView.RPC(nameof(RPC_InstantiateStruct), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_InstantiateStruct()
    {
        base.InstantiateStruct();
    }

    protected override void InstantiateWall()
    {
        _photonView.RPC(nameof(RPC_InstantiateWall), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_InstantiateWall()
    {
        base.InstantiateWall();
    }

    public override void BrokenStakes()
    {
        _photonView.RPC(nameof(RPC_BrokenStakes), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_BrokenStakes()
    {
        base.BrokenStakes();
    }
}
