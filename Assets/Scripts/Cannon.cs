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
    [SerializeField] private Transform CannonTransform;
    private ICTarget CannonTarget;
    public void Shoot()
    {

    }

    public void LookAt(Vector3 TrgtPos) => CannonTarget = new CTarget<Vector3>(TrgtPos);
    public void LookAt(Transform TrgtTransform) => CannonTarget = new CTarget<Transform>(TrgtTransform);
    // feat: Cannon class intemediate progress posted
    private void Update()
    {
        if (CannonTarget is not null) {
            Quaternion TargetRotation = Quaternion.LookRotation(CannonTarget.GetPos() - transform.position);
            Vector3 CurrentRotation = Quaternion.RotateTowards(transform.rotation, TargetRotation, 45 * Time.deltaTime).eulerAngles;
            CannonTransform.eulerAngles = new Vector3(0f, CurrentRotation.y , 0f);
        }
    }
}
