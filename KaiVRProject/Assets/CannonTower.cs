using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonTower : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]
    public float range = 0f;
    public float fireRate = 1f;
    public float baseSpeed = 5f; // Base speed for closest range
    private float fireCountdown = 0f;

    [Header("Unity Setup")]
    public Transform topRotate;
    public float turnSpeed = 10f;
    public string enemyTag = "enemy";
    public GameObject bulletPrefab;
    public Transform firePoint;

    void Start()
    {
        InvokeRepeating("UpdateTarget", 0f, 0.01f);
    }

    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range * 1.1)
        {
            target = nearestEnemy.transform;
        }
        else
        {
            target = null;
        }
    }

    void Update()
    {
        if (target == null) { return; }

        // Only rotate in Y-axis
        Vector3 dir = target.position - transform.position;
        dir.y = 0; // Keep rotation horizontal
        Quaternion lookAt = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(topRotate.rotation, lookAt, Time.deltaTime * turnSpeed).eulerAngles;
        topRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);

        if (fireCountdown <= 0)
        {
            Shoot();
            fireCountdown = 1f / fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    void Shoot()
    {
        GameObject cannonballGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        CannonballProjectile cannonball = cannonballGo.GetComponent<CannonballProjectile>();

        if (cannonball != null)
        {
            float distance = Vector3.Distance(firePoint.position, target.position);
            float adjustedSpeed = baseSpeed * (distance / range) * 2f;
            cannonball.SetSpeed(adjustedSpeed);
            cannonball.Seek(target);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}