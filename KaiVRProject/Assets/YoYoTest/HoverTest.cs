using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HoverTest : MonoBehaviour
{
    public string hoverName = "Hover";
    public string selectName = "Select";
    public XRRayInteractor xrRayInteractor;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 获取第一个hover的对象
        IXRHoverInteractable hoverObject = xrRayInteractor.interactablesHovered.Count > 0 ? 
            xrRayInteractor.interactablesHovered[0] : null;

        if (hoverObject != null)
        {
            hoverName = hoverObject.transform.name;
        }

        // 获取所有hover的对象
        List<IXRHoverInteractable> hoveredObjects = xrRayInteractor.interactablesHovered;

        // 获取选中的对象
        IXRSelectInteractable selectedObject = xrRayInteractor.interactablesSelected.Count > 0 ? 
            xrRayInteractor.interactablesSelected[0] : null;

        if (selectedObject != null)
        {
            selectName = selectedObject.transform.name;
        }
    }
}
