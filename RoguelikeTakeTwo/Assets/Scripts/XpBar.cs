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
    
    public void SetXp(float xp){
        slider.value = xp;

        if(slider.value == slider.maxValue){
            GameManager.Instance.playerXp = 0;
            GameManager.Instance.playerLevel++;
            slider.maxValue = slider.maxValue + 1;

            //pauses game, to unpause: Time.timeScale = 1;
            Time.timeScale = 0;
            panelPopUp.SetActive(true);
        }

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
