using UnityEngine;

/// <summary>
/// Handles behavior for interactable NPCs that trigger dialogue when approaching the player. This script is now deprecated and has been replaced with DialogueTrigger.
/// </summary>
public class InteractableNPC : MonoBehaviour
{
    [SerializeField] private string[] dialogueList;
    private DialogueBox dialogueBox;
    private bool hasSpoken;

    private void Awake()
    {
        dialogueBox = FindFirstObjectByType<DialogueBox>();
        hasSpoken = false;        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6 || hasSpoken) return;
        dialogueBox.StartDialogue(dialogueList);
        hasSpoken = true;
    }
}
