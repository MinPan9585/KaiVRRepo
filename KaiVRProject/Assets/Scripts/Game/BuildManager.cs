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
            return;
        }
        instance = this;
        
    }
    private void Update()
    {
        
    }

    public GameObject arrowTowerPrefab;
    public GameObject cannonTowerPrefab;
    public GameObject teslaCoilPrefab;

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
