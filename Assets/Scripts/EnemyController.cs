using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]private Rigidbody2D theRB;
    [SerializeField]private float moveSpeed;
    [SerializeField]private float damage;
    [SerializeField]private float hitWaitTime = 1f;
    [SerializeField]private float health = 5f;
    [SerializeField]private float knockBackTime = .5f;
    [SerializeField]private int expToGive =1;
    private float knockBackCounter;
    private float hitCounter;
    private Transform target;

    public int coinValue = 1;
    public float coinDropRate = .5f;
    // Start is called before the first frame update
    void Start()
    {
        //target = FindObjectOfType<PlayerController>().transform;
        target = PlayerHealthController.instance.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerController.instance.gameObject.activeSelf == true)
        {
            if (knockBackCounter > 0)
            {
                knockBackCounter -= Time.deltaTime;

                if (moveSpeed > 0)
                {
                    moveSpeed = -moveSpeed * 2f;
                }

                if(knockBackCounter <= 0)
                {
                    moveSpeed = -moveSpeed * .5f;
                }
            }

            theRB.velocity = (target.position - transform.position).normalized * moveSpeed;

            if (hitCounter > 0)
            {
                hitCounter -= Time.deltaTime;
            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (collision.gameObject.tag == "Player" && hitCounter <= 0f)
        {
            PlayerHealthController.instance.TakeDamage(damage);
            
            hitCounter = hitWaitTime;
        }
    }

    public void TakeDamage(float damageToTake)
    {
        health -= damageToTake;
        if (health <= 0f)
        {
            ExperienceLevelController.instance.SpawnExp(transform.position, expToGive);

            if (Random.value <= coinDropRate)
            {
                CoinController.instance.DropCoin(transform.position, coinValue);
            }

            SFXManager.instance.PlaySFXPitched(0);

            Destroy(gameObject);
        }
        else
        {
            SFXManager.instance.PlaySFXPitched(1);
        }

        DamageNumberController.instance.SpawnDamage(damageToTake, transform.position);
    }

    public void TakeDamage(float damageToTake, bool shouldKnockback)
    {
        TakeDamage(damageToTake);

        if (shouldKnockback)
        {
            knockBackCounter = knockBackTime;
        }
    }
}
