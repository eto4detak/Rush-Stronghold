using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    public Sprite myHealthSpriteBar;


    private void UpdateRotation()
    {
        //if(transform.parent.gameObject.GetComponent<Unit>().Selected == true)
        //{
        //    if (myHealthSpriteBar. == true)
        //    {
        //        HealthSpriteManager.ShowSprite(myHealthSpriteBar);
        //        HealthSpriteManager.UpdateBounds();
        //    }

        //    transform.eulerAngles = new Vector3(
        //        Camera.main.transform.eulerAngles.x,
        //        Camera.main.transform.parent.gameObject.transform.eulerAngles.y,
        //        Camera.main.transform.eulerAngles.z
        //        );
        //}
        //else
        //{
        //    if (myHealthSpriteBar.hidden == false)
        //    {
        //        HealthSpriteManager.HideSprite(myHealthSpriteBar);
        //    }
        //}
    }

    private void UpdateRotation2()
    {
        var camPosition = Camera.main.transform.position;
        transform.LookAt(Camera.main.transform.position);
        // var wantedPos = Camera.main.WorldToViewportPoint(GManager.character.transform.position);
    }

}
