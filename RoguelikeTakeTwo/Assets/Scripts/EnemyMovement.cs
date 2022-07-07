using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed;
    [SerializeField] int health;
    private float startingSpeed;
    private Rigidbody2D rb;
    private Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startingSpeed = speed;
        if(target == null){
            target = GameObject.Find("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
        dir.Normalize();
        movement = dir;
    }
    private void FixedUpdate() {
        MoveEnemy(movement);
    }
    void MoveEnemy(Vector2 direction){
        rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("Bullet")){
            health -= 1;
            Destroy(other.gameObject);
            if(health <= 0){
                GameManager.Instance.playerXp++;
                Destroy(gameObject);
            }
        }
    }

    //when it hits the player, it sets its speed to half for a second
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")){
            StartCoroutine(SlowDown());
        }
    }
    IEnumerator SlowDown(){
        speed = startingSpeed;
        speed = speed / 2;
        yield return new WaitForSeconds(1f);
        speed = startingSpeed;
    }
}
