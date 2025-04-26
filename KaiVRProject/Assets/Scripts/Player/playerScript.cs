using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerScript : MonoBehaviour
{
    public float playerMaxHealth = 100f;
    public static float playerHealth;
    public Image healthBarImage;

    private void Awake()
    {
        playerHealth = playerMaxHealth;
    }

    private void Update()
    {
        float fillAmount = playerHealth/playerMaxHealth;
        healthBarImage.fillAmount = fillAmount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            playerHealth -= 5;
        }
    }
}
