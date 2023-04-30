using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnMiniBoss", 1.0f, 0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void SpawnMiniBoss()
    {
        var miniE = Instantiate(enemyPrefab, gameObject.transform.position + new Vector3(0f, 1.10f, 0f), enemyPrefab.transform.rotation);
        Destroy(miniE, 5);
    }
}
