using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }
    public int coins = 10;
    public static int currentTowerCost = 0;
    public TextMeshProUGUI coinText;

    [Header("Tower Costs")]
    public int arrowTowerCost;
    public int cannonTowerCost;
    public int teslaCoilCost;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void addCoins(int amount)
    {
        coins += amount;
    }

    

    public int getCoinBalance()
    {
        return coins;
    }

    public void setCurrentTowerCost(int amount)
    {
        currentTowerCost = amount;
    }

    public int getCurrentTowerCost()
    {
        return currentTowerCost;
    }

    public void buyTower()
    {
        coins -= currentTowerCost;
    }

    public void Update()
    {
        coinText.text = ("$ " + coins.ToString());
    }
}