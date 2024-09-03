using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Weapon : MonoBehaviour
{

    [SerializeField]protected List<WeaponStats> stats;
    [SerializeField]protected int weaponLevel;

    [HideInInspector]
    public bool statsUpdated;

    public Sprite icon;

    public void LevelUp()
    {
        if (weaponLevel < stats.Count - 1)
        {
            weaponLevel++;

            statsUpdated = true;

            if (weaponLevel >= stats.Count - 1)
            {
                PlayerController.instance.fullyLevelWeapons.Add(this);
                PlayerController.instance.assignedWeapons.Remove(this);
            }
        }
    }

    public List<WeaponStats> GetStats()
    {
        return stats;
    }

    public int GetWeaponLevel()
    {
        return weaponLevel;
    }
}

[System.Serializable]
public class WeaponStats
{
    public float speed, damage, range, timeBetweenAttacks, amount, duration;
    public string upgradeText;
}
