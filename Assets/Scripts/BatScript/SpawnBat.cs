using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBat : Bat
{
    public static SpawnBat instance = null;
    public GameObject spawnPoint;
    public GameObject bat;
    public int maxBatOnScreen;//������������ ���-�� ����� �� ������
    public int totalBat;//������� ����� �� ������� ������ ������
    public int batPerSpawn;//���-�� ����� ������� ���������� ������������

    int batOnScreen = 0;//���-�� ����� �� ������ ������

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Spawn()
    {
        if(batPerSpawn>0 && batOnScreen <totalBat)
        {
            for(int i = 0;i< batPerSpawn; i++)
                if(batOnScreen<maxBatOnScreen)
                {
                    GameObject newbat = Instantiate(bat) as GameObject;
                    newbat.transform.position = spawnPoint.transform.position;

                    batOnScreen+=1;

                }
        }
    }
}
