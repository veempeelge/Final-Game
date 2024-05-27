using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementPlayer1 : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 700f;

    public string horizontalAxis ;
    public string verticalAxis ;

    [SerializeField] private Rigidbody rb;
    private Vector3 movement;

    int MaxHP = 4;
    int currentHP;

    void Start()
    {
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

   public  void TakeDamage(int damage)
    {
        currentHP -= damage;
        Debug.Log(currentHP);
        
        if (currentHP <= 0)
        {
            //Destroy(this);
        }

    }
}
