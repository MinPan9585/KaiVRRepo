using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    BuildManager buildManager;

    private void Start()
    {
        buildManager = BuildManager.instance;
    }
    public void purchaseArrowTower()
    {
        Debug.Log("Arrow Selected");
        buildManager.SetTowerToBuild(buildManager.arrowTowerPrefab);
    }

    public void purchaseCannonTower()
    {
        Debug.Log("Cannon Selected");
        buildManager.SetTowerToBuild(buildManager.cannonTowerPrefab);
    }
}
