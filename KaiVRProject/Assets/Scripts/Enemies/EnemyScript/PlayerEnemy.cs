using UnityEngine;

public class PlayerEnemy : MonoBehaviour
{
    public GameObject Player; 
    private Vector3 targetPosition; 
    private bool isWalking = true; 
    private float walkSpeed = 2f;
    private float walkDistance; 
    private float distanceWalked = 0f; 
    private Vector3 launchVelocity; 
    private bool hasLaunched = false;

    private bool destroyed = false;

    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        float distanceToPlayer = Vector3.Distance(transform.position, new Vector3(Player.transform.position.x, 0, Player.transform.position.z));
        walkDistance = Random.Range(distanceToPlayer * 0.2f, distanceToPlayer * 0.8f);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.freezeRotation = true;

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }

    void Update()
    {
        walkSpeed = (2f + (0.2f * WaveSpawner.waveIndex));
        if (isWalking)
        {
            WalkTowardsTarget();
        }

        if (destroyed)
        {
            WaveSpawner.enemiesAlive--;
            destroyed = false;
        }
    }

    void WalkTowardsTarget()
    {
        Vector3 currentPosition = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 direction = (targetPosition - currentPosition).normalized;
        Vector3 movement = direction * walkSpeed * Time.deltaTime;

        transform.position += movement;
        distanceWalked += movement.magnitude;

        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        if (movement != Vector3.zero)
        {
            Vector3 flatDirection = new Vector3(direction.x, 0, direction.z).normalized;
            transform.rotation = Quaternion.LookRotation(flatDirection, Vector3.up);
        }

        if (distanceWalked >= walkDistance && !hasLaunched)
        {
            LaunchTowardsPlayer();
        }
    }

    void LaunchTowardsPlayer()
    {
        isWalking = false;
        hasLaunched = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.freezeRotation = false;

        Vector3 toPlayer = Player.transform.position - transform.position;
        float horizontalDistance = new Vector3(toPlayer.x, 0, toPlayer.z).magnitude;
        float verticalDistance = Player.transform.position.y - transform.position.y; 

        float peakHeight = Player.transform.position.y + 5f - transform.position.y;
        float gravity = Physics.gravity.magnitude; // 9.81

        float timeToPeak = Mathf.Sqrt(2 * peakHeight / gravity);
        float fallDistance = peakHeight - verticalDistance; 
        float timeToFall = Mathf.Sqrt(2 * fallDistance / gravity);
        float totalTime = timeToPeak + timeToFall;

        float horizontalSpeed = horizontalDistance / totalTime;
        Vector3 horizontalVelocity = new Vector3(toPlayer.x, 0, toPlayer.z).normalized * horizontalSpeed;

        float verticalSpeed = timeToPeak * gravity;

        launchVelocity = horizontalVelocity + Vector3.up * verticalSpeed;
        rb.velocity = launchVelocity;
    }


}