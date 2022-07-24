using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage_falloff;
    private void Start() {
        damage_falloff = 0f;
        Destroy(this.gameObject, 10f);
    }

    private void Update()
    {
        damage_falloff -= 1f * Time.deltaTime;
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(!other.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }
        
        //damage enemy
    }
}
