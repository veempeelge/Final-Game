using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Enemy : MonoBehaviour
{
    public float Speed;
    GameObject player;
    Transform player1;
    public int health;
    int enemyAttack = 2;
    int index;
    

    public List<GameObject> Players = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        // Find all GameObjects with the tag "Player"
        GameObject[] foundPlayers = GameObject.FindGameObjectsWithTag("Player");
        // Add each found player to the list
        foreach (GameObject player in foundPlayers)
        {
            Players.Add(player);
            Debug.Log("Player added: " + player.name);
        }

        // Ensure there are players in the list before accessing it
        if (Players.Count > 0)
        {
            // Generate a random index within the range of the Players list
            index = Random.Range(0, Players.Count); // Note: Random.Range(max) is inclusive of 0 and exclusive of max
            player = Players[index];
            player1 = player.transform;

            Debug.Log("I am following player " + index);
        }
        else
        {
            Debug.LogWarning("No players found with the tag 'Player'.");
        }
    
}

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, player1.position, Speed * Time.deltaTime);
    }



    void OnTriggerEnter (Collider collision) 
    {
        if (collision.tag == "Player")
        {
            MovementPlayer1 player = collision.GetComponent<MovementPlayer1>();
            if (player != null)
            {
                // Call the PerformAction function on the PlayerScript
                player.TakeDamage(enemyAttack);
            }
            else
            {
                Debug.LogWarning("PlayerScript component not found on the collided GameObject.");
            }
        }
    }
    void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }


}