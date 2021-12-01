using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour, IDamage
{
    public float speed = 2.5f;
    Vector2 position;
    public int line;

    [SerializeField] private int maxHealth = 2; //������������ ��������
    public int damage = 1; //����
    protected float immunityPeriod = 1.0f; // ������������� ��������� �����
    protected float hitPeriod = 5.0f; // ������������� ��������� �����
    protected int currentHealth; //������� ��������
    public float immunityTimer; //������� ������������
    protected float hitTimer; //������� ������� ��������� �����
    public float firstHitPeriod = 1.5f; // ����� �� ������� ��������� �����

    protected Rigidbody2D batt;

    public GameObject cofiin;


    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(transform.position.x, (float)System.Math.Round(transform.position.y));
        if(transform.position.y>-1.4 && transform.position.y<1.4)
        {
            line = (int)System.Math.Round(transform.position.y);
        }
        else line = 0;
        
        batt = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        immunityTimer = 0;
        hitTimer = firstHitPeriod;
    }

    // Update is called once per frame
    void Update()
    {
         if (GameStats.enemyOnScreen[line + 1].Count > 0)
             FindEnemy();
         else GoHome();

        if (immunityTimer > 0)
        {
            immunityTimer -= Time.deltaTime;
        }
    }

    public void FindEnemy()
    {
        List<EnemyCharacter> listOfEnemies = GameStats.enemyOnScreen[line + 1];
        float distancetoEnemy;
        EnemyCharacter nearEnemy = null;
        float minDistance = float.MaxValue;
        for (int i = 0; i < listOfEnemies.Count; i++)
        {
            distancetoEnemy = Vector2.Distance(transform.position, listOfEnemies[i].transform.position);
            if (minDistance > distancetoEnemy)
            {
                minDistance = distancetoEnemy;
                nearEnemy = listOfEnemies[i];
            }
        }

        if (nearEnemy)
            EnterBat(minDistance, nearEnemy);


    }

    void EnterBat(float minDistance, EnemyCharacter nearEnemy)
    {
        /*if (nearEnemy.transform.position.x < transform.position.x)
        {
            if (nearEnemy.transform.position.y < transform.position.y)
                batt.velocity = new Vector2(-speed , -speed ) ;
            else batt.velocity = new Vector2(-speed , speed ) ;
        }
        else
        {
            if (nearEnemy.transform.position.y < transform.position.y)
                batt.velocity = new Vector2(speed , -speed) ;
            else batt.velocity = new Vector2(speed , speed) ;
        }*/
        //batt.position = Vector2.Lerp(batt.position, nearEnemy.transform.position,speed*Time.deltaTime);

        batt.position = Vector3.MoveTowards(batt.position, nearEnemy.transform.position, speed * Time.deltaTime);
 
    }

    public void DoDamage(IDamage obj)
    {
        hitTimer -= Time.deltaTime;
        if ((obj != null) && (obj.Equals(typeof(Bat))))
        {
             if (hitTimer <= 0)
             {
                 obj.RecieveDamage(damage);
                 hitTimer = hitPeriod;
             }
        }
    }

    public void RecieveDamage(int amount)
    {
        if (immunityTimer <= 0)
        {
            currentHealth -= amount;
            if (currentHealth <= 0)
                Destroy(gameObject);
            immunityTimer = immunityPeriod;
        }
    }

    void GoHome()
    {
        //batt.position = Vector2.Lerp(batt.position,cofiin.transform.position, speed * Time.deltaTime);
        batt.position = Vector3.MoveTowards(batt.position, cofiin.transform.position, speed * Time.deltaTime);
    }

    public int GetLine()
    {
        return line;
    }
}