using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MainBuilding : MonoBehaviour
{
    public GameObject facade = null;

    private bool isTriggered;

    SpriteRenderer sprite;

    public bool isTutorial;

    void Start()
    {
        sprite = facade.GetComponent<SpriteRenderer>();
        Color color = sprite.material.color;
        color.a = isTutorial? 1f : 0f;
        sprite.material.color = color;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyCharacter enemy = collision.gameObject.GetComponent<EnemyCharacter>();
        if (enemy)
        {
            enemy.EnterMainBuilding();
            transform.GetChild(2).GetComponent<Coffin>().RecieveDamage(enemy.damage, DamageType.close_combat);
        }

        PlayerControllerPhoton player = collision.gameObject.GetComponent<PlayerControllerPhoton>();
        if (player && player.photonView.IsMine)
        {
            player.EnterMainBuilding();
            isTriggered = true;
            StartCoroutine(nameof(MakeInvisible));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerControllerPhoton player = collision.gameObject.GetComponent<PlayerControllerPhoton>();
        if (player && player.photonView.IsMine)
        {
            player.ExitMainBuilding();
            isTriggered = false;
            StartCoroutine(nameof(MakeVisible));
        }
    }

    IEnumerator MakeVisible()
    {
        float f = sprite.material.color.a;
        while (f < 1f && !isTriggered)
        {
            f = System.Math.Min(f + 0.05f, 1f);
            Color color = sprite.material.color;
            color.a = f;
            sprite.material.color = color;
            yield return new WaitForSeconds(0.02f);
        }
    }

    IEnumerator MakeInvisible()
    {
        float f = sprite.material.color.a;
        while (isTriggered && f > 0f)
        {
            f = System.Math.Max(f - 0.05f, 0f);
            Color color = sprite.material.color;
            color.a = f;
            sprite.material.color = color;
            yield return new WaitForSeconds(0.02f);
        }
    }
}
