using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public int spawnCount;
    public CharacterManager itemPrefab;
    public float spawnTime;
    public GameObject[] points;
    public Team team;

    private bool starting;
    private float currentTime;

    private void Update()
    {
        if (!starting) return;
        if (spawnCount < 1 || itemPrefab == null)
        {
            enabled = false;
            return;
        }
        currentTime += Time.deltaTime;
        if(currentTime > spawnTime)
        {
            GameObject point = points[Random.Range(0, points.Length-1)];
            Collider[] units = Physics.OverlapSphere(point.transform.position + new Vector3(0, 0.6f, 0), 0.5f);
            if(units.Length == 0)
            {
                CharacterManager tempCh = Instantiate(itemPrefab, point.transform.position, point.transform.rotation);
                tempCh.health.SetTeam(team);
                tempCh.Command = new FindEnemyCommand(tempCh);

                currentTime = 0;
                spawnCount--;
            }
        }
    }

    public void SetLoop(bool loop)
    {
        starting = loop;
    }

}
