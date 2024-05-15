using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementPlayer2 : MonoBehaviour
{
    public float moveSpeed = 10f;

   [SerializeField] private Rigidbody rb;
    private Vector3 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Get input from the user
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Create a vector based on input
        movement = new Vector3(moveX, 0, moveZ);
    }

    void FixedUpdate()
    {
        // Apply movement to the rigidbody
        MovePlayer();
    }

    void MovePlayer()
    {
        // Normalize the movement vector to ensure consistent movement speed in all directions
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
