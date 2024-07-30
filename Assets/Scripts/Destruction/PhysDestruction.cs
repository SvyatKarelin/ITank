using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysDestruction : DestructionObjects
{
    [SerializeField] float ExplosionStenght = 100;
    public override void TakeDamage(int Damage)
    {
        HealthPoints -= Damage;
        if (HealthPoints <= 0) PhysDestroy(gameObject, transform.parent);
    }

    Rigidbody MakePhysical(GameObject Obj)
    {
        if (!Obj.GetComponent<BoxCollider>()) Obj.AddComponent<BoxCollider>();
        Rigidbody rb = Obj.GetComponent<Rigidbody>();
        if (rb == null) rb = Obj.AddComponent<Rigidbody>() as Rigidbody;
        rb.mass = 100;
        return rb;
    }
    protected void PhysDestroy(GameObject Obj, Transform Center)
    {
        Rigidbody rb = MakePhysical(Obj);
        rb.velocity = (Obj.transform.position - Center.position) * ExplosionStenght;
    }

    protected void PhysDestroy(GameObject Obj)
    {
        Rigidbody rb = MakePhysical(Obj);
        rb.velocity = ( Obj.transform.position - transform.position) * ExplosionStenght;
    }
}
