using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; set;}
    public float playerXp {get; set;}
    public int playerLevel {get; set;}

    private void Awake() {
        Instance = this;
    }

}
