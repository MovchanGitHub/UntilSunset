using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats:MonoBehaviour
{
    private static int level;

    private static int encounter;

    public static List<SpawnerScript> spawnerList;

    public static List<EnemyCharacter> enemyOnScreen;

    public static int Encounter
    {
        get { return encounter; }
        private set { encounter = value; }
    }

    public static int Level
    {
        get { return level; }
    }

    private static int coins = 0;
    public static int Coins
    {
        get { return coins; }
        set { coins = value; }
    }


    void Awake()
    {
        Encounter = 1;
        level = 1;
        spawnerList = new List<SpawnerScript>();
        enemyOnScreen = new List<EnemyCharacter>();
    }
}
