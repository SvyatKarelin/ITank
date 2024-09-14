using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField, Min(0)] private int EnemyMaxCount = 10;
    [SerializeField, Min(0)] private float NoSpawnRadius = 100;
    [SerializeField, Min(0)] private float SpawnRadius = 300;
    [SerializeField, Min(0)] private float EnemySpawnCountown = 300;
    [SerializeField] private bool RespawnEnemies = true;
    [SerializeField] private List<GameObject> Enemies;
    private float elapsedTime = 0.0f;

    void SpawnEnemy()
    {
        Vector2 RndCir = Utilits.GetRandomOnCircle();
        Vector3 EmnemyLocalPos = new Vector3(RndCir.x, 0f, RndCir.y) * Random.Range(NoSpawnRadius, SpawnRadius);
        Vector3 EnemyPos = Utilits.GetGround(GameObject.FindGameObjectWithTag("Player").transform.position + EmnemyLocalPos);
        //EnemyPos = GetGround(EnemyPos + new Vector3(0, GameObject.FindGameObjectWithTag("Player").transform.position.y + 100, 0));

        Instantiate(Enemies[Random.Range(0, Enemies.Count)], EnemyPos, Quaternion.identity);
    }

    private void Start()
    {
        for(int i = 0; i < EnemyMaxCount; i++) SpawnEnemy();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (RespawnEnemies && elapsedTime > EnemySpawnCountown && FindObjectsOfType<Enemy>().Where(Enemy => Enemy.enabled).ToArray().Length < EnemyMaxCount)
        {
            SpawnEnemy();
            elapsedTime = 0;
        }
    }
}
