using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest : MonoBehaviour
{
    public InputActionAsset actionAsset;
    private InputAction triggerAction;
    
    // Start is called before the first frame update
    void Start()
    {
        // 获取右手交互动作组中的Activate Value动作
        triggerAction = actionAsset.FindActionMap("XRI RightHand Interaction").FindAction("Activate Value");
        // 启用动作
        triggerAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = triggerAction.ReadValue<float>();
        Debug.Log(triggerValue);
        Debug.Log("hahaha");

    }

    void OnDisable()
    {
        // 禁用动作
        if (triggerAction != null)
            triggerAction.Disable();
    }
}
