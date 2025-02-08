using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{

    public GameObject[] normEnemies;
    public GameObject[] eliteEnemies;
    public GameObject jumpEnemy;
    public GameObject[] towerEnemies;

    private int randomDegree = 0;
    private float yCoord = 0;
    private float xCoord = 0;
    public float radius = 1;

    // Update is called once per frame
    private void Start()
    {
        //randomMove();
        //transform.position=new Vector3(radius,0,0);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            randomMove();
            Instantiate(jumpEnemy,transform.position,Quaternion.identity);
        }
        
    }

    void randomMove()
    {
        randomDegree = Random.Range(0,360);
        yCoord = Mathf.Sin(randomDegree)*radius; 
        xCoord = Mathf.Cos(randomDegree)*radius;
        transform.position = new Vector3(xCoord, 0, yCoord);
    }
}
