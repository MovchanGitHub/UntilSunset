using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInstantiateChangeDirection : MonoBehaviour
{
    private Coffin _coffin;
    private EnemyCharacter _enemyCharacter;

    private void Awake()
    {
        _coffin = FindObjectOfType<Coffin>();
        _enemyCharacter = GetComponent<EnemyCharacter>();
        _enemyCharacter.direction = _coffin.transform.position.x > _enemyCharacter.transform.position.x ? 1 : -1;
    }
}
