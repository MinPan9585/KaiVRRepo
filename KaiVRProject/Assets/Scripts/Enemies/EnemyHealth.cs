using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth;
    private float currentHealth;

    private void Awake()
    {
        maxHealth = 90 * (Mathf.Pow(1.03f, WaveSpawner.waveIndex));
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.fillAmount = currentHealth / maxHealth;
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}