using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heli : Enemy
{
    [SerializeField] private Transform SUS;
    public float CurAngle = 0;
    Vector3 MoveNextOnCircle(Vector3 Center, Vector3 CurPos, float TargetRadius)
    {
        CurAngle = Quaternion.LookRotation(CurPos - Center).eulerAngles.y;//угол относительно центра окружности
        CurAngle += 20;
        return Center + new Vector3(TargetRadius * Mathf.Sin(CurAngle * Mathf.Deg2Rad), 0f, TargetRadius * Mathf.Cos(CurAngle * Mathf.Deg2Rad));//переволдим обратно в декартову
    }

    public void Update()
    {
        MoveNextOnCircle(GetTarget().position, transform.position, 50);
        //else MoveNextOnCircle(GetTarget().position, transform.position, 20);
        //cannon.LookAt(GetTarget());
        //if (RaycastBetweenObj(transform, GetTarget(), new Vector3(0f, 2f, 0f)).Length <= 0 && cannon.IsAimed) cannon.Shoot();
    }
}
