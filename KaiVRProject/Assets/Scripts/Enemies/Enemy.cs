using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    private int currentPath;
    private Transform target;
    private int wavePointIndex = 0;
    private GameObject waypoints = null;

    private Vector3 spawn1 = new Vector3(11.2f, 0f, -18f);
    private Vector3 spawn2 = new Vector3(14.4f, 0f, 15.2f);

    // Start is called before the first frame update
    void Start()
    {
        if (Vector3.Distance(transform.position, spawn1) < 0.2)
        {
            waypoints = GameObject.Find("Waypoints");
            target = waypoints.transform.GetComponent<Waypoints>().points[0];
        } else if (Vector3.Distance(transform.position, spawn2) < 0.2)
        {
            waypoints = GameObject.Find("Waypoints2");
            target = waypoints.transform.GetComponent<Waypoints>().points[0];
        }
    }

    // Update is called once per frame
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



}
