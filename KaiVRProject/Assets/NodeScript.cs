using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NodeScript : XRBaseInteractable
{
    public Color originalColor;
    public Color hoverColor;
    private Renderer rend;

    private GameObject currentTurret;
    private Vector3 towerDisplace = new Vector3 (0f, .3f, 0f);

    protected override void Awake()
    {
        base.Awake();
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
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

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        HandleNodeClick();
    }

    private void HandleNodeClick()
    {
        if (currentTurret != null)
        {
            return;
        }
        Vector3 placePos = transform.position + towerDisplace;
        GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
        currentTurret = (GameObject)Instantiate(turretToBuild, placePos, transform.rotation);
    }

    // 保留鼠标交互用于测试
    private void OnMouseDown()
    {
        HandleNodeClick();
    }

    void OnMouseEnter()
    {
        rend.material.color = hoverColor;
    }

    private void OnMouseExit()
    {
        rend.material.color = originalColor;
    }
}
