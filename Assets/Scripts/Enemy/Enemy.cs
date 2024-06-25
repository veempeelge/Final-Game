using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public float rotationSpeed = 1f;
    public float speed;
    public float health;
    public float enemyAttack = 2;
    public float knockbackDuration = 0.2f;
    public float updateSpeed = 0.1f;
    public float stoppingDistance = 2.0f; // Set an appropriate stopping distance

    private NavMeshAgent agent;
    private List<GameObject> players = new List<GameObject>();
    private Transform closestPlayer;
    private bool wasAttacked;
    private bool isKnockedBack;
    private Rigidbody rb;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance; // Set the stopping distance
        rb = GetComponent<Rigidbody>();

        GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");
        players.AddRange(foundPlayers);

        StartCoroutine(FollowClosestPlayer());
    }

    void Update()
    {
        if (!isKnockedBack && closestPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);

            if (distanceToPlayer > stoppingDistance)
            {
                agent.isStopped = false;
                agent.SetDestination(closestPlayer.position);
            }
            else
            {
                agent.isStopped = true;
            }

            Vector3 direction = (closestPlayer.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private Transform FindClosestPlayer()
    {
        Transform nearestPlayer = null;
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            if (player == null) continue;

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
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);

        while (true)
        {
            closestPlayer = FindClosestPlayer();

            if (closestPlayer != null && !isKnockedBack)
            {
                agent.SetDestination(closestPlayer.position);
            }

            yield return wait;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == "Water")
        {
            Debug.Log("Got hit by holy water");
            Destroy(collision.gameObject);
            ChangeTarget();
        }
    }

    public void TakeDamage(float damageAmount, Vector3 knockbackDirection, float knockbackForce)
    {
        health -= damageAmount;
        isKnockedBack = true;

        if (rb != null)
        {
            StartCoroutine(Knockback(knockbackDirection, knockbackForce));
        }
    }

    public void OnPlayerDetected(Transform playerTransform, MovementPlayer1 playerStats)
    {
        Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
        if (!wasAttacked)
        {
            wasAttacked = true;
            TakeDamage(playerStats.playerAtk, -directionToPlayer, playerStats.playerKnockback);
            Invoke(nameof(CanTakeDamage), 0.2f);
        }
    }

    void CanTakeDamage()
    {
        wasAttacked = false;
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private IEnumerator Knockback(Vector3 direction, float knockbackForce)
    {
        float elapsedTime = 0f;
        agent.enabled = false; // Disable NavMeshAgent during knockback

        rb.AddForce(direction * knockbackForce * .6f, ForceMode.Impulse);

        while (elapsedTime < knockbackDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        rb.velocity = Vector3.zero; // Stop any remaining velocity
        agent.enabled = true; // Re-enable NavMeshAgent

        if (health <= 0)
        {
            Die();
        }

        isKnockedBack = false;
    }

    public void ChangeTarget()
    {
        // Randomize the list of players
        if (players.Count > 0)
        {
            GameObject randomPlayer;
            do
            {
                randomPlayer = players[Random.Range(0, players.Count)];
            } while (randomPlayer.transform == closestPlayer);

            closestPlayer = randomPlayer.transform;
        }
    }
}