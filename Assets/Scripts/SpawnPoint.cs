using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int spawnCount;
    public GameObject itemPrefab;
    public float spawnTime;
    private float currentTime;

    private void Update()
    {
        if (spawnCount < 1 || itemPrefab == null)
        {
            enabled = false;
            return;
        }
        currentTime += Time.deltaTime;
        if(currentTime > spawnTime)
        {
            currentTime = 0;
            spawnCount--;
            Instantiate(itemPrefab, transform.position, transform.rotation);
        }
    }

}
