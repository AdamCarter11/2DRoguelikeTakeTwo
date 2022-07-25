using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private Vector2 mousePos;
    public Weapon weapon;

    private bool canShoot = true;
    //private float shotDelay = .3f;

    private bool canTakeDamage = true;
    private bool speedRepeat = true;
    private float onDamageSpeed = 0;

    [HideInInspector] public static float speedKillTimer = 0;

    //private float xp;
    [SerializeField] XpBar xpBar;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //xp = 0; //load saved xp
        GameManager.Instance.playerXp = 0;
        xpBar.SetXp(GameManager.Instance.playerXp);

        GameManager.Instance.flatSpeedModifier = 0;
        GameManager.Instance.dynamicSpeedModifier = 1;

        // damage
        GameManager.Instance.flatDamage = 1;
        GameManager.Instance.lowerHealthDamage = false;
        GameManager.Instance.shotDelay = .5f;
    }

    void Update()
    {
        MovementInput();
        AimingFiring();
        XpStuff();
        
        if(GameManager.Instance.speedChainBonus && speedRepeat){
            InvokeRepeating("SpeedChainTimer", 1.0f, 1.0f);
            speedRepeat = false;
        }
    }

    private void SpeedChainTimer(){
        if(speedKillTimer > 0){
            speedKillTimer--;
            print(speedKillTimer);
        }
        if(speedKillTimer == 0){
            GameManager.Instance.speedChainCount = 0;
            print("KillStreak Lost");
            speedRepeat = true;
            CancelInvoke("SpeedChainTimer");
        }
    }

    void XpStuff(){
        //for testing
        if(Input.GetKeyDown(KeyCode.Space)){
            GameManager.Instance.playerXp++;
            xpBar.SetXp(GameManager.Instance.playerXp);
        }

        xpBar.SetXp(GameManager.Instance.playerXp);
    }

    private void MovementInput(){
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();
    }
    private void AimingFiring(){
        //firing
        if(Input.GetMouseButton(0) && canShoot){
            weapon.Fire();
            canShoot = false;
            StartCoroutine(ShootDelay(GameManager.Instance.shotDelay));
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    //used for shot delay
    IEnumerator ShootDelay(float delay){
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    private void FixedUpdate() {
        //movement
        float totalMoveSpeed = moveSpeed + GameManager.Instance.flatSpeedModifier + GameManager.Instance.speedChainCount + onDamageSpeed;
        rb.velocity = moveInput * (totalMoveSpeed) * GameManager.Instance.dynamicSpeedModifier;

        //aiming
        Vector2 aimDir = mousePos - rb.position;
        float aimAingle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg-90f;
        rb.rotation = aimAingle;
    }
    
    //Currently being used to take damage when colliding with enemies
    //setup this way so we can maybe have an ugprade for invincibility time
    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy") && canTakeDamage){
            GameManager.Instance.playerHealth -= 10;
            if(GameManager.Instance.onDamageSpeedBonus){
                StartCoroutine(DamagedBonusSpeed());
            }
            if(GameManager.Instance.onDamageShotSpeedBonus){
                StartCoroutine(DamagedShotSpeed());
            }
            if(GameManager.Instance.playerHealth <= 0){
                print("GAMEOVER");
            }
            StartCoroutine(TakeDamageTimer());
        }
    }
    IEnumerator DamagedBonusSpeed(){
        onDamageSpeed = 5;
        yield return new WaitForSeconds(3f);
        onDamageSpeed = 0;
    }
    IEnumerator DamagedShotSpeed(){
        float tempHolder = GameManager.Instance.shotDelay;
        GameManager.Instance.shotDelay -= .1f;
        yield return new WaitForSeconds(3f);
        GameManager.Instance.shotDelay = tempHolder;
    }
    IEnumerator TakeDamageTimer(){
        canTakeDamage = false;
        yield return new WaitForSeconds(1f);
        canTakeDamage = true;
    }
}
