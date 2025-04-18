using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public Image healthBar;
    public float health = 100f;
    public int coinReward = 2;
    bool alive = true;
    bool killedByPlayer = false;

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / 100f;
        if (health <= 0)
        {
            alive = false;
            killedByPlayer = true;
        }
    }

    private void Update()
    {
        if (alive == false)
        {
            
            WaveSpawner.enemiesAlive--;
            if (killedByPlayer)
            {
                CoinManager.Instance.addCoins(3);
            }
            Destroy(gameObject);
            alive = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            alive = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Castle") || other.gameObject.CompareTag("Tower"))
        {
            alive = false;
        }
    }


}