using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void Start() {
        Destroy(this.gameObject, 10f);
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(!other.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
        
        //damage enemy
    }
}
