using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public Slider health_bar;


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.maxHealth = 100;
        GameManager.Instance.playerHealth = GameManager.Instance.maxHealth;
        GameManager.Instance.killChainBonus = false;
        GameManager.Instance.killChainCount = 0;
        GameManager.Instance.regenSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        //for testing
        if (Input.GetKeyDown(KeyCode.Y))
        {
            GameManager.Instance.playerHealth -= 25;
        }

        // passive regen
        if (GameManager.Instance.playerHealth < GameManager.Instance.maxHealth)
        {
            GameManager.Instance.playerHealth += GameManager.Instance.regenSpeed * 1 * Time.deltaTime;
        }

        // kill chain bonus
        if (GameManager.Instance.killChainBonus)
        {
            if (GameManager.Instance.killChainCount >= 10)
            {
                GameManager.Instance.killChainCount = 0;
                GameManager.Instance.playerHealth += (GameManager.Instance.maxHealth * 0.25f);
            }
        }

        health_bar.value = GameManager.Instance.playerHealth / GameManager.Instance.maxHealth;
    }
}
