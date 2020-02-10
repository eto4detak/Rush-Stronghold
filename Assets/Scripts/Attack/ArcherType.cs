using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherType : AttackType
{
    public Rigidbody arrowPrefab;
    public float velocity = 3f;
    public Transform firePoint;
    private Health owner;

    public override void Attack(Health enemy, Health _owner)
    {
        owner = _owner;
        if (owner == null) return;
        float deltaOffset = (owner.transform.position - enemy.transform.position).magnitude / 10;
        var arraow = GameObject.Instantiate(arrowPrefab, firePoint.transform.position, firePoint.transform.rotation);
        arraow.GetComponent<Renderer>().material = PController.instance.GetColor(owner.GetTeam());
        Vector3 offset = new Vector3(Random.Range(-deltaOffset, deltaOffset), 
            Random.Range(-deltaOffset, deltaOffset), Random.Range(-deltaOffset, deltaOffset));
        arraow.transform.LookAt(enemy.transform.position + new Vector3(0,2,0) + offset);
        arraow.AddForce(arraow.transform.forward * velocity, ForceMode.Impulse);
    }
    
}

