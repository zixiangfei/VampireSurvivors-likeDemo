using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamager : MonoBehaviour
{

    [SerializeField]private float damageAmount;
    [SerializeField]private float lifeTime, growSpeed = 5f;
    [SerializeField]private bool shouldKnockback;
    [SerializeField]private bool destroyParent;
    private Vector3 targetSize;

    public bool damageOverTime;
    public float timeBetweenDamage;
    private float damageCounter;

    private List<EnemyController> enemiesInRange = new List<EnemyController>();

    public bool destroyOnImpact;

    // Start is called before the first frame update
    void Start()
    {
        // Destroy(gameObject, lifeTime);

        targetSize = transform.localScale;
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.MoveTowards(transform.localScale, targetSize, growSpeed * Time.deltaTime);

        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            targetSize = Vector3.zero;
            
            if (transform.localScale.x == 0f)
            {
                Destroy(gameObject);

                if (destroyParent)
                {
                    Destroy(transform.parent.gameObject);
                }
            }
        }

        if (damageOverTime == true)
        {
            damageCounter -= Time.deltaTime;

            if (damageCounter <= 0)
            {
                damageCounter = timeBetweenDamage;

                for (int i = 0; i < enemiesInRange.Count; ++i)
                {
                    if (enemiesInRange[i] != null)
                    {
                        enemiesInRange[i].TakeDamage(damageAmount, shouldKnockback);
                    }
                    else
                    {
                        enemiesInRange.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (damageOverTime == false)
        {
            if (collision.tag == "Enemy")
            {
                collision.GetComponent<EnemyController>().TakeDamage(damageAmount, shouldKnockback);

                if (destroyOnImpact == true)
                {
                    Destroy(gameObject);
                }
            }
        }
        else
        {
            if (collision.tag == "Enemy")
            {
                enemiesInRange.Add(collision.GetComponent<EnemyController>());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) 
    {
        if (damageOverTime == true)
        {
            if (collision.tag == "Enemy")
            {
                enemiesInRange.Remove(collision.GetComponent<EnemyController>());
            }
        }
    }

    public void SetDamageAmount(float damageToSet)
    {
        damageAmount = damageToSet;
    }

    public void SetLifeTime(float lifeTimeToSet)
    {
        lifeTime = lifeTimeToSet;
    }
}
