using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Text playerSpeedText;
    [SerializeField] private Text playerAmmoText;
    [SerializeField] private GameObject lootCratePanel;
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] UpgradeData[] allLootUpgrades;
    [HideInInspector] public static List<UpgradeData> possibleUpgrades = new List<UpgradeData>();
    List<int> usedVals = new List<int>();
    private Rigidbody2D rb;
    private Vector2 moveInput;

    private Vector2 mousePos;
    public Weapon weapon;

    private bool canShoot = true;
    //private float shotDelay = .3f;

    private bool canTakeDamage = true;
    private bool speedRepeat = true;
    private float onDamageSpeed = 0;
    private float trapSlowDown = 0;
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
        playerAmmoText.text = "Ammo: " + ammo;

        // loot crates
        foreach (var upgrade in allLootUpgrades)
        {
            if (upgrade.prereqs == null)
            {
                possibleUpgrades.Add(upgrade);
                //print(upgrade.name);
            }
        }
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
            playerAmmoText.text = "Ammo: " + ammo;
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
        playerAmmoText.text = "Ammo: " + ammo;
    }
    //used for shot delay
    IEnumerator ShootDelay(float delay){
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }

    private void FixedUpdate() {
        //movement
        totalMoveSpeed = moveSpeed + GameManager.Instance.flatSpeedModifier + GameManager.Instance.speedChainCount + onDamageSpeed + trapSlowDown;
        rb.velocity = moveInput * (totalMoveSpeed);
        playerSpeedText.text = "Speed: " + totalMoveSpeed;

        //aiming
        Vector2 aimDir = mousePos - rb.position;
        float aimAingle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg-90f;
        rb.rotation = aimAingle;
    }
    
    //Currently being used to take damage when colliding with enemies
    //setup this way so we can maybe have an ugprade for invincibility time
    private void OnCollisionStay2D(Collision2D other) {
        
        if(other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("explosionEnemy") && canTakeDamage){
            damageEffects(10);
        }
        if(other.gameObject.CompareTag("FlyingEnemy") && canTakeDamage){
            damageEffects(15);
        }
    }
    private void damageEffects(float damageToTake){
        if (!GameManager.Instance.armorActive)
            {
                GameManager.Instance.playerHealth -= damageToTake;
            }
            else
            {
                GameManager.Instance.playerHealth -= (damageToTake - (GameManager.Instance.armorHealth / 10));
                GameManager.Instance.armorHealth -= damageToTake;
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
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag("damageTrap")){
            GameManager.Instance.playerHealth -= 5;
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("slowTrap")){
            StartCoroutine(ActivteSlowTrap());
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("explosion") && canTakeDamage){
            damageEffects(20);
        }
        if(other.gameObject.CompareTag("spawnerTrap")){
            Instantiate(enemyToSpawn, new Vector2(transform.position.x - 5f, transform.position.y), Quaternion.identity);
            Instantiate(enemyToSpawn, new Vector2(transform.position.x, transform.position.y + 5), Quaternion.identity);
            Instantiate(enemyToSpawn, new Vector2(transform.position.x + 5f, transform.position.y), Quaternion.identity);
            Instantiate(enemyToSpawn, new Vector2(transform.position.x, transform.position.y - 5f), Quaternion.identity);
            Destroy(other.gameObject);
        }

        //What happens when you collect the loot crate
        if(other.gameObject.CompareTag("LootCrate")){
            Time.timeScale = 0;
            lootCratePanel.SetActive(true);
            DisplayUpgrade();
        }
    }

    // -------------------------------------------------
    // Loot Crates
    private void DisplayUpgrade()
    {
        //generates random numbers that don't overlap
        int val = UniqueRandomVals(0, possibleUpgrades.Count);
        //sprites
        lootCratePanel.transform.Find("UpgradeIcon").GetComponent<Image>().sprite = possibleUpgrades[val].upgradeSprite;
        //descs
        lootCratePanel.transform.Find("UpgradeDesc").GetComponent<Text>().text = possibleUpgrades[val].upgradeDesc;
        //clears unique random values list for next iteration
        usedVals.Clear();
    }

    private int UniqueRandomVals(int min, int max)
    {
        int val = Random.Range(min, max);
        while (usedVals.Contains(val))
        {
            val = Random.Range(min, max);
        }
        usedVals.Add(val);
        return val;
    }

    public void ApplyUpgrade()
    {
        string abilityName = lootCratePanel.transform.Find("UpgradeIcon").GetComponent<Image>().sprite.name;
        // add upgrades based on name

        lootCratePanel.SetActive(false);
        Time.timeScale = 1;
    }
    // -------------------------------------------------

    IEnumerator ActivteSlowTrap(){
        trapSlowDown = -3;
        yield return new WaitForSeconds(5f);
        trapSlowDown = 0;
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
