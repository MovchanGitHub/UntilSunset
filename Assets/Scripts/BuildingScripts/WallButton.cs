using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallButton : MonoBehaviour
{
    private Wall wl;
    private GameObject upginfo;
    private Resources resources;

    public void Start()
    {
        wl = transform.parent.GetComponent<Wall>();
        resources = GameObject.Find("CoinsText").GetComponent<Resources>();
    }

    public void WallUpgButtonPressed()
    {
        wl.UpgradeWall();
    }

    public void WallRecoverButtonPressed()
    {
        wl.Recover();
    }

    public void WallDelButtonPressed()
    {
        wl.DestroyWall();
    }

    public void MouseEnterButtonUpg(Button b)
    {
        //resources.SetChangeWood(1);
        resources.UpdateAll();
    }

    public void MouseExitButtonUpg(Button b)
    {
        //resources.ClearChangeWood();
        resources.UpdateAll();
    }
}