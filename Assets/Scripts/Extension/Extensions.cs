using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtension
{
    public static Transform GetClosest(this Transform fromThis, List<Transform> stack)
    {
        Transform closest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 directionToTarget;
        for (int i = 0; i < stack.Count; i++)
        {
            directionToTarget = stack[i].transform.position - fromThis.transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closest = stack[i];
            }
        }
        return closest;
    }

    public static Component GetClosest<T>(this Component fromThis, List<T> stack) where T : Component
    {
        Component closest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 directionToTarget;
        for (int i = 0; i < stack.Count; i++)
        {
            directionToTarget = stack[i].transform.position - fromThis.transform.position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closest = stack[i];
            }
        }
        return closest;
    }

    public static Component GetClosest<T>(Vector3 fromThis, List<T> stack) where T : Component
    {
        Component closest = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 directionToTarget;
        for (int i = 0; i < stack.Count; i++)
        {
            if (stack[i] == null) continue;
            directionToTarget = stack[i].transform.position - fromThis;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                closest = stack[i];
            }
        }
        return closest;
    }
    public static float SumPath(List<Vector3> stack)
    {
        float lenght = 0;
        for (int i = 1; i < stack.Count; i++)
        {
            lenght += (stack[i] - stack[i - 1]).magnitude;
        }
        return lenght;
    }

}

