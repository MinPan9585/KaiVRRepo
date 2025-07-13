using UnityEngine;

public class Swatter : MonoBehaviour
{
    public float knockbackForce = 10f;
    public float upwardForce = 4f;

    private Vector3 lastPosition;
    private Vector3 velocity;

    void Update()
    {
        velocity = (transform.position - lastPosition) / Time.deltaTime;
        lastPosition = transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("enemy"))
        {
            Rigidbody enemyRb = collision.rigidbody;
            if (enemyRb != null)
            {
                // Wake up physics
                enemyRb.isKinematic = false;
                enemyRb.useGravity = true;

                // Use hand velocity to create realistic swat force
                Vector3 knockDirection = velocity.normalized;
                Vector3 force = knockDirection * knockbackForce + Vector3.up * upwardForce;
                enemyRb.AddForce(force, ForceMode.Impulse);

                // Tell enemy to start delayed death
                PlayerEnemy enemy = collision.gameObject.GetComponent<PlayerEnemy>();
                if (enemy != null)
                {
                    enemy.OnSwatted();
                }
            }
        }
    }
}
