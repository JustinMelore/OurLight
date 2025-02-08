using UnityEngine;

public class InteractableNPC : MonoBehaviour
{
    [SerializeField] private string[] dialogueList;
    private DialogueBox dialogueBox;
    private bool hasSpoken;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueBox = FindFirstObjectByType<DialogueBox>();
        hasSpoken = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasSpoken) return;
        dialogueBox.StartDialogue(dialogueList);
        hasSpoken = true;
    }
}
