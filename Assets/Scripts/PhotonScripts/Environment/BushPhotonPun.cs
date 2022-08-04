using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BushPhotonPun : Bush
{
    private PhotonView _photonView;

    public void OnEnable()
    {
        _photonView = GetComponent<PhotonView>();
    }
    
    protected override void ObjectDie()
    {
        _photonView.RPC(nameof(RPC_DestroySource), RpcTarget.MasterClient);
    }
    
    [PunRPC]
    private void RPC_DestroySource()
    {
        PhotonNetwork.Destroy(gameObject);
    }
    
    protected override void DecreaseResource()
    {
        _photonView.RPC(nameof(RPC_DecreaseResource), RpcTarget.All);
    }
    
    [PunRPC]
    private void RPC_DecreaseResource()
    {
        res--;
    }
    
    protected override void AdjustIndicator()
    {
        _photonView.RPC(nameof(RPC_AdjustIndicator), RpcTarget.All);
    }
    
    [PunRPC]
    private void RPC_AdjustIndicator()
    {
        DTimeSprite = DTimeSpriteMax;
        spInd++;
        if (spInd < sp.Length - 1)
            resSp.sprite = sp[spInd];
    }
}
