using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    private int enemiesKilled = EnemyHealth.enemiesKilled;
    private int wavesSurvived = WaveSpawner.waveIndex;
    // Start is called before the first frame update
    public Text enemiesKilledTxt;
    public Text wavesSurvivedTxt;

    private void Awake()
    {
        enemiesKilledTxt.text = "Enemies Killed: " + enemiesKilled;
        wavesSurvivedTxt.text = "Waves Survived: " + wavesSurvived;
    }

    public void menu()
    {
        SceneManager.LoadScene(0);
    }

    public void quit()
    {
        Application.Quit();
    }

}
