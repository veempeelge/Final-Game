using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class MovementPlayer1 : MonoBehaviour
{
    public static MovementPlayer1 instance;

    public Attack attack;
    public AttackWater attackWater;
    public Inv_Item item;

    public HPBar hpBar;

    public GameManager gameManager;
    public float moveSpeed;
    public float maxAcceleration;
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
    public GameObject waterHitIndicatorPrefab;
    private bool IsDecreased;
    private bool usingWeapon = false;

    [SerializeField] GameObject wpDurabilityBar;
    private float defaultSpeed;
    private bool isImmune;

    [Header("Audio")]
    [SerializeField] AudioClip attackAir;
    [SerializeField] AudioClip gotItem;
    private int waterCharge;
    private bool waterDecreased;

    public Item_Slot slot;

    void Start()
    { 
        item = GetComponent<Inv_Item>();
        attackWater = GetComponentInChildren<AttackWater>();
        waterCharge = slot.count;
        // weaponDurability = weaponCurrentDurability;
        //gameManager = GameManager.Instance;
        moveSpeed = gameManager.defSpeed;
        maxAcceleration = gameManager.defAcc;
        playerAtk = gameManager.defPlayerAtk;
        playerAtkSpd = gameManager.defPlayerAtkSpd;
        playerRange = gameManager.defPlayerRange;
        playerAtkWidth = gameManager.defPlayerAtkWidth;
        playerKnockback = gameManager.defPlayerKnockback;
        StartCoroutine(AutoAttack());
        StartCoroutine(WaterAttack());
        wpDurabilityBar.SetActive(false);
        defaultSpeed = moveSpeed;

        currentHP = MaxHP;
        rb = GetComponent<Rigidbody>();
    }



    void Update()
    {
       
        float moveX = Input.GetAxis(horizontalAxis);
        float moveZ = Input.GetAxis(verticalAxis);


        movement = new Vector3(moveX, 0, moveZ);
    }

    void FixedUpdate()
    {
        MovePlayer();

        if (rb.velocity.y < -.1f)
        {
            rb.velocity += Physics.gravity * Time.fixedDeltaTime;
        }


        RotatePlayer();
    }

    public void StartWaterCoroutine()
    {
        Debug.Log("StartWaterCoroutine called");
        SprayWater();
        StartCoroutine(WaterAttack());
    }

    void MovePlayer()
    {
        Vector3 normalizedMovement = movement.normalized;

        rb.AddForce(normalizedMovement * moveSpeed * 5, ForceMode.Acceleration);

        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxAcceleration);

       
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
        //Debug.Log("Got Hit, HP Remaining = " + currentHP);
        
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
        SoundManager.Instance.Play(gotItem);

    }

    IEnumerator AutoAttack()
    {
        while (weaponDurability > 0)
        {
            if (canAttack && !isImmune)
            {
                yield return new WaitForSeconds(1f / playerAtkSpd);
                DurabilityCheck();
                AttackEnemy();
                yield return new WaitForSeconds(.1f);
                hitIndicator.SetActive(false);
                attack.isAttacking = false;
            }
            else
            {
                yield return null;
            }
        }
        Debug.Log("Weapon is broken!");
        wpDurabilityBar.SetActive(false);
    }

    private IEnumerator WaterAttack()
    {
        while(slot.count > 0)
        {
            //Debug.Log("WaterSpray");
            yield return new WaitForSeconds(1);
            // DurabilityCheck();
            SprayWater();
            yield return new WaitForSeconds(.1f);
            waterHitIndicatorPrefab.SetActive(false);
            attackWater.isAttacking = false;
           
        }
        for (int i = 0; i < item.slots.Length; i++)
        {
            item.DiscardItem(i);
        }
        waterHitIndicatorPrefab.SetActive(false);
        yield break;

    }

    void ResetStats()
    {
        weaponDurability = 10f;
        playerAtk = gameManager.defPlayerAtk;
        playerAtkSpd = gameManager.defPlayerAtkSpd;
        playerRange = gameManager.defPlayerRange;
        playerAtkWidth = gameManager.defPlayerAtkWidth;
        playerKnockback = gameManager.defPlayerKnockback;
    }

    void DurabilityCheck()
    {
        if (weaponCurrentDurability <= 0)
        {
            ResetStats();
            wpDurabilityBar.SetActive(false);
        }
        else
        {
            return;
        }
    }
    void AttackEnemy()
    {
        attack.isAttacking = true;
        hitIndicator.SetActive(true);
        SoundManager.Instance.Play(attackAir);
    }

    void SprayWater()
    {
        attackWater.isAttacking = true;
        waterHitIndicatorPrefab.SetActive(true);
        //SoundManager Spraying Water
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

    public void DecreaseWaterCharge()
    {
        if (!waterDecreased)
        {
            if (waterCharge >= 0)
            {
                slot.count--;
                slot.RefreshCount();
                waterDecreased = true;
               // waterCharge--;
                Invoke(nameof(WaterDecreasedOnce), .2f);
                Debug.Log("Decreased Water " + waterCharge);
            }


        }
    }

    void WaterDecreasedOnce()
    {
        waterDecreased = false;
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
