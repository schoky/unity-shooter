using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tutorial
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] spawnPoints;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private float timeToSpawn;


        private void Start()
        {
            StartCoroutine(Spawner());
        }

        private void Spawn()
        {
            GameObject enemy = Instantiate(enemyPrefab);
            enemy.transform.position = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
        }

        IEnumerator Spawner()
        {
            while (true)
            {
                yield return new WaitForSeconds(timeToSpawn);
                Spawn();
            }
        }
    }
}