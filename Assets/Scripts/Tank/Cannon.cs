using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
    [SerializeField] private float RotationSpeed = 45;
    [SerializeField] private float CannonStrength = 400;
    public bool IsReloading { get; private set; }
    public bool IsAimed { get; private set; }
    private ICTarget CannonTarget;
    public void Shoot()
    {
        if (!IsReloading)
        {
            GameObject Shell = Instantiate(ShellPref, CannonTransform.position, CannonTransform.rotation);
            Shell.GetComponent<Shell>().Ignore.Add(gameObject);
            Shell.GetComponent<Rigidbody>().AddForce(CannonTransform.forward * CannonStrength);
            StartCoroutine(Reload());
        }
    }

    Vector3 Aim(Transform ObjTransform, Vector3 Target)
    {
        Quaternion TargetRotation = Quaternion.LookRotation(Target - ObjTransform.position);
        Vector3 CurentRotation = Quaternion.RotateTowards(ObjTransform.rotation, TargetRotation, RotationSpeed * Time.deltaTime).eulerAngles;
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
            if (Utilits.CompareWithError(CannonTransform.eulerAngles, Quaternion.LookRotation(CannonTarget.GetPos() - CannonTransform.position).eulerAngles, 2f)) IsAimed = true; 
            else IsAimed = false;
        }
    }
}
