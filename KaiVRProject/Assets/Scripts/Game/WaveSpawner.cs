using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    
    public Transform enemyPrefab;

    public Transform spawnPoint;

    private int[] waveSpawns = {3, 5, 8, 12, 17, 23, 30};

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    public TextMeshProUGUI waveCountdownText;

    private int waveIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;

        waveCountdownText.text = Mathf.Floor(countdown).ToString();
    }

    IEnumerator SpawnWave()
    {
        Debug.Log("Wave Coming" + waveIndex);

        for (int i = 0; i < waveSpawns[waveIndex]; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.1f);
        }

        waveIndex++;
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}