using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class GameLoopManager : MonoBehaviour
{
    public static Vector3[] NodePositionsOne;
    private static Queue<Enemy> EnemiesToRemove;
    private static Queue<int> EnemyIDsToSummon;

    public Transform NodeParentOne;
    public bool GameEnd;
    // Start is called before the first frame update
    private void Start()
    {
        EnemyIDsToSummon = new Queue<int>();
        EnemiesToRemove = new Queue<Enemy>();

        EntitySummoner.Init();

        NodePositionsOne = new Vector3[NodeParentOne.childCount];

        for(int i = 0; i < NodePositionsOne.Length; i++)
        {
            NodePositionsOne[i] = NodeParentOne.GetChild(i).position;
        }

        StartCoroutine(GameLoop());
        InvokeRepeating("SummonTest", 0f, 1f);
    }

    void SummonTest()
    {
        enqueueEnemyIDToSummon(1);
    }

    IEnumerator GameLoop()
    {
        while (GameEnd == false) 
        {
            //spawn enemy
            if (EnemyIDsToSummon.Count > 0)
            {
                for(int i = 0; i < EnemyIDsToSummon.Count; i++)
                {
                    EntitySummoner.SummonEnemy(EnemyIDsToSummon.Dequeue());
                }
            }

            //Spawn Tower
            //Move Enemies

            NativeArray<Vector3> NodesToUse = new NativeArray<Vector3>(NodePositionsOne, Allocator.TempJob); 
            NativeArray<int> NodeIndices = new NativeArray<int>(EntitySummoner.enemiesInGame.Count, Allocator.TempJob);
            NativeArray<float> enemySpeeds = new NativeArray<float>(EntitySummoner.enemiesInGame.Count, Allocator.TempJob);
            TransformAccessArray enemyAccess = new TransformAccessArray(EntitySummoner.EnemiesInGameTransform.ToArray(), 2);

            for (int i = 0; i < EntitySummoner.enemiesInGame.Count; i++)
            {
                enemySpeeds[i] = EntitySummoner.enemiesInGame[i].speed;
                NodeIndices[i] = EntitySummoner.enemiesInGame[i].nodeIndex;
            }

            MoveEnemiesJob moveJob = new MoveEnemiesJob
            {
                NodePos = NodesToUse,
                EnemySpeed = enemySpeeds,
                NodeIndex = NodeIndices,
                deltaTime = Time.deltaTime
            };

            JobHandle moveJobHandle = moveJob.Schedule(enemyAccess);
            moveJobHandle.Complete();

            for(int i = 0; i < EntitySummoner.enemiesInGame.Count; i++)
            {
                EntitySummoner.enemiesInGame[i].nodeIndex = NodeIndices[i];
                if (EntitySummoner.enemiesInGame[i].nodeIndex == NodePositionsOne.Length)
                {
                    EnqueueEnemyToRemove(EntitySummoner.enemiesInGame[i]);
                }
            }

            NodesToUse.Dispose();
            NodeIndices.Dispose();
            enemySpeeds.Dispose();
            enemyAccess.Dispose();

            //remove Enemies
            if (EnemyIDsToSummon.Count > 0)
            {
                for (int i = 0; i < EnemiesToRemove.Count; i++)
                {
                    EntitySummoner.RemoveEnemy(EnemiesToRemove.Dequeue());
                }
            }
            
            yield return null;
        }
    }


    public static void enqueueEnemyIDToSummon(int ID)
    {
        EnemyIDsToSummon.Enqueue(ID);
    }

    public static void EnqueueEnemyToRemove(Enemy EnemytoRemove)
    {
        EnemiesToRemove.Enqueue(EnemytoRemove);
    }
}
public struct MoveEnemiesJob : IJobParallelForTransform
{
    [NativeDisableParallelForRestriction]
    public NativeArray<int> NodeIndex;
    [NativeDisableParallelForRestriction]
    public NativeArray<float> EnemySpeed;
    [NativeDisableParallelForRestriction]
    public NativeArray<Vector3> NodePos;
    public float deltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        if (NodeIndex[index] < NodePos.Length)
        {
            Vector3 PositionToMoveTo = NodePos[NodeIndex[index]];
            transform.position = Vector3.MoveTowards(transform.position, PositionToMoveTo, EnemySpeed[index] * deltaTime);

            if (transform.position == PositionToMoveTo)
            {
                NodeIndex[index]++;
            }
        }
    }
}
