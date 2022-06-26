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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        MovementInput();
        AimingFiring();
        
    }

    private void MovementInput(){
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize();
    }
    private void AimingFiring(){
        //firing
        if(Input.GetMouseButtonDown(0)){
            weapon.Fire();
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate() {
        //movement
        rb.velocity = moveInput * moveSpeed;

        //aiming
        Vector2 aimDir = mousePos - rb.position;
        float aimAingle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg-90f;
        rb.rotation = aimAingle;
    }
}
