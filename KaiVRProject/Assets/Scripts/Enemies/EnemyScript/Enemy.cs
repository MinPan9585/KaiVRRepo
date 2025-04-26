using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 1f;
    private float originalSpeed;
    private int currentPath;
    private Transform target;
    private int wavePointIndex = 0;
    private GameObject waypoints = null;

    private Vector3 spawn1 = new Vector3(11.2f, 0f, -18f);
    private Vector3 spawn2 = new Vector3(14.4f, 0f, 15.2f);
    private Vector3 spawn3 = new Vector3(-16.8f, 0f, -12.8f);
    private Vector3 spawn4 = new Vector3(-9.6f, 0f, 18.6f);
    private Vector3 spawn5 = new Vector3(-20.7f, 0f, 4.8f);

    void Start()
    {
        originalSpeed = speed;
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
        else if (Vector3.Distance(transform.position, spawn3) < 0.2)
        {
            waypoints = GameObject.Find("Waypoints3");
            target = waypoints.transform.GetComponent<Waypoints>().points[0];
        }
        else if (Vector3.Distance(transform.position, spawn4) < 0.2)
        {
            waypoints = GameObject.Find("Waypoints4");
            target = waypoints.transform.GetComponent<Waypoints>().points[0];
        }
        else if (Vector3.Distance(transform.position, spawn5) < 0.2)
        {
            waypoints = GameObject.Find("Waypoints5");
            target = waypoints.transform.GetComponent<Waypoints>().points[0];
        }
    }

    void Update()
    {
        speed = (1f + (0.1f * WaveSpawner.waveIndex));
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
            WaveSpawner.enemiesAlive--;
            return;
        }
        target = waypoints.transform.GetComponent<Waypoints>().points[wavePointIndex];
        wavePointIndex++;
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SlowEffect(slowFactor, duration));
    }

    private System.Collections.IEnumerator SlowEffect(float slowFactor, float duration)
    {
        speed = originalSpeed * slowFactor;
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
    }
}