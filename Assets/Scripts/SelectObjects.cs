using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObjects : MonoBehaviour
{

    private bool draw;
    private Vector2 startPos;
    private Vector2 endPos;
    private float minY = 0;
    private CharacterManager selected;
    private List<Vector3> path;

    private void Update()
    {
      //  TryDrawSelectBox();
    }

    private void TryDrawSelectBox()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log("star");

            startPos = Input.mousePosition;
            draw = true;

        }

        if (Input.GetMouseButtonUp(0))
        {

            Debug.Log("finish");

            draw = false;
          //  HighlightSelected();
        }

        if (draw)
        {
            Debug.Log("draw");
            //if (!CheckPosition(startPos, endPos)) return;
            //endPos = Input.mousePosition;
            //if (startPos == endPos) return;
            //endPos = GetAvailablePosition(endPos);
            //GetSelected().Clear();

            //DrawSelectBox();
        }
    }
}