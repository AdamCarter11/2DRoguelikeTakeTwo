using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    [SerializeField] GameObject panelPopUp;
    
    [SerializeField] Image firstImage, secondImage, thirdImage;
    [SerializeField] Text firstDesc, secondDesc, thirdDesc;

    [SerializeField] UpgradeData[] allUpgrades;
    private List<UpgradeData> possibleUpgrades = new List<UpgradeData>();
    private List<UpgradeData> lockedUpgrades = new List<UpgradeData>();

    List<int> usedVals = new List<int>();

    private void Start() {
        //creates initial list of all possible upgrades (that don't require prereqs)
        foreach (var upgrade in allUpgrades)
        {
            if(upgrade.prereqs == null){
                possibleUpgrades.Add(upgrade);
                //print(upgrade.name);
            }
        }
    }

    public void SetXp(float xp){
        slider.value = xp;

        if(slider.value == slider.maxValue){
            GameManager.Instance.playerXp = 0;
            GameManager.Instance.playerLevel++;
            slider.maxValue = slider.maxValue + 1;

            //pauses game, to unpause: Time.timeScale = 1;
            Time.timeScale = 0;
            
            DisplayUpgrades();
            
        }

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    private void DisplayUpgrades(){
        //generates random numbers that don't overlap
        int val1 = UniqueRandomVals(0,possibleUpgrades.Count);
        int val2 = UniqueRandomVals(0,possibleUpgrades.Count);
        int val3 = UniqueRandomVals(0,possibleUpgrades.Count);
        //sprites
        firstImage.sprite = possibleUpgrades[val1].upgradeSprite;
        secondImage.sprite = possibleUpgrades[val2].upgradeSprite;
        thirdImage.sprite = possibleUpgrades[val3].upgradeSprite;
        //descs
        firstDesc.text = possibleUpgrades[val1].upgradeDesc;
        secondDesc.text = possibleUpgrades[val2].upgradeDesc;
        thirdDesc.text = possibleUpgrades[val3].upgradeDesc;
        //displays upgrade panel
        panelPopUp.SetActive(true);
        //clears unique random values list for next iteration
        usedVals.Clear();
    }

    private int UniqueRandomVals(int min, int max){
        int val = Random.Range(min,max);
        while(usedVals.Contains(val)){
            val = Random.Range(min,max);
        }
        usedVals.Add(val);
        return val;
    }
}
