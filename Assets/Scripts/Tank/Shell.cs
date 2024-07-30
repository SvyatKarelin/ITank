using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{
    [SerializeField] private GameObject EffectPrefab;
    [SerializeField] private int IDamageable = 50;
    public List<GameObject> Ignore;

    void Start()
    {
        Destroy(gameObject,10);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<IDamageable>() is IDamageable Damagable && !Ignore.Contains(collision.gameObject)) { Damagable.TakeDamage(IDamageable); }
        Instantiate(EffectPrefab, collision.contacts[0].point , Quaternion.identity);
        Destroy(gameObject);
    }
}