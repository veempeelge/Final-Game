using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MovementPlayer1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;

    public string horizontalAxis;
    public string verticalAxis;
    public string attackButton;

    [SerializeField] private Rigidbody rb;
    private Vector3 movement;

    float MaxHP = 4;
    float currentHP;

    [Header("PlayerStats")]
    public float playerAtk, playerAtkSpd, playerRange, weaponDurability;
    private bool canAttack = true;

    void Start()
    {
        playerAtk = 1f;
        playerAtkSpd = 1f;
        playerRange = 1f;


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

        if (Input.GetKeyDown("e"))
        {
            if (canAttack)
            {
                StartCoroutine(Attack());
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
        // Normalize the movement vector to ensure consistent movement speed in all directions
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void RotatePlayer()
    {
        if (movement != Vector3.zero)
        {
            // Calculate the target rotation based on the movement direction
            Quaternion targetRotation = Quaternion.LookRotation(movement);

            // Smoothly rotate towards the target direction
            rb.rotation = Quaternion.RotateTowards(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

   public  void TakeDamage(float damage)
   {
        currentHP -= damage;
        Debug.Log(currentHP);
        
        if (currentHP <= 0)
        {
            //Destroy(this);
        }

   }

    public void ChangeStats(float atk, float atkspd, float range, float durability)
    {
        playerAtk = atk;
        playerAtkSpd = atkspd;
        playerRange = range;
        weaponDurability = durability;
    }

    IEnumerator Attack()
    {
        canAttack = false;

        // Find enemies within range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, playerRange);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                // Attack the enemy
                Enemy enemyScript = enemy.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(playerAtk);
                }

                // Reduce weapon durability
                weaponDurability--;

                // Check weapon durability
                if (weaponDurability <= 0)
                {
                    Debug.Log("Weapon is broken!");
                    canAttack = false;
                    yield break;
                }
            }
           
        }

        // Wait for the next attack based on attack speed
        yield return new WaitForSeconds(1f / playerAtkSpd);
        canAttack = true;

    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, playerRange);
    }
}
