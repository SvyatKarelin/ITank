using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Artillery : Enemy
{
    //protected override Transform FindShelter(float Radius) => 
    //    GetAllShelters(Radius).OrderByDescending(Col => Vector3.Distance(Col.transform.position, GetTarget().position)).ToList()[0].transform;

    private void Update()
    {
        GetShelterPos(200, out Vector3 SafePos, out Vector3 ShootPos);
        if (SafePos != Vector3.zero) agent.SetDestination(SafePos);
        else agent.SetDestination(GetTarget().position + (transform.position - GetTarget().position).normalized * 1);

        cannon.LookAt(GetTarget());
        if (cannon.IsAimed) cannon.Shoot();
    }
}
