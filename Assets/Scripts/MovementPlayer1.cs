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
    public float playerAtk, playerAtkSpd, playerRange, weaponDurability, playerAtkWidth, playerKnockback;
    public GameObject attackIndicatorPrefab;
    private bool canAttack = true;

    void Start()
    {
        playerAtk = 1f;
        playerAtkSpd = 1f;
        playerRange = 1f;
        playerAtkWidth = 1f;
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

    public void ChangeStats(float atk, float atkspd, float range, float durability, float knockback)
    {
        playerAtk = atk;
        playerAtkSpd = atkspd;
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
        Vector3 attackCenter = transform.position + transform.forward * playerRange / 2;
        Vector3 attackHalfExtents = new Vector3(playerAtkWidth / 2, 1f, playerRange / 2);

        GameObject attackIndicator = Instantiate(attackIndicatorPrefab, attackCenter, transform.rotation);
        attackIndicator.transform.SetParent(transform); 
        attackIndicator.transform.localPosition = transform.InverseTransformPoint(attackCenter); 
        attackIndicator.transform.localRotation = Quaternion.identity; 
        attackIndicator.transform.localScale = new Vector3(playerAtkWidth, 2f, playerRange);
        Destroy(attackIndicator, 0.5f); 

        // Find enemies within range
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, playerRange);
        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy"))
            {

                Vector3 knockbackDirection = (enemy.transform.position - transform.position).normalized;
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
                    canAttack = false;
                    return;
                }
            }
           
        }

        // Wait for the next attack based on attack speed
       
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
        Vector3 attackHalfExtents = new Vector3(playerAtkWidth / 2, 1f, playerRange / 2);
        Gizmos.matrix = Matrix4x4.TRS(attackCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackHalfExtents * 2);
    }
}
