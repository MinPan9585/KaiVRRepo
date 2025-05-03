using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shoot : MonoBehaviour
{
    public InputActionAsset actionAsset;
    private InputAction leftGripAction;
    private InputAction rightGripAction;
    public GameObject bulletPrefab; // 预制体

    private GameObject currentBullet; // 当前生成的子弹

    // Start is called before the第一帧 update
    void Start()
    {
        leftGripAction = actionAsset.FindActionMap("XRI LeftHand Interaction").FindAction("Select");
        rightGripAction = actionAsset.FindActionMap("XRI RightHand Interaction").FindAction("Select");
    }

    // Update is called once per帧
    void Update()
    {
        // 当左右手同时按下时
        if (leftGripAction.IsPressed() && rightGripAction.IsPressed())
        {
            if (currentBullet == null) // 确保只生成一个当前子弹
            {
                currentBullet = Instantiate(bulletPrefab, transform); // 生成子弹
                currentBullet.GetComponent<Bullet>().waitForShooting = true; // 打开子弹的 bool
            }
        }

        // 当任意一只手松开时
        if (leftGripAction.WasReleasedThisFrame() || rightGripAction.WasReleasedThisFrame())
        {
            if (currentBullet != null) // 确保当前子弹存在
            {
                currentBullet.GetComponent<Bullet>().waitForShooting = false; // 关闭当前子弹的 bool
                currentBullet = null; // 重置当前子弹引用，准备生成下一颗子弹
            }
        }
    }
}
