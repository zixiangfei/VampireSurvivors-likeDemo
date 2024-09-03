using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]private GameObject enemyToSpawn;

    [SerializeField]private float timeToSpawn;

    [SerializeField]private Transform minSpawn, maxSpawn;
    [SerializeField]private int checkPerFrame;
    [SerializeField]private List<WaveInfo> waveList;
    private int currentWave;
    private float waveCounter;
    
    private float spawnCounter;
    private Transform target;
    private float despawnDistance;
    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private int enemyToCheck = 0;

    // Start is called before the first frame update
    void Start()
    {
        // spawnCounter = timeToSpawn;

        target = PlayerHealthController.instance.transform;

        despawnDistance = Vector3.Distance(transform.position, maxSpawn.position) + 4f;

        currentWave = -1;
        GoToNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        /*spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = timeToSpawn;

            //Instantiate(enemyToSpawn, transform.position, transform.rotation);

            GameObject newEnemy = Instantiate(enemyToSpawn, SelectSpawnPoint(),transform.rotation);

            spawnedEnemies.Add(newEnemy);
        }*/

        if (PlayerHealthController.instance.gameObject.activeSelf)
        {
            if (currentWave < waveList.Count)
            {
                waveCounter -= Time.deltaTime;
                if (waveCounter <= 0)
                {
                    GoToNextWave();
                }

                spawnCounter -= Time.deltaTime;
                if (spawnCounter <= 0)
                {
                    spawnCounter = waveList[currentWave].timeBetweenSpawns;

                    GameObject newEnemy = Instantiate(waveList[currentWave].enemyToSpawn, SelectSpawnPoint(), Quaternion.identity);

                    spawnedEnemies.Add(newEnemy);
                }
            }
        }

        transform.position = target.position;

        int checkTarget = enemyToCheck + checkPerFrame;

        while(enemyToCheck < checkTarget)
        {
            if (enemyToCheck < spawnedEnemies.Count)
            {
                if (spawnedEnemies[enemyToCheck] != null)
                {
                    if (Vector3.Distance(transform.position, spawnedEnemies[enemyToCheck].transform.position) > despawnDistance)
                    {
                        Destroy(spawnedEnemies[enemyToCheck]);
                        spawnedEnemies.RemoveAt(enemyToCheck);
                        checkTarget--;
                    }
                    else
                    {
                        enemyToCheck++;
                    }
                }
                else
                {
                    spawnedEnemies.RemoveAt(enemyToCheck);
                    checkTarget--;
                }
            }
            else
            {
                enemyToCheck = 0;
                checkTarget = 0;
            }
            
        }
    }

    private Vector3 SelectSpawnPoint()
    {
        Vector3 spawnPoint = Vector3.zero;

        bool spawnVerticalEdge = Random.Range(0f, 1f) > 0.5f;

        if (spawnVerticalEdge)
        {
            spawnPoint.y = Random.Range(minSpawn.position.y, maxSpawn.position.y);

            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.x = maxSpawn.position.x;
            }
            else
            {
                spawnPoint.x = minSpawn.position.x;
            }
        }
        else
        {
            spawnPoint.x = Random.Range(minSpawn.position.x, maxSpawn.position.x);

            if (Random.Range(0f, 1f) > .5f)
            {
                spawnPoint.y = maxSpawn.position.y;
            }
            else
            {
                spawnPoint.y = minSpawn.position.y;
            }
        }
        return spawnPoint;
    }

    public void GoToNextWave()
    {
        currentWave++;

        if (currentWave >= waveList.Count)
        {
            currentWave = waveList.Count - 1;
        }

        waveCounter = waveList[currentWave].waveLength;
        spawnCounter = waveList[currentWave].timeBetweenSpawns;
    }
}

[System.Serializable]
public class WaveInfo
{
    [SerializeField]public GameObject enemyToSpawn;
    [SerializeField]public float waveLength = 10f;
    [SerializeField]public float timeBetweenSpawns = 1f;
}