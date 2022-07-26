using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class ButtonUpgrades : MonoBehaviour
{
    [SerializeField] GameObject panelPopUp;
    Button bt;
    private void Start() {
        bt = this.GetComponent<Button>();
        bt.onClick.AddListener(ApplyUpgrade);
    }

    public void ApplyUpgrade(){
        string abilityName = this.GetComponent<Image>().sprite.name;
        //print(abilityName);
        
        if(abilityName == "FlatSpeed"){
            GameManager.Instance.flatSpeedModifier+=5;
        }
        else if (abilityName == "SpeedChain")
        {
            GameManager.Instance.speedChainBonus = true;
        }
        else if (abilityName == "reloadSpeedSpeed")
        {
            GameManager.Instance.reloadSpeedSpeedBonus = true;
        }
        else if (abilityName == "FlatShotSpeed")
        {
            GameManager.Instance.shotDelay = .2f;
        }
        else if (abilityName == "ShotSpeedSpeed")
        {
            GameManager.Instance.shotSpeedSpeedBonus = true;
        }
        else if (abilityName == "DamagedShotSpeed")
        {
            GameManager.Instance.onDamageShotSpeedBonus = true;
        }
        else if (abilityName == "FlatReload")
        {
            GameManager.Instance.reloadTime = 1f;
        }
        else if (abilityName == "DamagedSpeed")
        {
            GameManager.Instance.onDamageSpeedBonus = true;
        }
        else if (abilityName == "FlatHealth")
        {
            GameManager.Instance.maxHealth += 25;
        }
        else if (abilityName == "KillChain")
        {
            GameManager.Instance.killChainBonus = true;
        }
        else if (abilityName == "RegenSpeed")
        {
            GameManager.Instance.regenSpeed += 1;
        }
        else if (abilityName == "FlatDamage")
        {
            GameManager.Instance.bonusDamage += 1;
        }
        else if (abilityName == "LowerHealth")
        {
            GameManager.Instance.lowerHealthDamage = true;
        }
        else if (abilityName == "ArmorUp")
        {
            GameManager.Instance.armorActive = true;
            GameManager.Instance.armorHealth = GameManager.Instance.maxArmorHealth;
        }

        panelPopUp.SetActive(false);
        Time.timeScale = 1;

        
        //removes upgrade from list assuming possible upgrades is static
        foreach (var upgrade in XpBar.possibleUpgrades){
            if(upgrade.upgradeSprite.name == abilityName){
                XpBar.possibleUpgrades.Remove(upgrade);
                foreach(var upgradeInLocked in upgrade.unlockedUpgrades){
                    XpBar.possibleUpgrades.Add(upgradeInLocked);
                }
                break;
            }
        }
        
    } 
}
