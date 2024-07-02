using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    Waypoints waypoints;

    public float rotationSpeed = 1f;
    public float speed;
    public float health;
    public float enemyAttack = 2;
    public float knockbackDuration = 0.2f;
    public float updateSpeed = 0.1f;
    public float stoppingDistance = 1.0f;

    private NavMeshAgent agent;
    private List<GameObject> players = new List<GameObject>();
    private Transform closestPlayer;
    private bool wasAttacked;
    private bool isKnockedBack;
    private Rigidbody rb;

    private Transform[] points;
    public GameObject targetPlayer;
    private Transform targetLocation;
    private Vector3 lastTargetPosition;

    public bool CanHitWater = true;

    [SerializeField] TMP_Text chasingWho;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        rb = GetComponent<Rigidbody>();

        GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");
        players.AddRange(foundPlayers);

        ChangeTarget();
    }

    void TargetPlayer()
    {
        if (players.Count == 0) return;

        targetPlayer = players[Random.Range(0, players.Count)];

        if (targetPlayer != null)
        {
            waypoints = targetPlayer.GetComponent<Waypoints>();
            points = waypoints?.points;

            if (points != null && points.Length > 0)
            {
                int index = Random.Range(0, points.Length);
                targetLocation = points[index];
                lastTargetPosition = targetLocation.position;
                Debug.Log($"Target player: {targetPlayer.name}, Waypoint index: {index}");
                chasingWho.text = $"{targetPlayer.name}, {index}";
                if (targetPlayer.name == "Player 1")
                {
                    chasingWho.color = Color.blue;
                }

                if (targetPlayer.name == "Player 2")
                {
                    chasingWho.color = Color.red;
                }

                if (targetPlayer.name == "Player 3")
                {
                    chasingWho.color = Color.green;
                }
            }
            else
            {
                Debug.Log("No waypoints found for the target player.");
            }
        }
        else
        {
            Debug.Log("Target player not found.");
        }
    }

    void Update()
    {
        if (!isKnockedBack && targetLocation != null)
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, targetLocation.position);

            if (distanceToWaypoint > stoppingDistance)
            {
                agent.isStopped = false;
                if (agent.isOnNavMesh && (agent.destination != targetLocation.position))
                {
                    agent.SetDestination(targetLocation.position);
                }

                // Make the enemy face the direction it is moving
                Vector3 velocity = agent.velocity;
                if (velocity.sqrMagnitude > 0.01f) // Check if the velocity is significant
                {
                    Quaternion lookRotation = Quaternion.LookRotation(velocity.normalized);
                    transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                targetLocation = targetPlayer.transform; // Move towards player after reaching waypoint
                if (agent.destination != targetLocation.position)
                {
                    agent.SetDestination(targetLocation.position);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Water"))
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

    public void OnPlayerHitWater()
    {
        //Debug.Log("Target another player");
        if (CanHitWater)
        {
            ChangeTarget();
            CanHitWater = false;
            Invoke(nameof(CanHitWaterCooldown), .2f);

        }

    }

    void CanHitWaterCooldown()
    {
        CanHitWater = true;
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

        rb.AddForce(direction * knockbackForce * 0.6f, ForceMode.Impulse);

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
        TargetPlayer();
    }
}
