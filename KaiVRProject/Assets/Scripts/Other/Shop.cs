using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    BuildManager buildManager;
    public Button arrowTowerButton;
    public Button cannonTowerButton;
    public Button teslaCoilButton;
    private int coins = 0;

    private void Start()
    {

        buildManager = BuildManager.instance;
    }
    public void Update()
    {
        coins = CoinManager.Instance.getCoinBalance();
        
        if (coins < 5)
        {
            arrowTowerButton.interactable = false;
        } 
        else
        {
            arrowTowerButton.interactable = true;
        }
        
        if (coins < 10)
        {
            cannonTowerButton.interactable = false;
        }
        else
        {
            cannonTowerButton.interactable = true;
        }
        
        if (coins < 20)
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
        CoinManager.Instance.setCurrentTowerCost(5);
        
    }

    public void purchaseCannonTower()
    {
        
        Debug.Log("Cannon Selected");
        buildManager.SetTowerToBuild(buildManager.cannonTowerPrefab);
        CoinManager.Instance.setCurrentTowerCost(10);
        
    }

    public void purchaseTeslaCoil()
    {
        Debug.Log("Cannon Selected");
        buildManager.SetTowerToBuild(buildManager.teslaCoilPrefab);
        CoinManager.Instance.setCurrentTowerCost(20);
        
    }
}