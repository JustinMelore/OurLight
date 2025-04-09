using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Plays walking sound effects for the player
/// </summary>
public class Footsteps : MonoBehaviour
{
    [SerializeField] AudioClip[] walkingSounds;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a random footstep sound effect from the WalkingSounds array
    /// </summary>
    private void Step()
    {
        audioSource.pitch = UnityEngine.Random.Range(0.6f, 1.2f);
        audioSource.clip = walkingSounds[UnityEngine.Random.Range(0, walkingSounds.Length)];
        audioSource.Play();
    }
}
