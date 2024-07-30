using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Stage
{
    public List<Transform> DestructOnStage;
}
public class PhysDestructionStages : PhysDestruction
{
    [SerializeField] private List<Stage> DestructionStages;
    public int HealthPerStage;

    public override void Start()
    {
        HealthPoints = StartHealth;
        HealthPerStage = (StartHealth / DestructionStages.Count);
    }
    public override void TakeDamage(int Damage)
    {
        if (GetStage(HealthPoints) < GetStage(HealthPoints -= Damage)) 
            foreach (Transform Tr in DestructionStages[GetStage(HealthPoints) - 1].DestructOnStage)
                PhysDestroy(Tr.gameObject);
        print(GetStage(HealthPoints));
    }

    int GetStage(int Health) => DestructionStages.Count - (int)Mathf.Ceil((float)Health / HealthPerStage);
}
    