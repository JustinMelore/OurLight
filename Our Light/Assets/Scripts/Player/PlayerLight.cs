using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

/// <summary>
/// Handles the player's light and its functionality
/// </summary>
public class PlayerLight : MonoBehaviour
{

    private int subdivisionCount = 30;
    private PlayerInput playerInput;
    private MeshCollider coneCollider;
    private bool isLightOn;
    private Light visualLight;
    private int lightStacks;

    [SerializeField] private float angle;
    [SerializeField] private float distance;
    [SerializeField] private int lightStackMax;

    GameManager gameManager;
    LightIndicator lightUI;

    void Start()
    {
        playerInput = new PlayerInput();
        gameManager = FindFirstObjectByType<GameManager>();
        lightUI = FindFirstObjectByType<LightIndicator>();
        isLightOn = false;
        lightStacks = lightStackMax;
        coneCollider = GetComponent<MeshCollider>();
        SetupVisualLight();
        GenerateColliderMesh();
    }

    /// <summary>
    /// Creates the cone mesh that serves as the light's collision
    /// </summary>
    private void GenerateColliderMesh()
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
            vertices[i + 1] = vertex;

            if (i > 0)
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
        coneCollider.sharedMesh = lightColliderMesh;
    }

    /// <summary>
    /// Sets up the visible spotlight that indicates the light's effective area
    /// </summary>
    private void SetupVisualLight()
    {
        visualLight = GameObject.FindGameObjectWithTag("VisibleLight").GetComponent<Light>();
        visualLight.range = distance + distance / 5;
        visualLight.spotAngle = angle;
        visualLight.innerSpotAngle = angle;
    }

    /// <summary>
    /// Toggles the player's light
    /// </summary>
    private void OnUseLight()
    {
        isLightOn = !isLightOn;
        coneCollider.enabled = isLightOn;
        visualLight.enabled = isLightOn;
    }

    /// <summary>
    /// Disables the light and resets it back to its max stacks
    /// </summary>
    public void ResetLight()
    {
        if (isLightOn) OnUseLight();
        AddLightStack(lightStackMax);
    }
    
    /// <summary>
    /// Adds the given value to the number of light stacks
    /// </summary>
    /// <param name="stackChange">The amount of light to add</param>
    public void AddLightStack(int stackChange)
    {
        lightStacks += stackChange;
        if(lightStacks > lightStackMax) lightStacks = lightStackMax;
        else if(lightStacks <= 0) gameManager.KillPlayer();
        lightUI.SetLightAmount(lightStacks);
        Debug.Log("Light stack amount: " + lightStacks);
    }
}
