using UnityEngine;
//using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    public Toggle arrowTowerButton;
    public Toggle cannonTowerButton;
    public Toggle teslaCoilButton;

    [Header("Tower Costs")]
    public int arrowTowerCost;
    public int cannonTowerCost;
    public int teslaCoilCost;
    private int coins = 0;

    private void Start()
    {

        buildManager = BuildManager.instance;
    }
    public void Update()
    {
        coins = CoinManager.Instance.getCoinBalance();
        
        if (coins < arrowTowerCost)
        {
            arrowTowerButton.interactable = false;
        } 
        else
        {
            arrowTowerButton.interactable = true;
        }
        
        if (coins < cannonTowerCost)
        {
            cannonTowerButton.interactable = false;
        }
        else
        {
            cannonTowerButton.interactable = true;
        }
        
        if (coins < teslaCoilCost)
        {
            teslaCoilButton.interactable = false;
        } 
        else
        {
            teslaCoilButton.interactable = true;
        }
    }

    public void purchaseArrowTower()
    {
        
        Debug.Log("Arrow Selected");
        buildManager.SetTowerToBuild(buildManager.arrowTowerPrefab);
        CoinManager.Instance.setCurrentTowerCost(arrowTowerCost);
        
    }

    public void purchaseCannonTower()
    {
        
        Debug.Log("Cannon Selected");
        buildManager.SetTowerToBuild(buildManager.cannonTowerPrefab);
        CoinManager.Instance.setCurrentTowerCost(cannonTowerCost);
        
    }

    public void purchaseTeslaCoil()
    {
        Debug.Log("Cannon Selected");
        buildManager.SetTowerToBuild(buildManager.teslaCoilPrefab);
        CoinManager.Instance.setCurrentTowerCost(teslaCoilCost);
        
    }
}