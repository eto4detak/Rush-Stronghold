using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherType : AttackType
{
    public Arrow arrowPrefab;
    public float velocity = 2f;
    public Transform firePoint;
    private Health owner;

    public override void Attack(Health enemy, Health _owner)
    {
        owner = _owner;
        if (owner == null) return;
        float distance = (owner.transform.position - enemy.transform.position).magnitude;
        float deltaOffset = distance / 20;
        var arraow = GameObject.Instantiate(arrowPrefab, firePoint.transform.position, firePoint.transform.rotation);
        arraow.Setup(damage);
        arraow.GetComponent<Renderer>().material = PController.instance.GetColor(owner.GetTeam());
        Vector3 offset = new Vector3(Random.Range(-deltaOffset, deltaOffset), 
            Random.Range(0, deltaOffset), Random.Range(-deltaOffset, deltaOffset));
        //Vector3 toCenter = new Vector3(0, 1, 0);
        Vector3 gravitiDistance = Vector3.up * distance * distance /100;

        //Debug.Log("enemy.GetCenter().position " + enemy.GetCenter().position);

        arraow.transform.LookAt(enemy.GetCenter().position + offset + gravitiDistance);
        arraow.rb.AddForce(arraow.transform.forward * velocity, ForceMode.Impulse);
    }


}

