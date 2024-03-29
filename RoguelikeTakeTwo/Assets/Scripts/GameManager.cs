using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; set;}
    public float playerXp {get; set;}
    public int playerLevel {get; set;}

    // health
    public float playerHealth { get; set;}

    public float maxHealth { get; set; }

    public bool killChainBonus { get; set; }

    public int killChainCount { get; set; }

    public float regenSpeed { get; set; }

    // armor

    public bool armorActive { get; set; }

    public float armorHealth { get; set; }

    public float maxArmorHealth { get; set; }

    // damage
    public float flatDamage { get; set; }

    public float bonusDamage { get; set; }
    
    public bool lowerHealthDamage { get; set; }

    //upgrades
    public float flatSpeedModifier {get; set;}

    public bool speedChainBonus { get; set; }
    public float speedChainCount { get; set; }
    public bool onDamageSpeedBonus { get; set; }

    public float shotDelay { get; set; }
    public bool onDamageShotSpeedBonus { get; set; }
    public bool shotSpeedSpeedBonus { get; set; }

    public int playerAmmo { get; set; }
    public float reloadTime { get; set; }
    public bool reloadSpeedSpeedBonus { get; set; }

    public float projSpeed { get; set; }

    private void Awake() {
        Instance = this;
    }

}
