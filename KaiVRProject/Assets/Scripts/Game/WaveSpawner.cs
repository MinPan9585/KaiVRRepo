using UnityEngine;
using TMPro;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    public static float challengeLevel = 1.05f;
    public static int enemiesAlive = 0;

    public Transform enemyPrefab;
    public Transform spawn1;
    public Transform spawn2;
    public Transform spawn3;
    public Transform spawn4;
    public Transform spawn5;

    PeripheralEnemySpawner pes;

    public float timeBetweenWaves = 2f;
    private float countdown = 2f;
    private bool waveDone = true;

    public TextMeshProUGUI waveCountdownText;

    public static int waveIndex = 0;

    private void Start()
    {
        pes = GameObject.FindGameObjectWithTag("PES").GetComponent<PeripheralEnemySpawner>();
        enemiesAlive = 0;
    }

    void Update()
    {
        if (waveDone && enemiesAlive == 0)
        {
            if (countdown <= 0f)
            {
                waveDone = false;
                StartCoroutine(SpawnWave());
                countdown = timeBetweenWaves + 0.5f * waveIndex;
            }
            countdown -= Time.deltaTime;
            waveCountdownText.text = Mathf.Floor(Mathf.Max(countdown, 0)).ToString();
        }
        else
        {
            waveCountdownText.text = "Wave " + waveIndex;
        }
        print(enemiesAlive);
    }

    IEnumerator SpawnWave()
    {
        int enemyCount = (int)(3 + Mathf.Pow(waveIndex, 1.3f));
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.1f);
        }
        
        waveDone = true;
        waveIndex++;
    }

    void SpawnEnemy()
    {
        enemiesAlive++;

        int whichEnemy = Random.Range(8, 10);
        if (whichEnemy < 6)
        {
            int spawn = Random.Range(0, 5);
            if (spawn == 0)
            {
                Instantiate(enemyPrefab, spawn1.position, spawn1.rotation);
            }
            else if (spawn == 1)
            {
                Instantiate(enemyPrefab, spawn2.position, spawn2.rotation);
            }
            else if (spawn == 2)
            {
                Instantiate(enemyPrefab, spawn3.position, spawn3.rotation);
            }
            else if (spawn == 3)
            {
                Instantiate(enemyPrefab, spawn4.position, spawn4.rotation);
            }
            else if (spawn == 4)
            {
                Instantiate(enemyPrefab, spawn5.position, spawn5.rotation);
            }
        }
        else if (whichEnemy > 8)
        {
            pes.spawnPlayerEnemy();
        }
        else
        {
            pes.spawnTowerEnemy();
        }
    }
}