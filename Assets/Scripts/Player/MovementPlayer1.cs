using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MovementPlayer1 : MonoBehaviour
{
    public static MovementPlayer1 instance;
    public Attack attack;

    public GameManager gameManager;
    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;

    public string horizontalAxis;
    public string verticalAxis;
    public string attackButton;

    [SerializeField] private Rigidbody rb;
    private Vector3 movement;

    float MaxHP = 4;
    float currentHP;

    public GameObject attackIndicatorPrefab;
    private bool canAttack = true;
    public bool player1, player2, player3;
    public float attackAreaMultiplier = 1.5f;

    [Header("PlayerStats")]

    public float playerAtk;
    public float playerAtkSpd;
    public float playerRange;
    public float weaponDurability;
    public float playerAtkWidth;
    public float playerKnockback;
    public float attackAngle;

    public Vector3 knockbackDirection;
    public GameObject hitIndicator;

    void Start()
    {
        //gameManager = GameManager.Instance;
        playerAtk = 1f;
        playerAtkSpd = 1f;
        playerRange = 4f;
        playerAtkWidth = 1f;
        playerKnockback = 8f;
        StartCoroutine(AutoAttack());


        currentHP = MaxHP;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input from the user
        float moveX = Input.GetAxis(horizontalAxis);
        float moveZ = Input.GetAxis(verticalAxis);

        // Create a vector based on input
        movement = new Vector3(moveX, 0, moveZ);

        if (Input.GetKeyDown(attackButton))
        {
            if (canAttack)
            {
                StartCoroutine(AutoAttack());
            }
        }
    }

    void FixedUpdate()
    {
        // Apply movement to the rigidbody
        MovePlayer();

        // Apply rotation to the player
        RotatePlayer();
    }

    void MovePlayer()
    {

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void RotatePlayer()
    {
        if (movement != Vector3.zero)
        {

            Quaternion targetRotation = Quaternion.LookRotation(movement);


            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

   public void TakeDamage(float damage)
   {
        currentHP -= damage;
        Debug.Log("Got Hit, HP Remaining = " +currentHP);
        
        if (currentHP <= 0)
        {
            Die();
        }

   }

    void Die()
    {
        Destroy(gameObject);
        if (player1)
        {
            gameManager.Player1Dead();
            //UI player 1
        }

        if (player2)
        {
            gameManager.Player2Dead();
            //UI player 2
        }

        if (player3)
        {
            gameManager.Player3Dead();
            //UI player 3
        }
    }

    public void ChangeStats(float atk, float atkspd, float range, float atkradius, float durability, float knockback)
    {
        playerAtk = atk;
        playerAtkSpd = atkspd;
        playerAtkWidth = atkradius;
        playerRange = range;
        weaponDurability = durability;
        playerKnockback = knockback;
    }

    IEnumerator AutoAttack()
    {
        while (weaponDurability > 0)
        {
            if (canAttack)
            {
                AttackEnemy();
                yield return new WaitForSeconds(.5f);
                hitIndicator.SetActive(false);
                attack.isAttacking = false;

                yield return new WaitForSeconds(1f / playerAtkSpd);
            }
            else
            {
                yield return null;
            }
        }

        Debug.Log("Weapon is broken!");
    }

    void AttackEnemy()
    {
        attack.isAttacking = true;
        hitIndicator.SetActive(true);
    }
}
