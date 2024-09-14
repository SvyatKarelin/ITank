using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DestructableVehicle : PhysDestruction
{
    [SerializeField, Range(0, 1)] float Protection = 1;
    void PartDestroy(Transform VehiclePart)
    {
        if (VehiclePart.GetComponent<Renderer>()) VehiclePart.GetComponent<Renderer>().material.color = new Color(0.215f, 0.215f, 0.215f, 1f);
        foreach (Behaviour C in VehiclePart.GetComponents<Behaviour>()) Destroy(C);
        PhysDestroy(VehiclePart);
        Destroy(VehiclePart.gameObject, 20);
    }
    public override void TakeDamage(int Damage)
    {
        HealthPoints -= (int)(Damage*Protection);
        if (HealthPoints <= 0) Destroy();
    }

    public void VehicleDestroy(Transform Vehicle)
    {
        PartDestroy(transform);
        foreach (Transform Child in Utilits.GetObjectsInHierarchy(Vehicle)) PartDestroy(Child);
    }

    public override void Destroy()
    {
        VehicleDestroy(transform);
    }
}
