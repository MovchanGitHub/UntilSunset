using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class LevelGeneration : MonoBehaviour
{
    public SpawnSpot[] spots;

    public GameObject stone;
    public int stonesAmount;

    public GameObject tree;
    public int treesAmount;

    public GameObject bush;
    public int bushAmount;

    public GameObject tombstone;
    public int tombstoneAmount;

    protected virtual void Start()
    {
#if UNITY_ANDROID
        SetPEValues(); // for Pocket Edition
#endif

        int generalAmount = stonesAmount + treesAmount + bushAmount + tombstoneAmount;
        var randomShuffle = Shuffle(generalAmount, spots.Length);

        int spawnedAmount = 0;

        foreach (var randInd in randomShuffle)
        {
            if (spawnedAmount < treesAmount)
                SpawnTree(randInd);
            else if (spawnedAmount < treesAmount + stonesAmount)
                SpawnStone(randInd);
            else if (spawnedAmount < treesAmount + stonesAmount + bushAmount)
                SpawnBush(randInd);
            else
                SpawnTombstone(randInd);

            spawnedAmount++;
        }

    }

    protected Vector3 CalculateTreePosition(GameObject ob)
    {
        return new Vector3(ob.transform.position.x, ob.transform.position.y + 1f, ob.transform.position.z);
    }

    protected virtual void SpawnTree(int seed)
    {
        Instantiate(tree, CalculateTreePosition(spots[seed].points[Random.Range(0, 4)]), Quaternion.identity);
    }

    protected Vector3 CalculateStonePosition(GameObject ob)
    {
        return ob.transform.position;
    }

    protected virtual void SpawnStone(int seed)
    {
        Instantiate(stone, CalculateStonePosition(spots[seed].points[Random.Range(0, 4)]), Quaternion.identity);
    }

    protected Vector3 CalculateBushPosition(GameObject ob)
    {
        return new Vector3(ob.transform.position.x, ob.transform.position.y, ob.transform.position.z);
    }

    protected virtual void SpawnBush(int seed)
    {
        Instantiate(bush, CalculateBushPosition(spots[seed].points[Random.Range(0, 4)]), Quaternion.identity);
    }

    protected Vector3 CalculateTombstonePosition(GameObject ob)
    {
        return new Vector3(ob.transform.position.x, ob.transform.position.y - 0.5f, ob.transform.position.z);
    }

    protected virtual void SpawnTombstone(int seed)
    {
        Instantiate(tombstone, CalculateTombstonePosition(spots[seed].points[Random.Range(0, 4)]), Quaternion.identity);
    }

    private HashSet<int> Shuffle(int need, int have)
    {
        need = Min(need, have); // ����� ����� �� ���� � ������ �� ������ ������� ������

        HashSet<int> result = new HashSet<int>();

        while (result.Count < need)
            result.Add(Random.Range(0, have));

        return result;
    }

    private void SetPEValues()
    {
        stonesAmount -= stonesAmount / 2;
        treesAmount -= treesAmount / 2;
        bushAmount -= bushAmount / 2;
        tombstoneAmount -= tombstoneAmount / 2;
    }
}
