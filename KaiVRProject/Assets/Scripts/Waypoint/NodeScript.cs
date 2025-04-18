using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeScript : MonoBehaviour
{
    public Color originalColor;
    public Color hoverColor;
    private Renderer rend;

    private GameObject currentTurret;
    private Vector3 towerDisplace = new Vector3 (0f, .3f, 0f);

    BuildManager buildManager;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (buildManager.GetTurretToBuild() == null)
            return;
        if (currentTurret != null)
            return;
        if (CoinManager.Instance.getCurrentTowerCost() > CoinManager.Instance.coins)
            return;
        


        Vector3 placePos = transform.position + towerDisplace;
        GameObject turretToBuild = buildManager.GetTurretToBuild();
        currentTurret = (GameObject)Instantiate(turretToBuild, placePos, transform.rotation);
        CoinManager.Instance.buyTower();
    }


    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (buildManager.GetTurretToBuild() == null)
            return;

        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        rend.material.color = originalColor;
    }
}
