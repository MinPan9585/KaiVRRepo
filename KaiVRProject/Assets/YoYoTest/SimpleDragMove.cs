using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
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




    public Vector3 centerPosition;//用这个作为中心点
    public Transform TrackingSpace; // 摄像机的父物体
    public float recordScale; // 记录的初始比例
    public float recordDistance; // 记录的初始距离


    public bool useDragScale = false;
    public bool useDragMove = false;

    // public XRInteractorLineVisual leftHandLineVisual;
    public XRInteractorLineVisual rightHandLineVisual;
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
        if (useDragMove)
        {
            DragMove();
        }
        if (useDragScale)
        {
            DragScale();
        }
        //左摇杆移动
        StrickMove();
        // //InputTest();

        rightHandLineVisual.lineLength = 0.5f*TrackingSpace.lossyScale.x;
        rightHandLineVisual.lineWidth = 0.005f*TrackingSpace.lossyScale.x;
        
    }

    private void DragScale()
    {
        centerPosition = (leftHand.position + rightHand.position) / 2;
        //如果按下了手柄的A键并且按下了手柄的X键 或者按下了手柄的X键并且按下了手柄的A键
        if (rightTriggerAction.IsPressed() && leftTriggerAction.WasPressedThisFrame() || rightTriggerAction.WasPressedThisFrame() && leftTriggerAction.IsPressed())
        {
            //记录当前的相机的缩放倍数
            recordScale = TrackingSpace.lossyScale.x;
            //记录当前两个手柄的距离
            recordDistance = Vector3.Distance(leftHand.localPosition, rightHand.localPosition);
        }
        if (rightTriggerAction.IsPressed() && leftTriggerAction.IsPressed())
        {
            // 获取现在两个手的距离
            var nowDistance = Vector3.Distance(leftHand.localPosition, rightHand.localPosition);
            // 计算现在的缩放比例
            var targetScale = recordDistance / nowDistance * recordScale;
            // 计算缩放后的大小
            var newSize = targetScale;

            // 计算要缩放的距离
            Vector3 zoomVector = (centerPosition - TrackingSpace.position) * (newSize / TrackingSpace.localScale.x - 1.0f);
            // 缩放物体并移动位置，以使中心点保持不变
            TrackingSpace.localScale = new Vector3(newSize, newSize, newSize);
            TrackingSpace.position -= zoomVector;
        }
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
    private void StrickMove()
    {
        //左摇杆移动
        Vector2 move = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Move").ReadValue<Vector2>();
        // 创建一个只有Y轴旋转的四元数
        Quaternion rotationY = Quaternion.Euler(0, leftHand.rotation.eulerAngles.y, 0);
        // 使用新的四元数旋转移动向量
        Vector3 dir = rotationY * new Vector3(move.x, 0, move.y);
        dir = new Vector3(dir.x, 0, dir.z);
        rigT.position += dir * moveSpeed * Time.deltaTime * TrackingSpace.lossyScale.x;
    }


    
    private void DragMove()
    {
        // 如果两个手柄同时按下，直接返回不执行拖拽移动
        if (rightTriggerAction.IsPressed() && leftTriggerAction.IsPressed())
        {
            return;
        }

        // 检测是否从双手模式切换到右手模式
        if (rightTriggerAction.IsPressed() && leftTriggerAction.WasReleasedThisFrame())
        {
            // 重新记录右手位置和基准位置
            rightHandRecordPos = rightHand.localPosition;
            rigRecordPos = rigT.position;
        }
        // 检测是否从双手模式切换到左手模式
        else if (leftTriggerAction.IsPressed() && rightTriggerAction.WasReleasedThisFrame())
        {
            // 重新记录左手位置和基准位置
            leftHandRecordPos = leftHand.localPosition;
            rigRecordPos = rigT.position;
        }

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
            rigT.position = rigRecordPos - offset * moveSpeed * TrackingSpace.lossyScale.x;
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
            rigT.position = rigRecordPos - offset * moveSpeed * TrackingSpace.lossyScale.x;
        }
    }

}
