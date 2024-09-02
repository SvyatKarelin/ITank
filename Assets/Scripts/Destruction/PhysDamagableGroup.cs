using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PhysDamagableGroup : PhysDestruction
{
    [SerializeField] private List<PhysDamagableGroup> GroubMembers;
    public delegate void TakeDamageToGroup(int Damage);
    public TakeDamageToGroup TakeDamageCallbacks;

    public void TakeDamageCallback(int Damage)
    {
        //print(gameObject.name);
        HealthPoints -= Damage;
        if (HealthPoints <= 0)
        {
            if (transform.parent is not null) PhysDestroy(transform, transform.parent);
            else PhysDestroy(transform);

            TakeDamageCallbacks = null;
            foreach (var Member in GroubMembers) Member.TakeDamageCallbacks -= TakeDamageCallback;
        }
    }
    public override void TakeDamage(int Damage)
    {
        TakeDamageCallbacks?.Invoke(Damage);
    }
    public override void Start()
    {
        HealthPoints = StartHealth;
        foreach (var Member in GroubMembers) Member.TakeDamageCallbacks += TakeDamageCallback;
        if(!GroubMembers.Contains(this)) TakeDamageCallbacks += TakeDamageCallback;
    }
}
