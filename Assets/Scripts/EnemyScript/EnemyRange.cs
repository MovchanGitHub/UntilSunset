using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRange : EnemyCharacter
{
    public Building target;
    public Vector2 targetPoint;

    public Animator animator;

    public GameObject projectilePrefab;

    //������ �������� ��� �������
    private float CalcForce(float x0, float x1)
    {
        float dist = System.Math.Abs(x0 - x1) - 0.5f;
        float D = (float)System.Math.Sqrt(0.43 * 0.43 - 4 * 0.173 * (-2.58 - dist));
        return (-0.43f + D) / (2f * 0.173f);
    }

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if ( tag == "Wall1" || tag == "Wall2")
        {
            Debug.Log("idle");
            animator.Play("Idle");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == "Wall1" || tag == "Wall2")
        {
            animator.Play("Movement");
        }
    }

    public void DoDamage(Collider2D collision)
    {
        if (target)
        {
            //hitTimer -= Time.deltaTime;

            //Debug.Log(hitTimer);
            if (hitTimer <= 0)
            {
                this.speed = 1f;

                //Debug.Log("ATACKING", target);
                GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
                EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();

                projectile.Launch(direction * CalcForce(transform.position.x, targetPoint.x), this.damage);
                hitTimer = hitPeriod;
            }
            else if (hitTimer <= 1f)
            {
                animator.Play("Hit");
                this.speed = 0.0001f;
            }
        }
    }
}
