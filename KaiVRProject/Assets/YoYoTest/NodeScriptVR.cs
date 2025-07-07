using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;


public class NodeScriptVR : XRBaseInteractable
{
    public Color originalColor;
    public Color hoverColor;
    private Renderer rend;

    private GameObject currentTurret;
    private Vector3 towerDisplace = new Vector3(0f, .3f, 0f);

    BuildManager buildManager;

    // protected override void Awake()
    // {
    //     base.Awake();
    //     rend = GetComponent<Renderer>();
    //     originalColor = rend.material.color;
    // }

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
        buildManager = BuildManager.instance;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        rend.material.color = hoverColor;
    }

    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        rend.material.color = originalColor;
    }

    // protected override void OnSelectEntered(SelectEnterEventArgs args)
    // {
    //     base.OnSelectEntered(args);
    //     HandleNodeClick();
    // }

    protected override void OnActivated(ActivateEventArgs args)
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        if (buildManager.GetTurretToBuild() == null)
            return;
        if (currentTurret != null && currentTurret.GetComponent<TowerHealth>().health > 0f)
        {
            return;
        }
        else if (currentTurret != null && currentTurret.GetComponent<TowerHealth>().health <= 0f && CoinManager.Instance.getCurrentTowerCost() <= CoinManager.Instance.coins)
        {
            Destroy(currentTurret);
        }
        if (CoinManager.Instance.getCurrentTowerCost() > CoinManager.Instance.coins)
            return;



        Vector3 placePos = transform.position + towerDisplace;
        GameObject turretToBuild = buildManager.GetTurretToBuild();
        currentTurret = (GameObject)Instantiate(turretToBuild, placePos, transform.rotation);
        CoinManager.Instance.buyTower();
    }
}
