using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    private int currentPath;
    private Transform target;
    private int wavePointIndex = 0;

    //Vector3 spawnThree = new Vector3(11.0f, 0.0f, -16.5f);
    //Vector3 spawnFour = new Vector3(11.0f, 0.0f, -16.5f);

    private Waypoints waypoints;
    // Start is called before the first frame update
    void Start()
    {

        target = GameObject.Find("Waypoints").transform.GetComponent<Waypoints>().points[0];
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
        if (wavePointIndex >= GameObject.Find("Waypoints").transform.GetComponent<Waypoints>().points.Length)
        {
            Destroy(gameObject);
            return;
        }
        target = GameObject.Find("Waypoints").transform.GetComponent<Waypoints>().points[wavePointIndex];
        wavePointIndex++;
    }

}
