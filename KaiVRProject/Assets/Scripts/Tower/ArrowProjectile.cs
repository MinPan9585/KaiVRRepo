using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    private Transform target;
    public float speed = 50f;
    public GameObject impactEffect;
    public float damage = 60f;

    public void Seek(Transform _target)
    {
        target = _target;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir.normalized, out hit, distanceThisFrame))
        {
            if (hit.collider.transform == target)
            {
                HitTarget();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == target.gameObject)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        if (impactEffect != null)
        {
            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);
            Destroy(effectIns, 2f);
        }

        EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}