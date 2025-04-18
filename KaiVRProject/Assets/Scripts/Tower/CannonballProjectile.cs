using UnityEngine;
using System.Collections;

public class CannonballProjectile : MonoBehaviour
{
    private Transform target;
    private float speed = 10f;
    private bool hasExploded = false;
    private float lifetime = 5f;

    [Header("Explosion Settings")]
    public float explosionRadius = 3f;
    public float maxDamage = 50f;
    public string enemyTag = "enemy";
    public GameObject explosionEffectPrefab;

    [Header("Slow Effect Settings")]
    public float slowFactor = 0.5f; 
    public float slowDuration = 3f; 

    [Header("Visual Settings")]
    public GameObject radiusVisualizerPrefab;
    public float visualDuration = 0.5f;

    public void Seek(Transform _target)
    {
        target = _target;
        StartCoroutine(MoveToTarget());
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private IEnumerator MoveToTarget()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = target != null ? target.position : startPosition + transform.forward * 100f;
        targetPosition.y = 0f;

        Vector3 flatStart = new Vector3(startPosition.x, 0, startPosition.z);
        Vector3 flatTarget = new Vector3(targetPosition.x, 0, targetPosition.z);
        float horizontalDistance = Vector3.Distance(flatStart, flatTarget);

        float angleInRadians = 18f * Mathf.Deg2Rad;
        Vector3 flatDirection = (flatTarget - flatStart).normalized;

        float vy = speed * Mathf.Sin(angleInRadians);
        float vz = speed * Mathf.Cos(angleInRadians);

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
                Explode(null);
                yield break;
            }

            transform.position = newPosition;
            yield return null;
        }

        if (!hasExploded)
        {
            Explode(null);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded && collision.gameObject.CompareTag(enemyTag))
        {
            Explode(collision.gameObject);
        }
    }

    private void Explode(GameObject hitObject)
    {
        if (hasExploded) return;
        hasExploded = true;

        Material hitMaterial = null;
        if (hitObject != null)
        {
            Renderer renderer = hitObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                hitMaterial = renderer.material;
            }
        }
        else
        {
            RaycastHit hit;
            Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 1f))
            {
                Renderer groundRenderer = hit.collider.GetComponent<Renderer>();
                if (groundRenderer != null)
                {
                    hitMaterial = groundRenderer.material;
                }
            }
        }

        if (explosionEffectPrefab != null)
        {
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Renderer effectRenderer = explosionEffect.GetComponentInChildren<Renderer>();
            if (effectRenderer != null && hitMaterial != null)
            {
                effectRenderer.material = hitMaterial;
            }
            Destroy(explosionEffect, 2f);
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

                Enemy pathEnemy = hit.GetComponent<Enemy>();
                if (pathEnemy != null)
                {
                    pathEnemy.ApplySlow(slowFactor, slowDuration);
                }

                TowerEnemies towerEnemy = hit.GetComponent<TowerEnemies>();
                if (towerEnemy != null)
                {
                    towerEnemy.ApplySlow(slowFactor, slowDuration);
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