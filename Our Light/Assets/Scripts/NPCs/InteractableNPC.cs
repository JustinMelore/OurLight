using UnityEngine;

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
