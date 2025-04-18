using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    public float castleMaxHealth = 30f;
    public static float castleHealth;
    public Image healthBarImage;

    private HashSet<GameObject> damagedEnemies = new HashSet<GameObject>();

    private void Awake()
    {
        castleHealth = castleMaxHealth;
    }
    public void Update()
    {
        float fillamount = castleHealth / castleMaxHealth;
        healthBarImage.fillAmount = fillamount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("enemy") && !damagedEnemies.Contains(other.gameObject))
        {
            castleHealth--;
            damagedEnemies.Add(other.gameObject);
        }
    }
}
