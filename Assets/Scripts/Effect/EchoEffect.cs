using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EchoEffect : MonoBehaviour
{
    private float timeBtwSpawn;
    public float startTimeBtwSpawn;

    public GameObject prefabEcho;
    //private Player player;

    void Start()
    {
        //player = GetComponent<Player>();    
    }

    void Update()
    {
        Spawn();
    }

    private void Spawn()
    {
        //if (player.moveInput == 0) return;
        if (timeBtwSpawn <= 0)
        {
           var echo =  Instantiate(prefabEcho, transform.position, Quaternion.identity);
            Destroy(echo, 4f);
            timeBtwSpawn = startTimeBtwSpawn;
        }
        else
        {
            timeBtwSpawn -= Time.deltaTime;
        }
    }

}
