using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

interface ICTarget {
    public Vector3 GetPos();
}
class CTarget<T> : ICTarget
{
    public T target;
    public CTarget(T target)
    {
        this.target = target;
    }
    public Vector3 GetPos() => target switch
    {
        Vector3 Pos => Pos,
        Transform Transform => Transform.position,
        _ => Vector3.zero
    };
}

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject ShellPref;
    [SerializeField] private Transform CannonTransform;
    private ICTarget CannonTarget;
    private bool IsReloading;
    public void Shoot()
    {
        if (!IsReloading)
        {
            GameObject Shell = Instantiate(ShellPref, CannonTransform.position, CannonTransform.rotation);
            Shell.GetComponent<Shell>().Ignore.Add(gameObject);
            Shell.GetComponent<Rigidbody>().AddForce(CannonTransform.forward * 400);
            StartCoroutine(Reload());
        }
    }

    Vector3 Aim(Transform ObjTransform, Vector3 Target)
    {
        Quaternion TargetRotation = Quaternion.LookRotation(Target - ObjTransform.position);
        Vector3 CurentRotation = Quaternion.RotateTowards(ObjTransform.rotation, TargetRotation, 45 * Time.deltaTime).eulerAngles;
        return CurentRotation;
    }

    IEnumerator Reload()
    {
        IsReloading = true;
        yield return new WaitForSeconds(2f);
        IsReloading = false;
    }

    public void LookAt(Vector3 TrgtPos) => CannonTarget = new CTarget<Vector3>(TrgtPos);
    public void LookAt(Transform TrgtTransform) => CannonTarget = new CTarget<Transform>(TrgtTransform);
    // feat: Cannon class intemediate progress posted
    private void Update()
    {
        if (CannonTarget is not null) {
            Vector3 CannonAim = Aim(CannonTransform, CannonTarget.GetPos()), TurretAim = Aim(transform, CannonTarget.GetPos());
            transform.eulerAngles = new Vector3(0f, TurretAim.y , 0f);
            CannonTransform.eulerAngles = new Vector3(CannonAim.x, CannonAim.y, 0f);
        }
    }
}
