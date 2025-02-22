using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoner : MonoBehaviour
{
    public static Dictionary<int, GameObject> enemyPrefabs;

    public static List<Transform> EnemiesInGameTransform;

    public static Dictionary<int, Queue<Enemy>> enemyObjectPools;
    public static List<Enemy> enemiesInGame;

    private static bool isInit;

    public static void Init()
    {
        if (!isInit)
        {
            enemyPrefabs = new Dictionary<int, GameObject>();
            enemyObjectPools = new Dictionary<int, Queue<Enemy>>();
            EnemiesInGameTransform = new List<Transform>();
            enemiesInGame = new List<Enemy>();

            EnemySummonData[] enemies = Resources.LoadAll<EnemySummonData>("Enemies");

            foreach (EnemySummonData enemy in enemies)
            {
                enemyPrefabs.Add(enemy.enemyID, enemy.enemyPrefab);
                enemyObjectPools.Add(enemy.enemyID, new Queue<Enemy>());
            }

            isInit = true;
        }
        else
        {
            Debug.Log("Entity Summoner: this class is already initialized");
        }
    }

    public static Enemy SummonEnemy(int enemyID)
    {
        Enemy summonedEnemy = null;
        if (enemyPrefabs.ContainsKey(enemyID))
        {
            Queue<Enemy> referenceedQueue = enemyObjectPools[enemyID];
            if (referenceedQueue.Count > 0)
            {
                summonedEnemy= referenceedQueue.Dequeue();
                summonedEnemy.Init();

                summonedEnemy.gameObject.SetActive(true);
            } 
            else
            {
                GameObject newEnemy = Instantiate(enemyPrefabs[enemyID], GameLoopManager.NodePositionsOne[0], Quaternion.identity);
                summonedEnemy = newEnemy.GetComponent<Enemy>();
                summonedEnemy.Init();
            }
        }
        else
        {
            return null;
        }

        EnemiesInGameTransform.Add(summonedEnemy.transform);

        enemiesInGame.Add(summonedEnemy);
        summonedEnemy.ID = enemyID; 
        return summonedEnemy;
    }

    public static void RemoveEnemy(Enemy EnemyToRemove)
    {
        enemyObjectPools[EnemyToRemove.ID].Enqueue(EnemyToRemove);
        EnemyToRemove.gameObject.SetActive(false);
        EnemiesInGameTransform.Remove(EnemyToRemove.transform);
        enemiesInGame.Remove(EnemyToRemove);
    }
}