using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public static Enemy instance;
    public float rotationSpeed = 1f;
    public float Speed;
    public float health;
    public float enemyAttack = 2;
    public float knockbackDuration = 0.2f;
    Vector3 parameter = new Vector3 (1,0,1);
    public float UpdateSpeed = 0.1f;
    GameObject player;
    Transform player1;
    int index;
    public Rigidbody body;
    public Transform Player1;

    private NavMeshAgent Agent;
    public List<GameObject> Players = new List<GameObject>();
    private Transform closestPlayer;
    private bool wasAttacked;
    private Transform currentEnemy;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();

        GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in foundPlayers)
        {
            Players.Add(player);
        }

        StartCoroutine(FollowClosestPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        if (closestPlayer != null)
        {
            Vector3 direction = (closestPlayer.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
        else
        {
            FindClosestPlayer();
        }

        
    }

    private Transform FindClosestPlayer()
    {
        Players.RemoveAll(player => player == null); 

        Transform nearestPlayer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject player in Players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < shortestDistance)
            {
                shortestDistance = distanceToPlayer;
                nearestPlayer = player.transform;
            }
        }

        return nearestPlayer;
    }

    private IEnumerator FollowClosestPlayer()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateSpeed);

        while (true)
        {
            closestPlayer = FindClosestPlayer();
            currentEnemy = closestPlayer;

            if (currentEnemy != null)
            {
                Agent.SetDestination(closestPlayer.position);
               // ChangeTarget();
            }

            yield return Wait;
        }

       
    }

    private void OnTriggerEnter(Collider collision)
    {
       
        if (collision.tag == "Water")
        {
            Debug.Log("Got hit by holy water");
            Destroy(collision.gameObject);
            //ChangeTarget();
        }
    }

    public void TakeDamage(float damageAmount,Vector3 knockbackDirection, float knockbackForce)
    {
        health -= damageAmount;
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 multiplier = new Vector3(0, -knockbackDirection.y, 0);

        if (rb != null)
        {
            StartCoroutine(Knockback(-knockbackDirection + multiplier, knockbackForce));
        }
        
    }

    public void OnPlayerDetected(Transform playerTransform, MovementPlayer1 playerStats)
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        if (!wasAttacked)
        {
            wasAttacked = true;
            TakeDamage(playerStats.playerAtk, directionToPlayer, playerStats.playerKnockback);
            Invoke(nameof(CanTakeDamage), 0.2f);
         //   Debug.Log("Hit");
        }
    }

   void CanTakeDamage()
    {
        wasAttacked = false;
    }

    private void Die()
    {
        // Handle enemy death
        Destroy(gameObject);
    }

    private IEnumerator Knockback(Vector3 direction, float knockbackForce)
    {
        float elapsedTime = 0f;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = startPosition + direction * knockbackForce;

        while (elapsedTime < knockbackDuration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / knockbackDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (health <= 0)
        {
            yield return new WaitForSeconds(.1f);
            Die();
        }

        transform.position = targetPosition; // Ensure the final position is set
    }

    public void ChangeTarget()
    {
       // List playernya di randomize

       // if result = currentEnemy
       ChangeTarget();
       // else 
       // currentEnemy = hasilRandomizenya

       //^^ Biar ga dpt enemy yg sama

       // diatas lu cache playernya atau (udh gw lakuin, atau kalau mau ganti2 lagi)
       // call ini method waktu kena collisionnya water (ud gw lakuin)
       
    }
}
