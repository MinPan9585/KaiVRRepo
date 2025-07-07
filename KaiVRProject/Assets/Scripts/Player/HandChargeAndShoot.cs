using UnityEngine;
using UnityEngine.XR;

public class HandChargeAndShoot : MonoBehaviour
{
    public Transform leftHand;
    public Transform rightHand;
    public Camera headsetCamera;
    public GameObject spherePrefab;
    public float maxSize = 1.5f;
    public float chargeSpeed = 10f;
    public float baseShootForce = 80f;

    private GameObject chargingSphere;
    private bool isCharging = false;
    private float chargeTimer = 0f;

    void Update()
    {
        InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.gripButton, out bool leftPressed);
        InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.gripButton, out bool rightPressed);

        if (leftPressed && rightPressed && !isCharging)
            StartCharging();

        if (isCharging)
        {
            if (leftPressed && rightPressed)
            {
                chargeTimer += Time.deltaTime;
                UpdateSize();
            }
            else
            {
                if (chargeTimer >= 0.2f)
                    Shoot();
                else
                    Cancel();
            }
        }
    }

    void StartCharging()
    {
        isCharging = true;
        chargeTimer = 0f;
        chargingSphere = Instantiate(spherePrefab, MidPoint(), Quaternion.identity);
        chargingSphere.transform.localScale = Vector3.one * 0.1f;
        chargingSphere.GetComponent<Rigidbody>().isKinematic = true;
        chargingSphere.GetComponent<Collider>().enabled = false;
        chargingSphere.GetComponent<SphereExplosion>().enabled = false;
    }

    void UpdateSize()
    {
        float dist = Vector3.Distance(leftHand.position, rightHand.position);
        float size = Mathf.Clamp(dist * chargeSpeed, 0.1f, maxSize);
        chargingSphere.transform.localScale = Vector3.one * size;
        chargingSphere.transform.position = MidPoint();
    }

    void Shoot()
    {
        float size = chargingSphere.transform.localScale.x;
        float force = baseShootForce * Mathf.Clamp(size, 1f, 3f);

        Rigidbody rb = chargingSphere.GetComponent<Rigidbody>();
        Collider col = chargingSphere.GetComponent<Collider>();

        chargingSphere.transform.position = MidPoint();
        rb.isKinematic = false;
        rb.velocity = headsetCamera.transform.forward * force;
        col.enabled = true;

        chargingSphere.GetComponent<SphereExplosion>().enabled = true;
        chargingSphere = null;
        isCharging = false;
    }

    void Cancel()
    {
        Destroy(chargingSphere);
        chargingSphere = null;
        isCharging = false;
    }

    Vector3 MidPoint()
    {
        return (leftHand.position + rightHand.position) / 2f;
    }
}
