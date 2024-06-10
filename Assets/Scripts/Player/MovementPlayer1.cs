using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MovementPlayer1 : MonoBehaviour
{
    public static MovementPlayer1 instance;

    public Attack attack;
    public HPBar hpBar;

    public GameManager gameManager;
    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;

    public string horizontalAxis;
    public string verticalAxis;
    public string attackButton;

    [SerializeField] private Rigidbody rb;
    private Vector3 movement;

    public float MaxHP = 4;
    public float currentHP;

    public GameObject attackIndicatorPrefab;
    private bool canAttack = true;
    public bool player1, player2, player3;
    public float attackAreaMultiplier = 1.5f;

    [Header("PlayerStats")]

    public float playerAtk;
    public float playerAtkSpd;
    public float playerRange;
    public float weaponDurability;
    public float weaponCurrentDurability;
    public float playerAtkWidth;
    public float playerKnockback;

    public Vector3 knockbackDirection;
    public GameObject hitIndicator;
    private bool IsDecreased;
    private bool usingWeapon = false;

    [SerializeField] GameObject wpDurabilityBar;
    private float defaultSpeed;
    private bool isImmune;

    void Start()
    {
       // weaponDurability = weaponCurrentDurability;
        //gameManager = GameManager.Instance;
        playerAtk = 1f;
        playerAtkSpd = 1f;
        playerRange = 4f;
        playerAtkWidth = 1f;
        playerKnockback = 3f;
        StartCoroutine(AutoAttack());
        wpDurabilityBar.SetActive(false);
        defaultSpeed = moveSpeed;

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
        float maxSpeed = 5f;

        rb.AddForce(movement * moveSpeed * 5, ForceMode.Acceleration);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
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
        hpBar.UpdateBar(currentHP);
        Debug.Log("Got Hit, HP Remaining = " + currentHP);
        
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
        usingWeapon = true;
        playerAtk = atk;
        playerAtkSpd = atkspd;
        playerAtkWidth = atkradius;
        playerRange = range;
        weaponDurability = durability;
        playerKnockback = knockback;

        weaponDurability = durability;
        weaponCurrentDurability = durability;
        hpBar.UpdateDurabilityBar(weaponDurability, weaponCurrentDurability);
        wpDurabilityBar.SetActive(true);

    }

    IEnumerator AutoAttack()
    {
        while (weaponDurability > 0)
        {
            if (canAttack && !isImmune)
            {
                DurabilityCheck();
                AttackEnemy();
                yield return new WaitForSeconds(.3f);
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
        wpDurabilityBar.SetActive(false);

    }

    void ResetStats()
    {
        weaponDurability = 10f;
        playerAtk = 1f;
        playerAtkSpd = 1f;
        playerRange = 4f;
        playerAtkWidth = 1f;
        playerKnockback = 3f;
    }

    void DurabilityCheck()
    {
        if (weaponDurability < 0)
        {
            ResetStats();
        }
    }
    void AttackEnemy()
    {
        attack.isAttacking = true;
        hitIndicator.SetActive(true);
    }

    public void DecreaseDurability()
    {
        if (!IsDecreased && usingWeapon)
        {
            
            IsDecreased = true;
            weaponCurrentDurability -= 1f;
            Debug.Log(weaponDurability);
            Invoke(nameof(DecreaseOnce), .2f);
            hpBar.UpdateDurabilityBar(weaponDurability, weaponCurrentDurability);
        }

    }

    void DecreaseOnce()
    {
        IsDecreased = false;
    }

   void PowerUpHP(float amount)
    {
        currentHP += amount;
    }

    void PowerUpSpeed(float amount, float duration)
    {
        moveSpeed += amount;
        Invoke(nameof(SpeedReset), duration);
    }

    void Immune(float duration)
    {
        isImmune = true;
        Invoke(nameof(ImmuneReset), duration);
    }

    void SpeedReset()
    {
        moveSpeed = defaultSpeed;
    }

    void ImmuneReset()
    {
        isImmune = false;
    }

}
