using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] MovementPlayer1 playerStats;
    public Material VisionConeMaterial;
    public float AttackRange;
    public float AttackAngle;
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
        DrawVisionCone();
    }

    void DrawVisionCone() // This method creates the vision cone mesh
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] vertices = new Vector3[VisionConeResolution + 1];
        vertices[0] = Vector3.zero;
        float currentAngle = -AttackAngle / 2;
        float angleIncrement = AttackAngle / (VisionConeResolution - 1);
        HashSet<Enemy> detectedEnemies = new HashSet<Enemy>();

        for (int i = 0; i < VisionConeResolution; i++)
        {
            float sine = Mathf.Sin(currentAngle);
            float cosine = Mathf.Cos(currentAngle);
            Vector3 raycastDirection = (transform.forward * cosine) + (transform.right * sine);
            Vector3 vertForward = (Vector3.forward * cosine) + (Vector3.right * sine);
            Vector3 offset = new Vector3(-1, 0, 0);
           

            if (Physics.Raycast(transform.position + offset, raycastDirection, out RaycastHit hit, AttackRange, VisionObstructingLayer | EnemyLayer))
            {
               // vertices[i + 1] = vertForward * hit.distance;
                if (((1 << hit.collider.gameObject.layer) & EnemyLayer) != 0)
                {
                    Enemy enemy = hit.collider.GetComponent<Enemy>();

                    if (enemy != null)

                    {
                        detectedEnemies.Add(enemy);
                        enemy.OnPlayerDetected(transform, playerStats, isAttacking);
                        isAttacking = false;
                    }
                }
            }
            else
            {
                vertices[i + 1] = vertForward * AttackRange;
            }

            currentAngle += angleIncrement;
        }

        if (isAttacking)
        {
            foreach (Enemy enemy in detectedEnemies)
            {
                Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
                enemy.TakeDamage(playerStats.playerAtk, directionToEnemy, playerStats.playerKnockback);
            }
            isAttacking = false;
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
