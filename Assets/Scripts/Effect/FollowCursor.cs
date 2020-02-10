using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    private TrailRenderer trail;


    private void Start()
    {
        trail = GetComponent<TrailRenderer>();
       // Cursor.visible = false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            trail.startColor = Color.green;
            trail.endWidth = 3;
        }
        //Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //transform.position = pos;
    }
    
}
