using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
[SerializeField]private int coinAmount = 1;
    private bool movingToPlayer;
    [SerializeField]private float moveSpeed;
    
    [SerializeField]private float timeBetweenChecks = .2f;
    private float checkCounter;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance;
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
            CoinController.instance.AddCoins(coinAmount);

            Destroy(gameObject);
        }
    }

    public void SetCoinAmount(int coinToGive)
    {
        coinAmount = coinToGive;
    }
}
