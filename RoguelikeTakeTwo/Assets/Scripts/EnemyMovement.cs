using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float speed;
    [SerializeField] float health;
    private float startingSpeed;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool canMove = true;
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
        if(canMove){
            rb.MovePosition((Vector2)transform.position + (direction * speed * Time.deltaTime));
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("explosion")){
            health -= 1;
            EnemyDeath();
        }

        if(other.gameObject.CompareTag("Bullet")){
            if (!GameManager.Instance.lowerHealthDamage)
            {
                health -= GameManager.Instance.flatDamage + ((0.25f * GameManager.Instance.bonusDamage) * GameManager.Instance.flatDamage)
                    + other.GetComponent<Bullet>().damage_falloff;
            }
            else
            {
                health -= GameManager.Instance.flatDamage + ((0.25f * GameManager.Instance.bonusDamage) * GameManager.Instance.flatDamage)
                    + other.GetComponent<Bullet>().damage_falloff 
                    + ((1 - (GameManager.Instance.playerHealth / GameManager.Instance.maxHealth)) * GameManager.Instance.flatDamage);
            }
            Destroy(other.gameObject);
            EnemyDeath();
        }
    }

    private void EnemyDeath(){
        if(health <= 0){
                
                
                if(gameObject.tag == "explosionEnemy"){
                    transform.GetChild(0).gameObject.SetActive(true);
                    print(transform.childCount);
                    canMove = false;
                    StartCoroutine(FadeTo(0.0f,.5f));

                    if(GameManager.Instance.killChainBonus)
                    {
                        GameManager.Instance.killChainCount++;
                    }
                    if(GameManager.Instance.speedChainBonus){
                        GameManager.Instance.speedChainCount += .5f;
                        Player.speedKillTimer = 5;
                    }
                    GameManager.Instance.playerXp++;
                } 
                else{
                    if(GameManager.Instance.killChainBonus)
                    {
                        GameManager.Instance.killChainCount++;
                    }
                    if(GameManager.Instance.speedChainBonus){
                        GameManager.Instance.speedChainCount += .5f;
                        Player.speedKillTimer = 5;
                    }
                    GameManager.Instance.playerXp++;
                    Destroy(gameObject);
                }
                
        }
    }

    //Used to fade the exploding enemy
    IEnumerator FadeTo(float endVal, float dur){
        Color alpha = transform.GetComponent<SpriteRenderer>().color;
        while(alpha.a > endVal){
            alpha.a -= dur * Time.deltaTime;
            transform.GetComponent<SpriteRenderer>().color = alpha;
            yield return null;
        }
        Destroy(gameObject);
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
