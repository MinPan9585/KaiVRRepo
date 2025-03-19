using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SimpleDragMove : MonoBehaviour
{
    // private OVRInput.Controller lController = OVRInput.Controller.LTouch;
    // private OVRInput.Controller RController = OVRInput.Controller.RTouch;

    public InputActionAsset actionAsset;
    private InputAction rightTriggerAction;
    private InputAction leftTriggerAction;
    public Transform rigT; // tracking space
    public Transform headT; // head(center相机位置)
    private Vector3 rigRecordPos; //rigT记录位置(用于拖拽移动，按下时记录位置)
    public float moveSpeed = 1; //移动速度
    public Transform leftHand; //左手
    private Vector3 leftHandRecordPos; //左手记录位置
    public Transform rightHand; //右手
    private Vector3 rightHandRecordPos; //右手记录位置
    // Start is called before the first frame update
    void Start()
    {
        rightTriggerAction = actionAsset.FindActionMap("XRI RightHand Interaction").FindAction("Select Value");
        leftTriggerAction = actionAsset.FindActionMap("XRI LeftHand Interaction").FindAction("Select Value");
        rightTriggerAction.Enable();
        leftTriggerAction.Enable();
    }
    // Update is called once per frame
    void Update()
    {
        // StickRotate();
        //拖拽移动
        DragMove();
        //左摇杆移动
        // StrickMove();
        // //InputTest();
    }
    // private void StickRotate()
    // {
    //     // //右手摇杆进行转向(以头部（headT）为旋转中心)
    //     // rigT.RotateAround(
    //     //     headT.position,
    //     //     Vector3.up,
    //     //     OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, RController).x * 180 * Time.deltaTime
    //     // );
    //     //右手摇杆进行转向，每次旋转45度(以头部（headT）为旋转中心)
    //     if (rightTriggerAction.WasPressedThisFrame())
    //     {
    //         rigT.RotateAround(headT.position, Vector3.up, 45);
    //     }
    //     if (leftTriggerAction.WasPressedThisFrame())
    //     {
    //         rigT.RotateAround(headT.position, Vector3.up, -45);
    //     }
    // }
    // private void StrickMove()
    // {
    //     //左摇杆移动
    //     Vector2 move = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick, lController);
    //     // 创建一个只有Y轴旋转的四元数
    //     Quaternion rotationY = Quaternion.Euler(0, leftHand.rotation.eulerAngles.y, 0);
    //     // 使用新的四元数旋转移动向量
    //     Vector3 dir = rotationY * new Vector3(move.x, 0, move.y);
    //     dir = new Vector3(dir.x, 0, dir.z);
    //     rigT.position += dir * moveSpeed * Time.deltaTime;
    // }
    private void DragMove()
    {
        //rightHand move record position
        if (rightTriggerAction.WasPressedThisFrame())
        {
            rightHandRecordPos = rightHand.localPosition;
            rigRecordPos = rigT.position;
        }
        
        //rightHand move moving
        if (rightTriggerAction.IsPressed())
        {
            Vector3 offset = rightHand.localPosition - rightHandRecordPos;
            offset = Quaternion.Euler(0, rigT.eulerAngles.y, 0) * offset;
            rigT.position = rigRecordPos - offset * moveSpeed;
        }

        //leftHand move record position
        if (leftTriggerAction.WasPressedThisFrame())
        {
            leftHandRecordPos = leftHand.localPosition;
            rigRecordPos = rigT.position;
        }
        
        //leftHand move moving
        if (leftTriggerAction.IsPressed())
        {
            Vector3 offset = leftHand.localPosition - leftHandRecordPos;
            offset = Quaternion.Euler(0, rigT.eulerAngles.y, 0) * offset;
            rigT.position = rigRecordPos - offset * moveSpeed;
        }
    }

}
