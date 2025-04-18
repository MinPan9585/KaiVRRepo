using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverallGameFlow : MonoBehaviour
{
    private int enemiesKilled = 0;
    private int wavesSurvived = 0;
    private void Update()
    {
        if (CastleHealth.castleHealth <= 0 || playerScript.playerHealth <= 0)
        {
            Debug.Log("GameEnd");
            
        }
    }
}
