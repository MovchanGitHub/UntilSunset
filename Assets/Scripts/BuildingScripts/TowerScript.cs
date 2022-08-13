using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerScript : Building, IDamage
{
    private BuildPlace_1 bp;
    public GameObject arrow;
    private Resources resources;
    private GameObject arr;
    public bool et;
    public bool etback;
    public int sc;
    public int rep_wood_cost = 2;
    public int rep_stone_cost = 1;
    public int del_wood_re = 1;
    public int del_stone_re = 1;

    public float shotTimer;
    float defTime = 3;
    public GameObject dialogBox;
    
    public BuildingPlacePhotonPun parentScript;
    
    protected override void Start()
    {
        bp = transform.parent.GetComponent<BuildPlace_1>();
        et = false;
        sc = bp.direction;
        transform.GetChild(0).GetComponent<Transform>().localScale = 
            new Vector3(transform.GetChild(0).GetComponent<Transform>().localScale.x * sc, transform.GetChild(0).
                GetComponent<Transform>().localScale.y, transform.GetChild(0).GetComponent<Transform>().localScale.z);
        transform.localScale = new Vector3(transform.localScale.x * sc, transform.localScale.y, transform.localScale.z);
        bp.GetComponent<BoxCollider2D>().enabled = false;
        maxHealth = 30;
        parentScript = GetComponentInParent<BuildingPlacePhotonPun>();
        base.Start();
    }

    private void Update()
    {

        if (pausecl)
        {
            HideDialog();
            pausecl = true;
        }

        if ((shotTimer >= 0) && (et || etback))
        {
            shotTimer -= Time.deltaTime;
            if (shotTimer < 0)
            {
                if (et)
                {
                    Shot(1);
                    Debug.Log(1);
                }

                if (etback)
                {
                    Shot(-1);
                    Debug.Log(-1);
                }
                shotTimer = defTime;
            }
        }

        
    }

    public void Recover()
    {
        resources = GameObject.Find("CoinsText").GetComponent<Resources>();
        if ((GameStats.Wood >= rep_wood_cost) && (GameStats.Stone >= rep_stone_cost) && (health != maxHealth))
        {
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

    public void DoDamage(IDamage obj)
    {
        obj.RecieveDamage(10, DamageType.projectile);
    }

    public void DestroyStruct()
    {
        resources = GameObject.Find("CoinsText").GetComponent<Resources>();
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

    protected void OnDestroy()
    {
        bp.GetComponent<BoxCollider2D>().enabled = true;
    }

    public void Shot(int type)
    {
        Debug.Log(type);
        arr = Instantiate(arrow, new Vector3(transform.position.x, transform.position.y + 0.7f, transform.position.z), transform.rotation);
        arr.transform.GetComponent<Transform>().localScale = new Vector3(arr.transform.GetComponent<Transform>().localScale.x * type, arr.transform.GetComponent<Transform>().localScale.y, arr.transform.GetComponent<Transform>().localScale.z);
        if (type == -1)
        {
            arr.transform.GetComponent<ArrowScript>().direction = Vector3.right;
        }
        arr.transform.SetParent(this.transform);
    }

    public void DisplayDialog()
    {
        if (!pause)
        {
            dialogBox.SetActive(true);
        }
    }

    public void HideDialog()
    {
        dialogBox.SetActive(false);
    }

    public void UpdateHelthBar()
    {
        transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<WallHPBar>().SetValue(health / (float)maxHealth);
    }

}
