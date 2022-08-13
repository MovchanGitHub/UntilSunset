using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Photon.Pun;
using UnityEngine;

public class Wall : Building
{
    public GameObject dialogBox;
    public GameObject repairButton;
    public GameObject upgradeButton;
    private bool upgraded = false;
    private Resources resources;
    public GameObject nextwall;
    private BuildPlace_1 bp;
    public AudioSource source;

    public int rep_wood_cost;
    public int rep_stone_cost;
    public int upg_wall_cost;
    public int upg_stone_cost;
    public int del_wood_re;
    public int del_stone_re;
    public AudioClip CDestroy;
    public AudioClip CRecover;
    public AudioClip CUpgrade;

    public BuildingPlacePhotonPun parentScript;

    protected override void Start()
    {
        resources = GameObject.Find("CoinsText").GetComponent<Resources>();
        source = GetComponent<AudioSource>();
        bp = transform.parent.GetComponent<BuildPlace_1>();
        bp.GetComponent<BoxCollider2D>().enabled = false;
        base.Start();
        parentScript = GetComponentInParent<BuildingPlacePhotonPun>();
    }

    private void Update()
    {
        if (pausecl)
        {
            HideDialog();
            pausecl = true;
        }
    }

    public void DestroyWall()
    {
        GameStats.Wood += del_wood_re;
        GameStats.Stone += del_stone_re;
        resources.UpdateWood();
        resources.UpdateStones();

        if (parentScript != null)
        {
            parentScript.DestroyBuilding();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnDestroy()
    {
        resources.ClearPriceOrRefund();
        resources.UpdateAll();
        HideDialog();
        if (!this.gameObject.scene.isLoaded) return;
        if (!upgraded)
        {
            bp.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    public void Recover()
    {
        resources = GameObject.Find("CoinsText").GetComponent<Resources>();
        if ((GameStats.Wood >= rep_wood_cost) && (GameStats.Stone >= rep_stone_cost) && (health != maxHealth))
        {
            source.PlayOneShot(CRecover, 0.5f);
            GameStats.Wood -= rep_wood_cost;
            GameStats.Stone -= rep_stone_cost;
            resources.UpdateWood();
            resources.UpdateStones();
            HideDialog();
            
            if (parentScript != null)
            {
                parentScript.RecoverBuilding();
            }
            else
            {
                health = maxHealth;
            }
            
            transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<WallHPBar>().SetValue(health / (float)maxHealth);
        }
    }

    public void UpgradeWall()
    {
        if ((GameStats.Wood >= upg_wall_cost) && (GameStats.Stone >= upg_stone_cost) )
        {
            upgraded = true;
            GameStats.Wood -= upg_wall_cost;
            GameStats.Stone -= upg_stone_cost;
            resources.UpdateWood();
            resources.UpdateStones();
            HideDialog();

            if (parentScript != null)
            {
                parentScript.UpgradeWall();
            }
            else
            {
                var wall2inst = Instantiate(nextwall, transform.position, transform.rotation);
                wall2inst.transform.SetParent(transform.parent.transform);
                Destroy(gameObject);
            }
        }
    }

    public void DisplayDialog()
    {
        if (!pause)
        {
            dialogBox.SetActive(true);
            if (transform.tag != "Wall3")
            {
                if (maxHealth == health)
                {
                    upgradeButton.SetActive(true);
                }
                else
                {
                    repairButton.SetActive(true);
                }
            }
        }
    }

    public void HideDialog()
    {
        dialogBox.SetActive(false);
        if (transform.tag != "Wall3")
        {
            upgradeButton.SetActive(false);
            repairButton.SetActive(false);
        }
    }

    public void UpdateDialog()
    {
        if (dialogBox.activeSelf)
        {
            if (maxHealth == health)
            {
                repairButton.SetActive(false);
                upgradeButton.SetActive(true);
            }
            else if (!repairButton.activeSelf)
            {
                repairButton.SetActive(true);
                upgradeButton.SetActive(false);
            }
        }
    }

    public void UpdateHelthBar()
    {
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<WallHPBar>().SetValue(health / (float)maxHealth);
    }
}
