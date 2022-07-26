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
    private int ammo;
    private float tempReload;

    private float totalMoveSpeed;

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

        // damage
        GameManager.Instance.flatDamage = 1; // weapon damage
        GameManager.Instance.bonusDamage = 0;
        GameManager.Instance.lowerHealthDamage = false;
        GameManager.Instance.shotDelay = .5f;


        //ammo
        GameManager.Instance.reloadTime = 4f;
        GameManager.Instance.playerAmmo = 6;
        ammo = GameManager.Instance.playerAmmo;
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
        }
        if(speedKillTimer == 0){
            GameManager.Instance.speedChainCount = 0;
            //print("KillStreak Lost");
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
        //reload time
        tempReload = GameManager.Instance.reloadTime;
        if(GameManager.Instance.reloadSpeedSpeedBonus){
            tempReload = tempReload / (moveSpeed / 4);
        }

        //firing
        if(Input.GetMouseButton(0) && canShoot && ammo > 0){
            weapon.Fire();
            canShoot = false;
            float tempShotSpeed = GameManager.Instance.shotDelay;
            if(GameManager.Instance.shotSpeedSpeedBonus){
                tempShotSpeed = tempShotSpeed / (totalMoveSpeed / 2);
            }
            StartCoroutine(ShootDelay(tempShotSpeed));
            ammo--;
            if(ammo <= 0){
                StartCoroutine(Reload());
            }
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    //reloading
    IEnumerator Reload(){
        yield return new WaitForSeconds(tempReload);
        ammo = GameManager.Instance.playerAmmo;
    }
    //used for shot delay
    IEnumerator ShootDelay(float delay){
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    private void FixedUpdate() {
        //movement
        totalMoveSpeed = moveSpeed + GameManager.Instance.flatSpeedModifier + GameManager.Instance.speedChainCount + onDamageSpeed;
        rb.velocity = moveInput * (totalMoveSpeed);

        //aiming
        Vector2 aimDir = mousePos - rb.position;
        float aimAingle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg-90f;
        rb.rotation = aimAingle;
    }
    
    //Currently being used to take damage when colliding with enemies
    //setup this way so we can maybe have an ugprade for invincibility time
    private void OnCollisionStay2D(Collision2D other) {
        if(other.gameObject.CompareTag("Enemy") && canTakeDamage){
            if (!GameManager.Instance.armorActive)
            {
                GameManager.Instance.playerHealth -= 10;
            }
            else
            {
                GameManager.Instance.playerHealth -= (10 - (GameManager.Instance.armorHealth / 10));
                GameManager.Instance.armorHealth -= 10;
                if (GameManager.Instance.armorHealth <= 0)
                {
                    GameManager.Instance.armorActive = false;
                }
            }
            
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
