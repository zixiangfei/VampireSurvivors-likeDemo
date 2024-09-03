using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class ExperienceLevelController : MonoBehaviour
{

    public static ExperienceLevelController instance;

    public int currentExperience;

    [SerializeField]private ExpPickup pickup;
    [SerializeField]private List<int> expLevels;
    [SerializeField]private int currentLevel = 1, levelCount = 100;

    public List<Weapon> weaponsToUpgrade;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        while(expLevels.Count < levelCount)
        {
            expLevels.Add(Mathf.CeilToInt(expLevels[expLevels.Count - 1] * 1.1f));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetExp(int amountToGet)
    {
        currentExperience += amountToGet;
        
        if (currentExperience >= expLevels[currentLevel])
        {
            LevelUp();
        }

        UIController.instance.UpdateExperience(currentExperience, expLevels[currentLevel], currentLevel);

        SFXManager.instance.PlaySFXPitched(2);
    }

    public void SpawnExp(Vector3 position, int expValue)
    {
        Instantiate(pickup, position, Quaternion.identity).SetExpValue(expValue);
    }

    private void LevelUp()
    {
        currentExperience -= expLevels[currentLevel];
        currentLevel++;

        if (currentLevel >= expLevels.Count)
        {
            currentLevel = expLevels.Count - 1;
        }

        // PlayerController.instance.GetActiveWeapon().LevelUp();
        UIController.instance.levelUpPanel.SetActive(true);

        Time.timeScale = 0;

        //UIController.instance.levelUpSelections[1].UpdateButtonDisplay(PlayerController.instance.GetActiveWeapon());

        // UIController.instance.levelUpSelections[0].UpdateButtonDisplay(PlayerController.instance.assignedWeapons[0]);
        // UIController.instance.levelUpSelections[1].UpdateButtonDisplay(PlayerController.instance.unassignedWeapons[0]);
        // UIController.instance.levelUpSelections[2].UpdateButtonDisplay(PlayerController.instance.unassignedWeapons[1]);
        
        weaponsToUpgrade.Clear();

        List<Weapon> availableWeapons = new List<Weapon>();
        availableWeapons.AddRange(PlayerController.instance.assignedWeapons);

        if (availableWeapons.Count > 0)
        {
            int selected = Random.Range(0, availableWeapons.Count);
            weaponsToUpgrade.Add(availableWeapons[selected]);
            availableWeapons.RemoveAt(selected);
        }
        
        if (PlayerController.instance.assignedWeapons.Count + PlayerController.instance.fullyLevelWeapons.Count < PlayerController.instance.maxWeapons)
        {
            availableWeapons.AddRange(PlayerController.instance.unassignedWeapons);
        }

        for (int i = weaponsToUpgrade.Count; i < 3; ++i)
        {
            if (availableWeapons.Count > 0)
            {
                int selected = Random.Range(0, availableWeapons.Count);
                weaponsToUpgrade.Add(availableWeapons[selected]);
                availableWeapons.RemoveAt(selected);
            }
        }

        for (int i = 0; i < weaponsToUpgrade.Count; ++i)
        {
            UIController.instance.levelUpSelections[i].UpdateButtonDisplay(weaponsToUpgrade[i]);
        }

        for (int i = 0; i < UIController.instance.levelUpSelections.Length; ++i)
        {
            if (i < weaponsToUpgrade.Count)
            {
                UIController.instance.levelUpSelections[i].gameObject.SetActive(true);
            }
            else
            {
                UIController.instance.levelUpSelections[i].gameObject.SetActive(false);
            }
        }

        PlayerStatController.instance.UpdateDisplay();
    }
}
