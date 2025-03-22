using UnityEngine;
using System.Collections;

public class CannonballProjectile : MonoBehaviour
{
    private Transform initialTarget;
    private float speed = 5f;
    private bool hasExploded = false;
    private float lifetime = 5f;

    [Header("Explosion Settings")]
    public float explosionRadius = 3f;
    public float maxDamage = 50f;
    public string enemyTag = "enemy";
    public GameObject explosionEffect;

    [Header("Visual Settings")]
    public GameObject radiusVisualizerPrefab;
    public float visualDuration = 0.5f;

    public void Seek(Transform _target)
    {
        initialTarget = _target;
        StartCoroutine(MoveToTarget());
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition;

        if (initialTarget != null)
        {
            targetPosition = PredictTargetPosition(startPosition);
            targetPosition.y = 0f; // Ground level
        }
        else
        {
            targetPosition = startPosition + transform.forward * 100f;
            targetPosition.y = 0f;
        }

        Vector3 flatStart = new Vector3(startPosition.x, 0, startPosition.z);
        Vector3 flatTarget = new Vector3(targetPosition.x, 0, targetPosition.z);
        float horizontalDistance = Vector3.Distance(flatStart, flatTarget);

        float angleInRadians = 18f * Mathf.Deg2Rad;
        Vector3 flatDirection = (flatTarget - flatStart).normalized;

        float vy = speed * Mathf.Sin(angleInRadians);
        float g = Physics.gravity.magnitude;
        float timeToPeak = vy / g;
        float peakHeight = startPosition.y + (vy * timeToPeak) - (0.5f * g * timeToPeak * timeToPeak);
        float timeToGround = Mathf.Sqrt(2 * peakHeight / g);
        float totalTime = timeToPeak + timeToGround;

        float horizontalSpeed = horizontalDistance / totalTime;
        Vector3 velocity = flatDirection * horizontalSpeed + Vector3.up * vy;

        float timeAlive = 0f;

        while (timeAlive < lifetime && !hasExploded)
        {
            timeAlive += Time.deltaTime;

            Vector3 newPosition = startPosition + velocity * timeAlive + 0.5f * Physics.gravity * timeAlive * timeAlive;

            if (newPosition.y <= 0f)
            {
                newPosition.y = 0f;
                transform.position = newPosition;
                Explode();
                yield break;
            }

            transform.position = newPosition;
            yield return null;
        }

        if (!hasExploded)
        {
            Explode();
        }
    }

    private Vector3 PredictTargetPosition(Vector3 startPosition)
    {
        if (initialTarget == null) return startPosition + transform.forward * 100f;

        // Check for path-following enemy
        Enemy pathEnemy = initialTarget.GetComponent<Enemy>();
        if (pathEnemy != null)
        {
            return PredictPathEnemyPosition(startPosition, pathEnemy);
        }

        // Check for tower-attacking enemy
        TowerEnemies towerEnemy = initialTarget.GetComponent<TowerEnemies>();
        if (towerEnemy != null)
        {
            return PredictTowerEnemyPosition(startPosition, towerEnemy);
        }

        // Fallback: Assume static position
        return initialTarget.position;
    }

    private Vector3 PredictPathEnemyPosition(Vector3 startPosition, Enemy pathEnemy)
    {
        float g = Physics.gravity.magnitude;
        float vy = speed * Mathf.Sin(18f * Mathf.Deg2Rad);
        float timeToPeak = vy / g;
        float peakHeight = startPosition.y + (vy * timeToPeak) - (0.5f * g * timeToPeak * timeToPeak);
        float timeToGround = Mathf.Sqrt(2 * peakHeight / g);
        float flightTime = timeToPeak + timeToGround;

        // Access waypoints and current index via reflection or public fields (assuming Waypoints script exists)
        GameObject waypointsObj = pathEnemy.GetType().GetField("waypoints", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(pathEnemy) as GameObject;
        int wavePointIndex = (int)pathEnemy.GetType().GetField("wavePointIndex", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(pathEnemy);
        float enemySpeed = pathEnemy.speed;

        if (waypointsObj == null || wavePointIndex < 0) return initialTarget.position;

        Waypoints waypoints = waypointsObj.GetComponent<Waypoints>();
        if (waypoints == null || waypoints.points.Length == 0) return initialTarget.position;

        Vector3 currentPos = initialTarget.position;
        float remainingTime = flightTime;
        int index = wavePointIndex;

        while (remainingTime > 0 && index < waypoints.points.Length)
        {
            Vector3 nextWaypoint = waypoints.points[index].position;
            float distanceToNext = Vector3.Distance(currentPos, nextWaypoint);
            float timeToNext = distanceToNext / enemySpeed;

            if (remainingTime >= timeToNext)
            {
                currentPos = nextWaypoint;
                remainingTime -= timeToNext;
                index++;
            }
            else
            {
                Vector3 direction = (nextWaypoint - currentPos).normalized;
                return currentPos + direction * enemySpeed * remainingTime;
            }
        }

        return currentPos; // If time exceeds path, return last position
    }

    private Vector3 PredictTowerEnemyPosition(Vector3 startPosition, TowerEnemies towerEnemy)
    {
        float g = Physics.gravity.magnitude;
        float vy = speed * Mathf.Sin(18f * Mathf.Deg2Rad);
        float timeToPeak = vy / g;
        float peakHeight = startPosition.y + (vy * timeToPeak) - (0.5f * g * timeToPeak * timeToPeak);
        float timeToGround = Mathf.Sqrt(2 * peakHeight / g);
        float flightTime = timeToPeak + timeToGround;

        Vector3 targetTowerPos = towerEnemy.FindClosestTower();
        Vector3 targetVelocity = (targetTowerPos - initialTarget.position).normalized * towerEnemy.speed;

        Vector3 predictedPosition = initialTarget.position + targetVelocity * flightTime;
        float distanceToTower = Vector3.Distance(initialTarget.position, targetTowerPos);
        float timeToTower = distanceToTower / towerEnemy.speed;

        if (timeToTower < flightTime && towerEnemy.speed > 0)
        {
            predictedPosition = targetTowerPos;
            flightTime = timeToTower;
        }

        float distance = Vector3.Distance(startPosition, predictedPosition);
        float horizontalSpeed = distance / flightTime;
        flightTime = distance / horizontalSpeed;

        if (timeToTower < flightTime && towerEnemy.speed > 0)
        {
            predictedPosition = targetTowerPos;
        }
        else
        {
            predictedPosition = initialTarget.position + targetVelocity * flightTime;
        }

        return predictedPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded && collision.gameObject.CompareTag(enemyTag))
        {
            Explode();
        }
    }

    private void Explode()
    {
        if (hasExploded) return;
        hasExploded = true;

        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, Quaternion.identity);
        }

        if (radiusVisualizerPrefab != null)
        {
            GameObject visual = Instantiate(radiusVisualizerPrefab, transform.position, Quaternion.identity);
            visual.transform.localScale = Vector3.one * explosionRadius * 2f;
            Destroy(visual, visualDuration);
        }

        Vector3 explosionCenter = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionCenter, explosionRadius);
        foreach (Collider hit in colliders)
        {
            if (hit.CompareTag(enemyTag))
            {
                EnemyHealth enemyHealth = hit.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    float distance = Vector3.Distance(explosionCenter, hit.transform.position);
                    float damageMultiplier = Mathf.Clamp01(1f - (distance / explosionRadius));
                    float damage = maxDamage * damageMultiplier;
                    enemyHealth.TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
        Gizmos.color = new Color(1f, 1f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
}