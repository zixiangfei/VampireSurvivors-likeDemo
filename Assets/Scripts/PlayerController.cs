using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public static PlayerController instance;

    [SerializeField]public float moveSpeed;
    [SerializeField]private Animator anim;
    [SerializeField]public float pickupRange = 1.5f;
    //[SerializeField]private Weapon activeWeapon;

    public int maxWeapons = 3;

    [HideInInspector]
    public List<Weapon> fullyLevelWeapons = new List<Weapon>();

    public List<Weapon> unassignedWeapons, assignedWeapons;

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
        if (assignedWeapons.Count == 0)
        {
            int id = Random.Range(0, unassignedWeapons.Count);
            AddWeapon(id);
        }
       
        moveSpeed = PlayerStatController.instance.moveSpeed[0].value;
        pickupRange = PlayerStatController.instance.pickupRange[0].value;
        maxWeapons = Mathf.RoundToInt(PlayerStatController.instance.maxWeapons[0].value);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveInput = new Vector3(0f, 0f, 0f);
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
        
        moveInput.Normalize();

        transform.position += moveInput * moveSpeed * Time.deltaTime;

        if (moveInput != Vector3.zero)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
    }

    public float GetPickupRange()
    {
        return pickupRange;
    }

    public float GetMoveSpeed()
    {
        return moveSpeed;
    }

    // public Weapon GetActiveWeapon()
    // {
    //     return activeWeapon;
    // }

    public void AddWeapon(int weaponNumber)
    {
        if (weaponNumber < unassignedWeapons.Count)
        {
            assignedWeapons.Add(unassignedWeapons[weaponNumber]);

            unassignedWeapons[weaponNumber].gameObject.SetActive(true);
            unassignedWeapons.RemoveAt(weaponNumber);
        }
    }

    public void AddWeapon(Weapon weaponToAdd)
    {
        weaponToAdd.gameObject.SetActive(true);

        assignedWeapons.Add(weaponToAdd);

        unassignedWeapons.Remove(weaponToAdd);
    }
}
