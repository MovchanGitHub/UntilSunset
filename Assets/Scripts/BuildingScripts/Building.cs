using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public static int tool;

    protected int _line;
    public int maxHealth;
    public static bool pause;
    public static bool pausecl;
    private int currentHealth;

    public int health
    {
        get { return currentHealth; }
        set { 
            currentHealth = value;
            UpdateInfo();
            if (transform.tag == "Wall1" || transform.tag == "Wall2")
            {
                transform.GetComponent<Wall>().UpdateDialog();
            }
        }
    }

    public int line
    {
        get { return _line; }
    }

    protected virtual void Start()
    {
        pause = false;
        pausecl = false;
        currentHealth = maxHealth;
        _line = (int)transform.position.y;
    }

    public void RecieveDamage(int amount, DamageType damageType)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
            if (gameObject.name == "Coffin")
                FindObjectOfType<PauseMenu>().Lose();
            Debug.Log(name + " has been destoyed");
        }
    }

    public void UpdateInfo()
    {
        if (transform.GetComponent<Wall>() && transform.tag != "Stakes")
        {
            transform.GetComponent<Wall>().UpdateHelthBar();
        }

        if (transform.GetComponent<TowerScript>() && transform.tag != "Stakes")
        {
            transform.GetComponent<TowerScript>().UpdateHelthBar();
        }
    }

    public int GetLine()
    {
        return line;
    }

    public static void PauseBuildingUI()
    {
        pause = true;
        pausecl = true;
    }

    public static void ResumeBuildingUI()
    {
        pause = false;
        pausecl = false;
    }
}
