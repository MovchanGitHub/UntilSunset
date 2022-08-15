using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PauseMenuPhoton : PauseMenu
{
    private PhotonView _photonView;

    private void Start()
    {
        PlayerController = FindObjectOfType<PlayerController>();
        _photonView = GetComponent<PhotonView>();
    }

    protected override void Pause()
    {
        pauseMenuUI.SetActive(true);
        GameIsPaused = true;
        BuildPlace_1.PauseBuilding();
        Building.PauseBuildingUI();
        res = GameObject.Find("CoinsText").GetComponent<Resources>();
        res.ClearPriceOrRefund();
        res.UpdateAll();
    }
    
    protected override void Resume()
    {
        _photonView.RPC(nameof(RPC_Resume), RpcTarget.All);
    }

    [PunRPC]
    private void RPC_Resume()
    {
        pauseMenuUI.SetActive(false);
        GameIsPaused = false;
        BuildPlace_1.ResumeBuilding();
        Building.ResumeBuildingUI();
    }
    
    protected override void RestartGame()
    {
        _photonView.RPC(nameof(RPC_RestartGame), RpcTarget.AllViaServer);
    }
    
    [PunRPC]
    private void RPC_RestartGame()
    {
        base.RestartGame();
    }
}
