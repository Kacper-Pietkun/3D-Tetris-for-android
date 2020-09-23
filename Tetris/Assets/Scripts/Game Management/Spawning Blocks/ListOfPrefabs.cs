using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfPrefabs : MonoBehaviour
{
    [SerializeField]
    private GameObject[] prefabs;

    // This function will be used by spawner to randomly choose one of seven prefabs, then spawner will create it in game
    public GameObject GetOnePrefab()
    {
        int index;
        index = Random.Range(0, prefabs.Length);

        return prefabs[index];
    }
}
