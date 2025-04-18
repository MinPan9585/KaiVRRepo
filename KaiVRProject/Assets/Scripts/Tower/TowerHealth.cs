using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealth : MonoBehaviour
{
    public Image healthBar;
    public float maxHealth = 3f;
    public float health;
    public GameObject destroyed;
    public GameObject[] normal;
    public bool resurrected = false;

    private void Awake()
    {
        health = maxHealth;
        destroyed = transform.GetChild(0).gameObject;
        destroyed.SetActive(false);
    }

    private void Update()
    {
        if (health <= 0)
        {
            setComponents(false);
            for (int i = 0; i < normal.Length; i++)
            {  
                normal[i].SetActive(false);
            }
            gameObject.tag = "deadTower";
        }

        if (resurrected == true)
        {
            setComponents(true);
            health = maxHealth;
            for (int i = 0; i < normal.Length; i++)
            {
                normal[i].SetActive(true);
            }
            gameObject.tag = "Tower";
            resurrected = false;
        }

        float fillAmount = health/maxHealth;
        healthBar.fillAmount = fillAmount;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy"))
        {
            health--;
        }
        
    }

    void setComponents(bool tf)
    {
        destroyed.SetActive(!tf);
        for (int i = 1; i < (transform.childCount - 1); i++)
        {
            transform.GetChild(i).gameObject.SetActive(tf);
        }
        if (GetComponent<ArrowTower>() != null)
        {
            GetComponent<ArrowTower>().enabled = tf;
            GetComponent<BoxCollider>().enabled = tf;
        }
        else if (GetComponent<CannonTower>() != null)
        {
            GetComponent<CannonTower>().enabled = tf;
            GetComponent<BoxCollider>().enabled = tf;
        }
        else if (GetComponent<TeslaCoilTower>() != null)
        {
            GetComponent<TeslaCoilTower>().enabled = tf;
            GetComponent<BoxCollider>().enabled = tf;
        }
    }
}