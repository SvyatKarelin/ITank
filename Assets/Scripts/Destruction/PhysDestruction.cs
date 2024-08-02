using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysDestruction : DestructionObjects
{
    [SerializeField] float ExplosionStenght = 100;
    [SerializeField] float ExplosionRadius = 100;

    Rigidbody MakePhysical(Transform Obj)
    {
        Utilits.CheckComponent<BoxCollider>(Obj);
        Rigidbody rb = Utilits.CheckComponent<Rigidbody>(Obj);
        return rb;
    }
    protected void PhysDestroy(Transform Obj, Transform Center)
    {
        Rigidbody rb = MakePhysical(Obj);
        rb.AddExplosionForce(ExplosionStenght, Center.position, ExplosionRadius, ExplosionStenght/2); //rb.velocity = (Obj.transform.position - Center.position) * ExplosionStenght;
    }

    protected void PhysDestroy(Transform Obj)
    {
        Rigidbody rb = MakePhysical(Obj);
        rb.AddExplosionForce(ExplosionStenght, transform.position, ExplosionRadius, ExplosionStenght/2); //Obj.transform.position - transform.position) * ExplosionStenght;
    }

    public override void Destroy()
    {
        PhysDestroy(transform);
    }
}
