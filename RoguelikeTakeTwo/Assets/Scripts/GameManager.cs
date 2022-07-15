using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; set;}
    public float playerXp {get; set;}
    public int playerLevel {get; set;}

    //upgrades
    public float flatSpeedModifier {get; set;}
    public float dynamicSpeedModifier {get; set;}

    private void Awake() {
        Instance = this;
    }

}
