using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    public static Enemy instance;
    public float rotationSpeed = 1f;
    public float Speed;
    GameObject player;
    Transform player1;
    public float health;
    public float enemyAttack = 2;
    public float knockbackDuration = 0.2f;
    Vector3 parameter = new Vector3 (1,0,1);
    int index;
    public  Rigidbody body;
    

    public List<GameObject> Players = new List<GameObject>();

    bool wasAttacked;


    // Start is called before the first frame update
    void Start()
    {
        // Find all GameObjects with the tag "Player"
        GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");
        // Add each found player to the list
        foreach (GameObject player in foundPlayers)
        {
            Players.Add(player);
           // Debug.Log("Player added: " + player.name);
        }

        // Ensure there are players in the list before accessing it
        if (Players.Count > 0)
        {
            // Generate a random index within the range of the Players list
            index = Random.Range(0, Players.Count); // Note: Random.Range(max) is inclusive of 0 and exclusive of max
            player = Players[index];
            player1 = player.transform;

           // Debug.Log("I am following player " + index);
        }
        else
        {
          //  Debug.LogWarning("No players found with the tag 'Player'.");
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (player != null)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, player1.position + parameter, Speed * Time.deltaTime);

            Vector3 direction = (player1.position - transform.position).normalized;
            transform.position += direction * Speed * Time.deltaTime;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotationSpeed * Time.deltaTime);
            transform.position = Vector3.MoveTowards(transform.position, player1.position + parameter, Speed * Time.deltaTime);
        }
        else
        {
            return;
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
            Invoke(nameof(CanTakeDamage), .2f);
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

}
