using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnemies : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] towers;
    Vector3 direction;
    public float speed = 0.1f;

    // Update is called once per frame
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

    Vector3 FindClosestTower()
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
}