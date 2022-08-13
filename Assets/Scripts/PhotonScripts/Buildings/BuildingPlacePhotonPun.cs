using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Photon.Pun;
using UnityEngine;

public class BuildingPlacePhotonPun : BuildPlace_1
{
    private PhotonView _photonView;
    public GameObject structinst;

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
        structinst = null;
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

    public void DestroyBuilding()
    {
        _photonView.RPC(nameof(RPC_DestroyBuilding), RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void RPC_DestroyBuilding()
    {
        Destroy(structinst);
    }
    
    public void UpgradeWall()
    {
        _photonView.RPC(nameof(RPC_UpgradeWall), RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void RPC_UpgradeWall()
    {
        var wall2inst = Instantiate(structinst.GetComponent<Wall>().nextwall, 
            new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z), 
            transform.rotation);
        wall2inst.transform.SetParent(transform);
        Destroy(structinst);
        structinst = wall2inst;
    }
    
    public void RecoverBuilding()
    {
        _photonView.RPC(nameof(RPC_RecoverBuilding), RpcTarget.AllViaServer);
    }

    [PunRPC]
    private void RPC_RecoverBuilding()
    {
        Building building = structinst.GetComponent<Building>();
        building.health = building.maxHealth;
    }
    
    
}
