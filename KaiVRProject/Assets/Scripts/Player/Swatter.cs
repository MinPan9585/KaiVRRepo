using UnityEngine;

public class Swatter : MonoBehaviour
{
    public float knockbackForce = 10f;

    private Rigidbody playerRigidbody;

    private void Awake()
    {
        // Optional: if you want to add player velocity to knockback
        playerRigidbody = transform.root.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("enemy")) // Make sure your PlayerEnemy has tag "Enemy"
        {
            Rigidbody enemyRb = other.attachedRigidbody;
            if (enemyRb != null)
            {
                Vector3 knockDirection = GetKnockbackDirection();
                enemyRb.isKinematic = false;
                enemyRb.useGravity = true;

                // Apply force for knockback, including some upward force
                Vector3 force = knockDirection * knockbackForce + Vector3.up * (knockbackForce * 0.5f);
                enemyRb.AddForce(force, ForceMode.Impulse);

                // Optional: You can call enemy-specific logic here to mark as swatted, etc.
            }
        }
    }

    private Vector3 GetKnockbackDirection()
    {
        // Use the swatter¡¯s forward direction as knockback direction
        return transform.forward.normalized;
    }
}
