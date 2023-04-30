using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject S_enemyPrefab;
    private float spawnRang = 9;
    public int enemyCount;
    public int waveNumber=1;
    public GameObject powerupPrefab;

    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemyWave(waveNumber);
        Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);

    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            Instantiate(powerupPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        }
        
    }
    private Vector3 GenerateSpawnPosition()
    {
        float RandomPosX = Random.Range(-spawnRang, spawnRang);
        float RandomPosZ = Random.Range(-spawnRang, spawnRang);
        Vector3 randomPos = new Vector3(RandomPosX, 0, RandomPosZ);

        return randomPos;
    }
    private void SpawnEnemyWave(int enemyToSpawn)
    {
        Instantiate(S_enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        for (int i = 0; i < enemyToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }
}
