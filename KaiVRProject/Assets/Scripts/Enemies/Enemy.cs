using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    private float originalSpeed; // Store the original speed
    private int currentPath;
    private Transform target;
    private int wavePointIndex = 0;
    private GameObject waypoints = null;

    private Vector3 spawn1 = new Vector3(11.2f, 0f, -18f);
    private Vector3 spawn2 = new Vector3(14.4f, 0f, 15.2f);

    void Start()
    {
        originalSpeed = speed; // Initialize original speed
        if (Vector3.Distance(transform.position, spawn1) < 0.2)
        {
            waypoints = GameObject.Find("Waypoints");
            target = waypoints.transform.GetComponent<Waypoints>().points[0];
        }
        else if (Vector3.Distance(transform.position, spawn2) < 0.2)
        {
            waypoints = GameObject.Find("Waypoints2");
            target = waypoints.transform.GetComponent<Waypoints>().points[0];
        }
    }

    void Update()
    {
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position, target.position) <= .03f)
        {
            GetNextWaypoint(currentPath);
        }
    }

    void GetNextWaypoint(int Path)
    {
        if (wavePointIndex >= waypoints.transform.GetComponent<Waypoints>().points.Length)
        {
            Destroy(gameObject);
            return;
        }
        target = waypoints.transform.GetComponent<Waypoints>().points[wavePointIndex];
        wavePointIndex++;
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        StartCoroutine(SlowEffect(slowFactor, duration));
    }

    private IEnumerator SlowEffect(float slowFactor, float duration)
    {
        speed = originalSpeed * slowFactor; // Reduce speed
        yield return new WaitForSeconds(duration);
        speed = originalSpeed; // Restore speed
    }
}