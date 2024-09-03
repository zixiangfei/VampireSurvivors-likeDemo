using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExpPickup : MonoBehaviour
{

    [SerializeField]private int expValue;
    private bool movingToPlayer;
    [SerializeField]private float moveSpeed;
    
    [SerializeField]private float timeBetweenChecks = .2f;
    private float checkCounter;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerHealthController.instance.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (movingToPlayer == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
        }
        else
        {
            checkCounter -= Time.deltaTime;

            if (checkCounter <= 0)
            {
                checkCounter = timeBetweenChecks;

                if (Vector3.Distance(transform.position, player.transform.position) < player.GetPickupRange())
                {
                    movingToPlayer = true;
                    moveSpeed += player.GetMoveSpeed();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ExperienceLevelController.instance.GetExp(expValue);

            Destroy(gameObject);
        }
    }

    public void SetExpValue(int expToGive)
    {
        expValue = expToGive;
    }
}
