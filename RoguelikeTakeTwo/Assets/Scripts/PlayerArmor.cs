using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerArmor : MonoBehaviour
{
    public Slider armor_bar;

    // Start is called before the first frame update
    void Start()
    {
        // armor
        GameManager.Instance.armorActive = false;
        GameManager.Instance.maxArmorHealth = 100;
        GameManager.Instance.armorHealth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        armor_bar.value = GameManager.Instance.armorHealth / GameManager.Instance.maxArmorHealth;
    }
}
