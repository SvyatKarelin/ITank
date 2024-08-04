using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : Cannon
{
    private bool IsShooting;
    IEnumerator Fire()
    {
        IsShooting = true;
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.1f);
            ShootShell(ShellPref);
        }
        IsShooting = false;
        StartCoroutine(Reload(2f));
    }

    public override void Shoot()
    {
        if (!IsReloading && !IsShooting)
        {
            StartCoroutine(Fire());
        }
    }
}
