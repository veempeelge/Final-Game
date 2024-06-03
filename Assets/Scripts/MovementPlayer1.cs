using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MovementPlayer1 : MonoBehaviour
{
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

    public void ChangeStats(float atk, float atkspd, float range, float atkwidth, float durability, float knockback)
    {
        playerAtk = atk;
        playerAtkSpd = atkspd;
        playerAtkWidth = atkwidth;
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
                Attack();
                yield return new WaitForSeconds(1f / playerAtkSpd);
            }
            else
            {
                yield return null;
            }
        }

        Debug.Log("Weapon is broken!");
    }

    private void Attack()
    {
        canAttack = false;
        Vector3 attackCenter = transform.position + transform.forward * playerRange / 3f;
        Vector3 attackHalfExtents = new Vector3(playerAtkWidth / 2, 1f, playerRange / 3f);

        GameObject attackIndicator = Instantiate(attackIndicatorPrefab, attackCenter, transform.rotation);
        attackIndicator.transform.SetParent(transform); 
        attackIndicator.transform.localPosition = transform.InverseTransformPoint(attackCenter); 
        attackIndicator.transform.localRotation = Quaternion.identity; 
        attackIndicator.transform.localScale = new Vector3(playerAtkWidth, 2f, playerRange);
        Destroy(attackIndicator, 0.5f);

        // Find enemies within range
        Collider[] hitEnemies = Physics.OverlapSphere(attackCenter, playerRange);

        foreach (Collider enemy in hitEnemies)
        {
            {
                Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                if (angleToEnemy < attackAngle / 2)
                {
                    // Calculate the knockback direction
                    Vector3 knockbackDirection = directionToEnemy;

                    // Attack the enemy
                    Enemy enemyScript = enemy.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.TakeDamage(playerAtk, knockbackDirection, playerKnockback);
                    }

                    // Reduce weapon durability
                    weaponDurability--;

                    // Check weapon durability
                    if (weaponDurability <= 0)
                    {
                        Debug.Log("Weapon is broken!");
                        // Handle weapon break (e.g., disable further attacks, change weapon, etc.)
                        canAttack = false;
                        return;
                    }
                }
            }

        }
       
        canAttack = true;

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackCenter = transform.position + transform.forward * playerRange / 2;
        Gizmos.DrawWireSphere(attackCenter, playerRange);

        // Draw the cone angle
        Vector3 rightBoundary = Quaternion.Euler(0, attackAngle / 2, 0) * transform.forward * playerRange ;
        Vector3 leftBoundary = Quaternion.Euler(0, -attackAngle / 2, 0) * transform.forward * playerRange;
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
    }
}
