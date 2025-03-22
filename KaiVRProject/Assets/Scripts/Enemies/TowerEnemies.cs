using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnemies : MonoBehaviour
{
    public GameObject[] towers;
    Vector3 direction;
    public float speed = 0.1f;
    private float originalSpeed; // Store the original speed

    void Start()
    {
        originalSpeed = speed; // Initialize original speed
    }

    void Update()
    {
        LookAt();
        MoveTowardTarget();
    }

    void MoveTowardTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, FindClosestTower(), speed);
    }

    void LookAt()
    {
        direction = FindClosestTower() - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    public Vector3 FindClosestTower()
    {
        Vector3 closestXY = Vector3.zero;
        float distance = 0;
        float shortestDistance = 1000;
        towers = GameObject.FindGameObjectsWithTag("Tower");

        for (int i = 0; i < towers.Length; i++)
        {
            distance = Vector3.Distance(transform.position, towers[i].transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestXY = towers[i].transform.position;
            }
        }

        return closestXY;
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