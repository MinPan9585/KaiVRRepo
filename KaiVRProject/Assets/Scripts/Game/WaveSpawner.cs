using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using System;

public class WaveSpawner : MonoBehaviour
{
    public float challengeLevel = 1.1f;
    public static int enemiesAlive = 0;
    
    public Transform enemyPrefab;

    public Transform spawn1;
    public Transform spawn2;

    PeripheralEnemySpawner pes;

    //private int[] waveSpawns = {3, 5, 8, 12, 17, 23, 30, 60};

    public float timeBetweenWaves = 2f;
    private float countdown = 2f;
    private bool waveDone = true;

    public TextMeshProUGUI waveCountdownText;

    public static int waveIndex = 0;

    // Update is called once per frame

    private void Start()
    {
        pes=GameObject.FindGameObjectWithTag("PES").GetComponent<PeripheralEnemySpawner>();
    }
    void Update()
    {
        if (countdown <= 0f)
        {
            waveDone = false;
            StartCoroutine(SpawnWave());
            countdown = 2 + 0.5f * waveIndex;
        }
        waveCountdownText.text = (Mathf.Floor(countdown)).ToString();
        if (waveDone == true)
            countdown -= Time.deltaTime;
    }

    IEnumerator SpawnWave()
    {
        Debug.Log("Wave Coming" + waveIndex);

        for (int i = 0; i < (Mathf.Pow(waveIndex, 1.1f)); i++)
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

        int whichEnemy = UnityEngine.Random.Range(0, 10);
        if (whichEnemy < 6)
        {
            int spawn = UnityEngine.Random.Range(0, 2);
            if (spawn == 0)
            {
                Instantiate(enemyPrefab, spawn1.position, spawn1.rotation);
            }
            else if (spawn == 1)
            {
                Instantiate(enemyPrefab, spawn2.position, spawn2.rotation);
            }
        }
        else if (whichEnemy > 8)
        {
            //spawn PLAYERENEMY
        }
        else
        {
            pes.spawnTowerEnemy();
        }
    }
}