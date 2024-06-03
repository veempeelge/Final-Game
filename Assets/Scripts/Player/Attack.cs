using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
   [SerializeField] MovementPlayer1 playerStats;
    public Material VisionConeMaterial;
    public float AttackRange;
    public float AttackAngle;
    public Vector3 KnockbackDirection;
    public float knockbackForce;
    public float playerAttack;

    public LayerMask VisionObstructingLayer; // Layer with objects that obstruct the enemy view, like walls, for example
    public LayerMask EnemyLayer; // Layer for enemies
    public int VisionConeResolution = 120; // The vision cone will be made up of triangles, the higher this value is the prettier the vision cone will be
    Mesh VisionConeMesh;
    MeshFilter MeshFilter_;
    public bool isAttacking;

    void Start()
    {
        
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = VisionConeMaterial;
        MeshFilter_ = gameObject.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        AttackAngle *= Mathf.Deg2Rad;
    }

    void Update()
    {
        AttackRange = playerStats.playerRange;
        AttackAngle = playerStats.playerAtkWidth;
        playerAttack = playerStats.playerAtk;
        knockbackForce = playerStats.playerKnockback;

        DrawVisionCone(); // Calling the vision cone function every frame just so the cone is updated every frame
    }


    void DrawVisionCone() // This method creates the vision cone mesh
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] vertices = new Vector3[VisionConeResolution + 1];
        vertices[0] = Vector3.zero;
        float currentAngle = -AttackAngle / 2;
        float angleIncrement = AttackAngle / (VisionConeResolution - 1);

        for (int i = 0; i < VisionConeResolution; i++)
        {
            float sine = Mathf.Sin(currentAngle);
            float cosine = Mathf.Cos(currentAngle);
            Vector3 raycastDirection = (transform.forward * cosine) + (transform.right * sine);
            Vector3 vertForward = (Vector3.forward * cosine) + (Vector3.right * sine);


            if (Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit, AttackRange, VisionObstructingLayer | EnemyLayer))
            {
                vertices[i + 1] = vertForward * hit.distance;
                if (((1 << hit.collider.gameObject.layer) & EnemyLayer) != 0)
                {
                    Vector3 directionToEnemy = (hit.transform.position - transform.position).normalized;
                    float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                    if (isAttacking)
                    {
                        hit.collider.GetComponent<Enemy>().TakeDamage(playerAttack, directionToEnemy, knockbackForce);
                        isAttacking = false;
                    }
                        
                    // Enemy detected
                    Debug.Log("Enemy detected: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                vertices[i + 1] = vertForward * AttackRange;
            }

            currentAngle += angleIncrement;
        }

        for (int i = 0, j = 0; i < triangles.Length; i += 3, j++)
        {
            triangles[i] = 0;
            triangles[i + 1] = j + 1;
            triangles[i + 2] = j + 2;
        }

        VisionConeMesh.Clear();
        VisionConeMesh.vertices = vertices;
        VisionConeMesh.triangles = triangles;
        MeshFilter_.mesh = VisionConeMesh;
    }
}
