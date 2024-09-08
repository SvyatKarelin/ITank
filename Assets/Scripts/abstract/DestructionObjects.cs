using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DestructionObjects : MonoBehaviour, IDamageable
{
    [SerializeField] public int StartHealth;
    protected int _HealthPoints;
    public int HealthPoints { get { return _HealthPoints; } protected set { _HealthPoints = Mathf.Clamp( value , 0 , int.MaxValue ); } }
    public virtual void Start()
    {
        HealthPoints = StartHealth;
    }
    public virtual void TakeDamage(int Damage)
    {
        HealthPoints -= Damage;
        if (HealthPoints <= 0) Destroy();
    }

    public virtual void Destroy()
    {
        Destroy(gameObject);
    }
}
