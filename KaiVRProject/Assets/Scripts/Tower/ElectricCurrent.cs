using UnityEngine;
using System.Collections.Generic;

public class ElectricCurrent : MonoBehaviour
{
    private List<Transform> targets = new List<Transform>();
    private float chainRange;
    private int maxChainTargets;
    private float towerRange;
    private Vector3 firePointPosition;
    public float damagePerSecond = 15f;
    public float slowFactor = 0.5f;
    public float slowDuration = 2f;
    public LineRenderer lineRenderer;
    private float lifetime = 1f;

    public void Initialize(Transform initialTarget, float _chainRange, int _maxChainTargets, float _towerRange, Vector3 _firePointPosition)
    {
        targets.Add(initialTarget);
        chainRange = _chainRange;
        maxChainTargets = _maxChainTargets;
        towerRange = _towerRange;
        firePointPosition = _firePointPosition;

        ChainToNextTarget();
        SetupLineRenderer();
        Invoke(nameof(DestroySelf), lifetime);
    }

    void ChainToNextTarget()
    {
        if (targets.Count >= maxChainTargets) return;

        Transform lastTarget = targets[targets.Count - 1];
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            if (!targets.Contains(enemy.transform) && Vector3.Distance(transform.position, enemy.transform.position) <= towerRange)
            {
                float distance = Vector3.Distance(lastTarget.position, enemy.transform.position);
                if (distance < shortestDistance && distance <= chainRange)
                {
                    shortestDistance = distance;
                    nearestEnemy = enemy.transform;
                }
            }
        }

        if (nearestEnemy != null)
        {
            targets.Add(nearestEnemy);
            ChainToNextTarget();
        }
    }

    Vector3 GetTargetCenter(Transform target)
    {
        Collider col = target.GetComponent<Collider>();
        if (col != null)
        {
            Vector3 centerOffset = col.bounds.center - target.position; 
            return target.position + centerOffset;
        }
        return target.position + Vector3.up * 1f;
    }

    void SetupLineRenderer()
    {
        lineRenderer.positionCount = targets.Count + 1;
        lineRenderer.SetPosition(0, firePointPosition);
        for (int i = 0; i < targets.Count; i++)
        {
            lineRenderer.SetPosition(i + 1, GetTargetCenter(targets[i]));
        }
    }

    void Update()
    {
        if (targets.Count > 0)
        {
            lineRenderer.positionCount = targets.Count + 1;
            lineRenderer.SetPosition(0, firePointPosition);
            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null)
                {
                    targets.RemoveAt(i);
                    i--;
                    continue;
                }
                lineRenderer.SetPosition(i + 1, GetTargetCenter(targets[i]));
            }
        }

        foreach (Transform target in targets)
        {
            if (target != null)
            {
                EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerSecond * Time.deltaTime);
                }

                Enemy pathEnemy = target.GetComponent<Enemy>();
                if (pathEnemy != null)
                {
                    pathEnemy.ApplySlow(slowFactor, slowDuration);
                }

                TowerEnemies towerEnemy = target.GetComponent<TowerEnemies>();
                if (towerEnemy != null)
                {
                    towerEnemy.ApplySlow(slowFactor, slowDuration);
                }
            }
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}