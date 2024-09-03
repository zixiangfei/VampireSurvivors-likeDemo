using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWeapon : Weapon
{
    [SerializeField]private float rotateSpeed;
    [SerializeField]private Transform holder, fireballToSpawn;
    [SerializeField]private float timeBetweenSpawn;
    private float spawnCounter;
    [SerializeField]private EnemyDamager damager;

    // Start is called before the first frame update
    void Start()
    {
        // spawnCounter = timeBetweenSpawn;
        
        SetStats();

        // UIController.instance.levelUpSelections[0].UpdateButtonDisplay(this);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
        transform.rotation = Quaternion.Euler(0f, 0f, holder.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime * stats[weaponLevel].speed));
        
        spawnCounter -= Time.deltaTime;
        if (spawnCounter <= 0)
        {
            spawnCounter = timeBetweenSpawn;

            //Instantiate(fireballToSpawn, fireballToSpawn.position, fireballToSpawn.rotation, holder).gameObject.SetActive(true);

            for (int i = 0; i < stats[weaponLevel].amount; ++i)
            {
                float rot = (360f / stats[weaponLevel].amount) * i;
                Instantiate(fireballToSpawn, fireballToSpawn.position, Quaternion.Euler(0f, 0f, rot), holder).gameObject.SetActive(true);
            }

            SFXManager.instance.PlaySFX(8);
        }

        if (statsUpdated == true)
        {
            SetStats();
            statsUpdated = false;
        }
    }

    public void SetStats()
    {
        damager.SetDamageAmount(stats[weaponLevel].damage);

        transform.localScale = Vector3.one * stats[weaponLevel].range;

        timeBetweenSpawn = stats[weaponLevel].timeBetweenAttacks;

        damager.SetLifeTime(stats[weaponLevel].duration);

        spawnCounter = 0f;
    }
}
