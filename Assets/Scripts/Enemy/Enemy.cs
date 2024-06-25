using System.Collections;
using System.Collections.Generic;
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
    public float stoppingDistance = 2.0f; // Set an appropriate stopping distance

    private NavMeshAgent agent;
    private List<GameObject> players = new List<GameObject>();
    private Transform closestPlayer;
    private bool wasAttacked;
    private bool isKnockedBack;
    private Rigidbody rb;

    private Transform[] points;
    private GameObject targetPlayer;
    private Transform targetLocation;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance; 
        rb = GetComponent<Rigidbody>();

        GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");
        players.AddRange(foundPlayers);

        TargetPlayer();

        


    }

    void TargetPlayer()
    {
        int index;

        targetPlayer = players[Random.Range(0, players.Count)];

        if (targetPlayer != null)
        {
            waypoints = targetPlayer.GetComponent<Waypoints>();
            points = waypoints.points;
            Debug.Log(targetPlayer);
        }
        else
        {
            Debug.Log("TargetNotFound");
        }
            

         if (waypoints != null)
        {
            index = Random.Range(0, points.Length);
            targetLocation = points[index];
            Debug.Log(index);
        }
        else
        {
            Debug.Log("pointnoTFound");
        }
       
    }


    void Update()
    {
        if (!isKnockedBack && closestPlayer != null)
        {
            if(transform.position == targetLocation.position)
            {
                targetLocation = targetPlayer.gameObject.transform;
            }
            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);

            if (distanceToPlayer > stoppingDistance)
            {
                agent.isStopped = false;
                if (agent.isOnNavMesh)
                {
                    agent.SetDestination(targetLocation.position);
                }    
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

    public void OnPlayerHitWater()
    {
        Debug.Log("Target another player");
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


    //randomize player
    //add children to list
    //randomize children
    //get children posititon
    //enemy go to children position
    // enemy got to posititon -> go to player position

}