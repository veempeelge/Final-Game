using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public static Attack Instance;
    public Material VisionConeMaterial;
    public float VisionRange;
    public float VisionAngle;
    public LayerMask VisionObstructingLayer; // Layer with objects that obstruct the enemy view, like walls, for example
    public LayerMask EnemyLayer; // Layer for enemies
    public int VisionConeResolution = 120; // The vision cone will be made up of triangles, the higher this value is the prettier the vision cone will be
    Mesh VisionConeMesh;
    MeshFilter MeshFilter_;

    void Start()
    {
        var meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = VisionConeMaterial;
        MeshFilter_ = gameObject.AddComponent<MeshFilter>();
        VisionConeMesh = new Mesh();
        VisionAngle *= Mathf.Deg2Rad;
    }

    void Update()
    {
        //DrawVisionCone(); // Calling the vision cone function every frame just so the cone is updated every frame
    }

    public void DrawVisionCone(float playerAtk, float playerKnockback, float atkRadius, float range) // This method creates the vision cone mesh
    {
        int[] triangles = new int[(VisionConeResolution - 1) * 3];
        Vector3[] vertices = new Vector3[VisionConeResolution + 1];
        vertices[0] = Vector3.zero;
        float currentAngle = -atkRadius * Mathf.Deg2Rad / 2;
        float angleIncrement = atkRadius * Mathf.Deg2Rad / (VisionConeResolution - 1);

        for (int i = 0; i < VisionConeResolution; i++)
        {
            float sine = Mathf.Sin(currentAngle);
            float cosine = Mathf.Cos(currentAngle);
            Vector3 raycastDirection = (transform.forward * cosine) + (transform.right * sine);
            Vector3 vertForward = (Vector3.forward * cosine) + (Vector3.right * sine);

            if (Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit, VisionRange, VisionObstructingLayer | EnemyLayer))
            {
                Vector3 directionToEnemy = (hit.collider.gameObject.transform.position - transform.position).normalized;
                float angleToEnemy = Vector3.Angle(transform.forward, directionToEnemy);

                vertices[i + 1] = vertForward * hit.distance;
                if (((1 << hit.collider.gameObject.layer) & EnemyLayer) != 0)
                {
                    Vector3 knockbackDirection = directionToEnemy;
                    Enemy enemyScript = hit.collider.gameObject.GetComponent<Enemy>();
                    if (enemyScript != null)
                    {
                        enemyScript.TakeDamage(playerAtk, knockbackDirection, playerKnockback);
                    }

                    Debug.Log("Enemy detected: " + hit.collider.gameObject.name);
                }
            }
            else
            {
                vertices[i + 1] = vertForward * range;
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