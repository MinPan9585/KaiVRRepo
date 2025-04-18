using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputTest2 : MonoBehaviour
{
    public InputActionAsset actionAsset;
    private InputAction rightTriggerAction;
    
    // Start is called before the first frame update
    void Start()
    {
        // 获取右手交互动作组中的Activate Value动作
        rightTriggerAction = actionAsset.FindActionMap("XRI RightHand Interaction").FindAction("Activate Value");
        // 启用动作（其实默认是自动启用的，不写这个也可以）
        rightTriggerAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // 检测按钮是否刚刚被按下
        if (rightTriggerAction.WasPressedThisFrame())
        {
            Debug.LogError("按钮被按下！");
        }

        if (rightTriggerAction.IsPressed())
        {
            Debug.LogError("按钮被按住！");
        }

        if (rightTriggerAction.WasReleasedThisFrame())
        {
            Debug.LogError("按钮被释放！");
        }

        // 获取按钮的值
        float triggerValue = rightTriggerAction.ReadValue<float>();
        Debug.Log(triggerValue);

    }

    void OnDisable()
    {
        // 禁用动作
        if (rightTriggerAction != null)
            rightTriggerAction.Disable();
    }
}
