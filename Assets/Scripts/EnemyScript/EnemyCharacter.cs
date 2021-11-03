using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyCharacter: MonoBehaviour
{
    public string _name; // ���
    public int price; // �������� ��������� ������
    public int maxHealth = 2; //��������
    public float speed = 1.0f; //�������� ��������
    public int armor = 0; //�����
    public int damage = 1; //����
    public float immunityPeriod = 2.0f; // ������������� ��������� �����
    public float hitPeriod = 5.0f; // ������������� ��������� �����
    protected int direction = 1; //�����������
    protected int currentHealth; //������� ��������
    public float immunityTimer; //������� ������������
    protected float hitTimer; //������� ������� ��������� �����

    private bool enterMainBuilding = false;

    public LayerMask aviableHitMask;

    protected Rigidbody2D rigidbody2d;

    public int health 
    { 
        get { return currentHealth; } 
    }

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        immunityTimer = 0;
        hitTimer = 0;
    }

    // Update is called once per frame
    private void Update()
    {
        if (immunityTimer > 0)
        {
            immunityTimer -= Time.deltaTime;
        }
        if (hitTimer > 0)
        { 
            hitTimer -= Time.deltaTime;
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        if (enterMainBuilding)
        {
            position.y = position.y + Time.deltaTime * speed;
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
            
        }
        rigidbody2d.MovePosition(position);
        if (position.y > 2)
        {
            Destroy(gameObject);
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

    public void EnterMainBuilding()
    {
        enterMainBuilding = true;
        rigidbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
