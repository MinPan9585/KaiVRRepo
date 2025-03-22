using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("more than one buildmanager");
            return;
        }
        instance = this;
        
    }

    public GameObject arrowTowerPrefab;
    public GameObject cannonTowerPrefab;

    private GameObject turretToBuild;

    public GameObject GetTurretToBuild ()
    {
        return turretToBuild;
    }

    public void SetTowerToBuild(GameObject tower)
    {
        turretToBuild = tower;
    }
}
