using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VictoryPoint : MonoBehaviour
{
    public Renderer meshBody;
    public Material captureMaterial;
    public UnityEvent Capture = new UnityEvent();
    public CharacterManager attached;
    public bool isCapture;

    private void OnTriggerStay(Collider other)
    {
        CharacterManager playerUnit = CheckPlayerUnit(other);
        if (!isCapture && playerUnit != null)
        {
            isCapture = true;
            SwitchToTeam(playerUnit.health);
            ChangeColor();
            Capture?.Invoke();
        }
    }

    private CharacterManager CheckPlayerUnit(Collider other)
    {
        CharacterManager ch = other.GetComponent<CharacterManager>();
        if (ch != null && ch.GetTeam() == Team.Player1)
        {
            return ch;
        }
        return null;
    }


    public void ChangeColor()
    {
        meshBody.material = captureMaterial;
    }

    private void SwitchToTeam(Health newOwner)
    {
        if (attached == null) return;
        attached.health.SetTeam(newOwner.GetTeam());
      //  attached.command = new GuardCommand(attached);
    }

}
