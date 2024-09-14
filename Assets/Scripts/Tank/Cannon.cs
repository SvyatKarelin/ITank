using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public interface ICTarget {
    public Vector3 GetPos();
    public Transform GetTargetTransform();
}
public class CTarget<T> : ICTarget
{
    public T target;

    public CTarget(T target)
    {
        this.target = target;
    }

    public Transform GetTargetTransform() => target switch
    {
        Transform Transform => Transform,
        _ => null
    };


public Vector3 GetPos() => target switch
    {
        Vector3 Pos => Pos,
        Transform Transform => Transform.position,
        _ => Vector3.zero
    };
}

public class Cannon : MonoBehaviour
{
    [SerializeField] private AudioClip ShootSound;
    [SerializeField] protected GameObject ShellPref;
    [SerializeField] protected Transform CannonTransform;
    [SerializeField] protected float ReloadingTime = 2;
    [SerializeField] protected float RotationSpeed = 45;
    [SerializeField] protected float CannonStrength = 400;
    public bool IsReloading { get; protected set; }
    public bool IsAimed { get; protected set; }
    protected ICTarget CannonTarget;
    protected AudioSource CannonAudioSource;

    protected virtual void ShootShell(GameObject Shell)
    {
        CannonAudioSource.Play();
        Shell = Instantiate(ShellPref, CannonTransform.position, CannonTransform.rotation);
        Shell.GetComponent<Shell>().Ignore.Add(gameObject);
        Shell.GetComponent<Rigidbody>().AddForce(CannonTransform.forward * CannonStrength);
    }

    public virtual void Shoot()
    {
        if (!IsReloading)
        {
            ShootShell(ShellPref);
            StartCoroutine(Reload(ReloadingTime));
        }
    }

    Vector3 Aim(Transform ObjTransform, Vector3 Target)
    {
        Quaternion TargetRotation = Quaternion.LookRotation(Target - ObjTransform.position);
        Vector3 CurentRotation = Quaternion.RotateTowards(ObjTransform.rotation, TargetRotation, RotationSpeed * Time.deltaTime).eulerAngles;
        return CurentRotation;
    }

    protected IEnumerator Reload(float ReloadTime)
    {
        IsReloading = true;
        yield return new WaitForSeconds(ReloadTime);
        IsReloading = false;
    }

    public void LookAt(Vector3 TrgtPos) => CannonTarget = new CTarget<Vector3>(TrgtPos);
    public void LookAt(Transform TrgtTransform) => CannonTarget = new CTarget<Transform>(TrgtTransform);

    protected void Start()
    {
        CannonAudioSource = Utilits.CheckComponent<AudioSource>(transform);
        CannonAudioSource.clip = ShootSound;
    }

    // feat: Cannon class intemediate progress posted
    protected void Update()
    {
        if (CannonTarget is not null) {
            Vector3 CannonAim = Aim(CannonTransform, CannonTarget.GetPos()), TurretAim = Aim(transform, CannonTarget.GetPos());
            print(transform.rotation.eulerAngles.y);
            transform.localRotation = Quaternion.Euler( 0f, CannonAim.y - transform.parent.eulerAngles.y, 0f);
            CannonTransform.eulerAngles = new Vector3(CannonAim.x, CannonAim.y, 0f);

            if (Utilits.CompareWithError(CannonTransform.eulerAngles, Quaternion.LookRotation(CannonTarget.GetPos() - CannonTransform.position).eulerAngles, 2f)) IsAimed = true; 
            else IsAimed = false;
        }
    }
}
