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
            GameManager.Instance.flatDamage += 1;
        }
        else if (abilityName == "LowerHealth")
        {
            GameManager.Instance.lowerHealthDamage = true;
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
