using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour
{
    public CharacterManager character;

    void Update()
    {
        if (!IsPointerOverUIObject())
        {
            CheckLeftClick();
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData e = new PointerEventData(EventSystem.current);
        e.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(e, results);
        return results.Count > 0;
    }

    private void CheckLeftClick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            CharacterManager unitTarget;
            if (Physics.Raycast(mouseRay, out RaycastHit mouseHit, 100))
            {
                //attack
                unitTarget = mouseHit.collider.GetComponent<CharacterManager>();
                if (unitTarget)
                {
                    OnClickRightObject(unitTarget);
                    return;
                }

                //move
                Collider filterTerrain = mouseHit.collider.GetComponent(typeof(Collider)) as Collider;
                if (filterTerrain)
                {
                    OnClickRight(mouseHit.point);
                    return;
                }
            }
        }
    }

    private void OnClickRightObject(CharacterManager target)
    {
        character.armament.Attack(target.health);
    }

    private void OnClickRight(Vector3 point)
    {
        character.SetTargetMovement(point);
    }
}
