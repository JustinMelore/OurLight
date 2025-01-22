using UnityEngine;

/// <summary>
/// Handles the player's light and its functionality
/// </summary>
public class PlayerLight : MonoBehaviour
{

    private int subdivisionCount = 30;
    [SerializeField] private float angle;
    [SerializeField] private float distance;
    
    /// <summary>
    /// Creates the cone mesh that serves as the light's collision
    /// </summary>
    void Start()
    {
        Mesh lightColliderMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = lightColliderMesh;
        transform.rotation = Quaternion.Euler(new Vector3(90f, -angle / 2, 0));

        Vector3[] vertices = new Vector3[subdivisionCount + 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[subdivisionCount * 3];
        vertices[0] = Vector3.zero;
        float currentAngle = 0f;
        float angleIncrement = angle / subdivisionCount;

        for (int i = 0; i <= subdivisionCount; i++)
        {
            float angleRad = currentAngle * (Mathf.PI / 180f);
            Vector3 vertex = new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * distance;
            vertices[i+1] = vertex;

            if(i > 0)
            {
                triangles[i * 3 - 3] = 0;
                triangles[i * 3 - 2] = i;
                triangles[i * 3 - 1] = i + 1;
            }

            currentAngle -= angleIncrement;
        }

        lightColliderMesh.vertices = vertices;
        lightColliderMesh.uv = uvs;
        lightColliderMesh.triangles = triangles;
        GetComponent<MeshCollider>().sharedMesh = lightColliderMesh;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
