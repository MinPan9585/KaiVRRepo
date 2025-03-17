using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    public Transform targetUITransform;
    public Transform lookTargetTransform;
    public float lookRotateSpeed = 10f;
    public Transform moveTargetTransform;
    public float moveSpeed = 10f;
    public float moveDistance = 10f;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (targetUITransform != null && lookTargetTransform != null)
        {
            // 计算目标方向
            Vector3 targetDirection = targetUITransform.position - lookTargetTransform.position;
            // targetDirection.y = 0; // 保持UI在水平面上旋转

            // 计算目标旋转
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

            // 平滑旋转到目标方向
            targetUITransform.rotation = Quaternion.Lerp(targetUITransform.rotation, targetRotation, lookRotateSpeed * Time.deltaTime);
        }

        if (moveTargetTransform != null)
        {
            targetUITransform.position = Vector3.Lerp(targetUITransform.position, moveTargetTransform.position + offset, moveSpeed * Time.deltaTime);
        }
    }
}
