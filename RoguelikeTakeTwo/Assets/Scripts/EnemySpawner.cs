using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private float time = 1.5f;
    [SerializeField] private GameObject[] enemies;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] traps;
    private float scaleTime = 0;
    private Vector2 spawnPos;

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    IEnumerator SpawnEnemy(){
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
        int enemyOrTraps = Random.Range(0,100);
        if(enemyOrTraps > 40){
            Instantiate(enemies[Random.Range(0,enemies.Length)], spawnPos, Quaternion.identity);
        }
        else{
            Instantiate(traps[Random.Range(0,traps.Length)], spawnPos, Quaternion.identity);
        }
        print("Enemy spawn time: " + (time-scaleTime));
        yield return new WaitForSeconds((time-scaleTime));
        if(scaleTime < 1f){
            scaleTime = Mathf.Round(Time.time / 30) / 10;
        }
        else{
            scaleTime = 1f;
        }

        StartCoroutine(SpawnEnemy());
    }
}
