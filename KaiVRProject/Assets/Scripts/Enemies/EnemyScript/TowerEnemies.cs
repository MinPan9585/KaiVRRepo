//using UnityEditor.UI;
using UnityEngine;

public class TowerEnemies : MonoBehaviour
{
    public GameObject[] towers;
    Vector3 direction;
    private float speed;
    private float originalSpeed;

    private bool destroyed = false;

    void Start()
    {
        originalSpeed = speed;
    }

    void Update()
    {
        speed = (0.05f + (0.002f * WaveSpawner.waveIndex));

        LookAt();
        MoveTowardTarget();

        if (destroyed)
        {
            WaveSpawner.enemiesAlive--;
            destroyed = false;
        }
    }

    void MoveTowardTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, FindClosestTower(), speed);
    }

    void LookAt()
    {
        direction = FindClosestTower() - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = rotation;
    }

    public Vector3 FindClosestTower()
    {
        Vector3 closestXY = Vector3.zero;
        float distance = 0;
        float shortestDistance = 1000;
        towers = GameObject.FindGameObjectsWithTag("Tower");

        for (int i = 0; i < towers.Length; i++)
        {
            distance = Vector3.Distance(transform.position, towers[i].transform.position);

            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestXY = towers[i].transform.position;
            }
        }

        return closestXY;
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        StopAllCoroutines();
        StartCoroutine(SlowEffect(slowFactor, duration));
    }

    private System.Collections.IEnumerator SlowEffect(float slowFactor, float duration)
    {
        speed = originalSpeed * slowFactor;
        yield return new WaitForSeconds(duration);
        speed = originalSpeed;
    }
}