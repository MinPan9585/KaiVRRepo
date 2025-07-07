using UnityEngine;

public class SphereExplosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float explosionRadiusMultiplier = 2f;
    public float maxExplosionDamage = 80f;         // Max damage when fully charged
    public float maxSphereSize = 3f;               // This should match your launcher script
    public LayerMask enemyLayer;

    [Header("Explosion Visual Effect")]
    public GameObject explosionEffectPrefab;

    private float currentSize;
    private bool exploded = false;

    void Start()
    {
        currentSize = transform.localScale.x;
        Invoke(nameof(SelfDestruct), 6f);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            Explode(collision.gameObject);
        }
    }

    void Explode(GameObject hitObject)
    {
        if (exploded) return;
        exploded = true;

        float radius = currentSize * explosionRadiusMultiplier;

        // cale damage by size (capped)
        float normalizedSize = Mathf.Clamp01(currentSize / maxSphereSize);
        float damage = normalizedSize * maxExplosionDamage;

        // Copy material from floor (optional visual effect)
        Material hitMaterial = null;
        if (hitObject != null)
        {
            Renderer renderer = hitObject.GetComponent<Renderer>();
            if (renderer != null)
                hitMaterial = renderer.material;
        }
        else
        {
            RaycastHit hit;
            Vector3 rayOrigin = transform.position + Vector3.up * 0.5f;
            if (Physics.Raycast(rayOrigin, Vector3.down, out hit, 1f))
            {
                Renderer groundRenderer = hit.collider.GetComponent<Renderer>();
                if (groundRenderer != null)
                    hitMaterial = groundRenderer.material;
            }
        }

        // Spawn explosion effect
        if (explosionEffectPrefab != null)
        {
            GameObject explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
            Renderer effectRenderer = explosionEffect.GetComponentInChildren<Renderer>();
            if (effectRenderer != null && hitMaterial != null)
                effectRenderer.material = hitMaterial;

            Destroy(explosionEffect, 2f);
        }

        //  Deal damage
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, radius, enemyLayer);
        foreach (var enemy in hitEnemies)
        {
            EnemyHealth eh = enemy.GetComponent<EnemyHealth>();
            if (eh != null)
                eh.TakeDamage(damage);
        }

        Destroy(gameObject);
    }

    void SelfDestruct()
    {
        if (!exploded)
        {
            Destroy(gameObject);
        }
    }
}
