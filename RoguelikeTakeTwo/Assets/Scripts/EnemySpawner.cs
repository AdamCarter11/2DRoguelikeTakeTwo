using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float spawnRadius = 7, time = 1.5f;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject player;
    private Vector2 spawnPos;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy(){
        //Vector2 spawnPos = player.transform.position;
        //spawnPos += Random.insideUnitCircle.normalized * spawnRadius;
        int randomSide = Random.Range(0,4);
        float randomPos = Random.Range(0f,1f);
        if(randomSide == 0){
            //right
            spawnPos = Camera.main.ViewportToWorldPoint(new Vector2(1.1f, randomPos));
        }
        else if(randomSide == 1){
            //left
            spawnPos = Camera.main.ViewportToWorldPoint(new Vector2(-.1f, randomPos));
        }
        else if(randomSide == 2){
            //top
            spawnPos = Camera.main.ViewportToWorldPoint(new Vector2(randomPos, 1.1f));
        }
        else if(randomSide == 3){
            //bottom
            spawnPos = Camera.main.ViewportToWorldPoint(new Vector2(randomPos, -.1f));
        }
        
        Instantiate(enemies[Random.Range(0,enemies.Length)], spawnPos, Quaternion.identity);
        yield return new WaitForSeconds(time);
        StartCoroutine(SpawnEnemy());
    }
}
