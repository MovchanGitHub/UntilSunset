using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUIScript : MonoBehaviour
{
    public GameObject wll1;
    public GameObject wll1Ghost;
    public GameObject stakes;
    public GameObject stakesGhost;
    public GameObject tower;
    public GameObject towerGhost;

    public static BuildingUIScript instance;

    private void Awake()
    {
        instance = this;
    }

    public void BuildWallButton()
    {
        if (!BuildPlace_1.paused)
        {
            BuildPlace_1.obj_struct = wll1;
            BuildPlace_1.obj_ghost = wll1Ghost;
            BuildPlace_1.obj_price_wood = 3;
            BuildPlace_1.obj_price_stone = 0;
            BuildPlace_1.structNumber = 1;
        }
    }

    public void BuildStakesButton()
    {
        if (!BuildPlace_1.paused)
        {
            BuildPlace_1.obj_struct = stakes;
            BuildPlace_1.obj_ghost = stakesGhost;
            BuildPlace_1.obj_price_wood = 1;
            BuildPlace_1.obj_price_stone = 0;
            BuildPlace_1.structNumber = 2;
        }
    }

    public void BuildTowerButton()
    {
        if (!BuildPlace_1.paused)
        {
            BuildPlace_1.obj_struct = tower;
            BuildPlace_1.obj_ghost = towerGhost;
            BuildPlace_1.obj_price_wood = 4;
            BuildPlace_1.obj_price_stone = 2;
            BuildPlace_1.structNumber = 3;
        }
    }

    public void NullStruct()
    {
        BuildPlace_1.obj_struct = null;
        BuildPlace_1.obj_ghost = null;
        BuildPlace_1.obj_price_wood = 0;
        BuildPlace_1.obj_price_stone = 0;
        BuildPlace_1.structNumber = 0;
    }

    public void DestroyTool()
    {
        NullStruct();
        Building.tool = 3;
    }
}
