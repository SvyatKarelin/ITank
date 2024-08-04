using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DestructableVehicle : PhysDestruction
{
    void PartDestroy(Transform VehiclePart)
    {
        if (VehiclePart.GetComponent<Renderer>()) VehiclePart.GetComponent<Renderer>().material.color = new Color(0.215f, 0.215f, 0.215f, 1f);
        foreach (Behaviour C in VehiclePart.GetComponents<Behaviour>()) C.enabled = false;
        PhysDestroy(VehiclePart);
        Destroy(VehiclePart.gameObject, 20);
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
