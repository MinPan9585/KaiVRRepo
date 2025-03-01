using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeScript : MonoBehaviour
{
    public Color originalColor;
    public Color hoverColor;
    private Renderer rend;

    private GameObject currentTurret;

    private void Start()
    {
        rend = GetComponent<Renderer>();
        originalColor = rend.material.color;
    }

    private void OnMouseDown()
    {
        if (currentTurret != null)
        {
            return;
        }

        GameObject turretToBuild = BuildManager.instance.GetTurretToBuild();
        currentTurret = (GameObject)Instantiate(turretToBuild, transform.position, transform.rotation);
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
