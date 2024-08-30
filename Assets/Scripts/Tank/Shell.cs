using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static UnityEngine.GraphicsBuffer;

public class Shell : MonoBehaviour
{
    [SerializeField] private GameObject EffectPrefab;
    [SerializeField] private int Damage = 50;
    [SerializeField] private int SplashRadius = 1;
    [SerializeField] private int SplashDamage = 10;
    public List<GameObject> Ignore;
    private Rigidbody _rigitBody;

    void Start()
    {
        Destroy(gameObject,20);
        _rigitBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(_rigitBody.velocity);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.GetComponent<Shell>()) return;
        if (collision.transform.GetComponent<IDamageable>() is IDamageable Damagable && !Ignore.Contains(collision.gameObject)) { Damagable.TakeDamage(Damage); }
        Instantiate(EffectPrefab, collision.contacts[0].point , Quaternion.identity);
        //Splash
        //foreach(Collider Damageable in Physics.OverlapSphere(transform.position, SplashRadius).Where(Obj => Obj is IDamageable).ToArray() ) (Damageable as IDamageable).TakeDamage(SplashDamage) ;
        Destroy(gameObject);
    }
}