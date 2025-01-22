using UnityEngine;

public class PlayerLight : MonoBehaviour
{

    private int subdivisionCount = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Mesh lightColliderMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = lightColliderMesh;

        Vector3[] vertices = new Vector3[subdivisionCount + 2];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[subdivisionCount * 3];


        vertices[0] = new Vector3(1, 0, 1);
        vertices[1] = new Vector3(0, 1, 1);
        vertices[2] = new Vector3(2, 1, 1);

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        //vertices[0] = Vector3.zero;
        //for(int i = 0; i <= subdivisionCount; i++)
        //{
        //    Vector3
        //}

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
