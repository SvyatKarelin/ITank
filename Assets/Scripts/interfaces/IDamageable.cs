using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public int HealthPoints { get; }
    public void TakeDamage(int Damage);
    public void Destroy();
}
