using System;
using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private Transform initialTarget; // Renamed to clarify it’s the initial target
    public float speed = 50f;
    public GameObject impactEffect;
    private float damage = 60f;
    private bool hasHit = false;

    public void Seek(Transform _target)
    {
        initialTarget = _target;
        // Set initial direction even if target dies
        if (initialTarget != null)
        {
            Vector3 dir = (initialTarget.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }

    void Update()
    {
        if (hasHit) return;

        float distanceThisFrame = speed * Time.deltaTime;
        Vector3 dir = transform.forward; // Continue in current direction

        // Move forward
        transform.Translate(dir * distanceThisFrame, Space.World);

        // Check if we hit the ground (assuming ground is at y = 0)
        if (transform.position.y <= 0f)
        {
            HitGround();
            return;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasHit && collision.gameObject.CompareTag("enemy"))
        {
            HitTarget(collision.gameObject);
        }
    }

    void HitTarget(GameObject hitObject)
    {
        if (hasHit) return;
        hasHit = true;

        if (impactEffect != null)
        {
            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 2f);
        }

        EnemyHealth enemyHealth = hitObject.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void HitGround()
    {
        if (hasHit) return;
        hasHit = true;

        if (impactEffect != null)
        {
            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 2f);
        }

        Destroy(gameObject);
    }
}