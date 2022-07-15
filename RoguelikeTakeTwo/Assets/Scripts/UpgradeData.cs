using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UpgradeData : ScriptableObject
{
    public string nameOfUpgrade;
    [TextArea(2,5)]
    public string upgradeDesc;
    public Sprite upgradeSprite;
    public UpgradeData[] unlockedUpgrades;
    public UpgradeData prereqs;
}
