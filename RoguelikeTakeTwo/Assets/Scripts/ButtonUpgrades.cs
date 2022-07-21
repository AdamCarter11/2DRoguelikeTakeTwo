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
        print(abilityName);

        if(abilityName == "Circle"){
            GameManager.Instance.flatSpeedModifier+=5;
        }

        panelPopUp.SetActive(false);
        Time.timeScale = 1;
    } 
}
