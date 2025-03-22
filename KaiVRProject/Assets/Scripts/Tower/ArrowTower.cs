using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    private Transform target;

    [Header("Attributes")]

    public float range = 0f;
    public float fireRate = 1f;
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

        foreach (GameObject enemy in enemies) {

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance) {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }

        }

        if (nearestEnemy != null && shortestDistance <= range * 1.1) {
            target = nearestEnemy.transform;
        } else
        {
            target = null;
        }

    }


    // Update is called once per frame
    void Update()
    {
        if (target == null) { return; }

        Vector3 dir = target.position - transform.position;
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
        GameObject arrowGo = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        ArrowProjectile arrow = arrowGo.GetComponent<ArrowProjectile>();
        if (arrow != null)
        {
            arrow.Seek(target); // Ensure target has EnemyHealth
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
