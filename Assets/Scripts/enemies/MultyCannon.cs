using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class SingleCannon
{
    public bool IsReloaded;
    public Transform CannonTransform;
}

public class MultyCannon : Cannon
{
    [SerializeField] private List<SingleCannon> Cannons;
    public Queue<SingleCannon> ReloadingQueue = new Queue<SingleCannon>();

    private List<SingleCannon> GetReloaded() => Cannons.Where(Cannon => Cannon.IsReloaded).ToList();

    IEnumerator ReloadCannon(SingleCannon cannon)
    {
        cannon.IsReloaded = false;
        ReloadingQueue.Enqueue(cannon);
        yield return new WaitUntil(() => ReloadingQueue.Peek() == cannon);

        yield return new WaitForSeconds(1f);
        cannon.IsReloaded = true;
        ReloadingQueue.Dequeue();
    }



    private void ShootCannon(SingleCannon Cannon)
    {
        if (Cannon.IsReloaded)
        {
            CannonTransform = Cannon.CannonTransform;
            ShootShell(ShellPref);
            StartCoroutine( ReloadCannon(Cannon) );
        }
    }

    public void ShootOne()
    {
        ShootCannon(GetReloaded()[0]);
    }

    private new void Update()
    {
        base.Update();
        IsReloading = !(GetReloaded().Count >= Cannons.Count);
        //print($"R:{GetReloaded().Count} C:{Cannons.Count} {GetReloaded().Count == Cannons.Count}");
    }

    public override void Shoot()
    {
        foreach (SingleCannon cannon in GetReloaded()) ShootCannon(cannon); 
    }
}
