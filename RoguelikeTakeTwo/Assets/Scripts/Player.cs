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
    private float shotDelay = .3f;

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
    }

    void Update()
    {
        MovementInput();
        AimingFiring();
        XpStuff();
        
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
            StartCoroutine(ShootDelay(shotDelay));
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
        rb.velocity = moveInput * (moveSpeed + GameManager.Instance.flatSpeedModifier) * GameManager.Instance.dynamicSpeedModifier;

        //aiming
        Vector2 aimDir = mousePos - rb.position;
        float aimAingle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg-90f;
        rb.rotation = aimAingle;
    }
}
