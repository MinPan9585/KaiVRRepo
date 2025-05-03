using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool waitForShooting;
    Vector3 currentScale;
    public Transform leftCon;
    public Transform rightCon;

    public float k;

    // Start is called before the first frame update
    void Start()
    {
        currentScale = new Vector3(0.1f, 0.1f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        if (waitForShooting)
        {
            //currentScale += new Vector3(0.1f, 0.1f, 0.1f) * Time.deltaTime;
            //transform.localScale  = Mathf.Clamp(currentScale.x, 0.1f, 0.3f) * Vector3.one;

            transform.position = (rightCon.position + leftCon.position) * 0.5f;

            transform.localScale = Vector3.Distance(rightCon.position, leftCon.position) * Vector3.one * k;
        }
        else
        {
            transform.forward = rightCon.forward;
            transform.position += transform.forward * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //
    }
}
