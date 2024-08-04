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
    [SerializeField] private List<GameObject> Enemies;
    private float elapsedTime = 0.0f;

    Vector3 GetGround(Vector3 Pos) {
        if (Physics.Raycast(Pos, Vector3.down, out RaycastHit hit)) return hit.point;
        else return Pos;
    }

    void SpawnEnemy()
    {
        float RndAng = Random.Range(0, 2 * Mathf.PI);
        Vector3 EmnemyLocalPos = new Vector3(Mathf.Cos(RndAng), 0f, Mathf.Sin(RndAng)) * Random.Range(NoSpawnRadius, SpawnRadius);
        Vector3 EnemyPos = GameObject.FindGameObjectWithTag("Player").transform.position + EmnemyLocalPos;
        EnemyPos = GetGround(EnemyPos + new Vector3(0, GameObject.FindGameObjectWithTag("Player").transform.position.y + 100, 0));

        Instantiate(Enemies[Random.Range(0, Enemies.Count - 1)], EnemyPos, Quaternion.identity);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > EnemySpawnCountown && FindObjectsOfType<Enemy>().Where(Enemy => Enemy.enabled).ToArray().Length < EnemyMaxCount)
        {
            SpawnEnemy();
            elapsedTime = 0;
        }
    }
}
