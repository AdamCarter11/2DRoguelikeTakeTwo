using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XpBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Gradient gradient;
    [SerializeField] Image fill;
    
    public void SetXp(float xp){
        slider.value = xp;

        if(slider.value == slider.maxValue){
            GameManager.Instance.playerXp = 0;
            GameManager.Instance.playerLevel++;
            slider.maxValue = slider.maxValue + 1;
        }

        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
