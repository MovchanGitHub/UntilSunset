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
        _photonView.RPC(nameof(RPC_InstantiateStruct), RpcTarget.AllViaServer, structNumber);
    }

    [PunRPC]
    private void RPC_InstantiateStruct(int structIndex)
    {
        GameObject structinst = null;
        switch (structIndex)
        {
            case 1:
                structinst = Instantiate(BuildingUIScript.instance.wll1, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);
                break;
            case 2:
                structinst = Instantiate(BuildingUIScript.instance.stakes, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);
                break;
            case 3:
                structinst = Instantiate(BuildingUIScript.instance.tower, new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), transform.rotation);
                break;
        }
        
        structinst.transform.SetParent(this.transform);
        source.PlayOneShot(CBuild, 0.2f);
    }

    /*protected override void InstantiateWall()
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
    }*/
}
