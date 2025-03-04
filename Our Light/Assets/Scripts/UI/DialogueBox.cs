using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    private CanvasGroup boxDisplay;
    private Queue<string> dialogueQueue;
    private TextMeshProUGUI dialogueText;

    void Awake()
    {
        boxDisplay = GetComponent<CanvasGroup>();
        dialogueQueue = new Queue<string>();
        dialogueText = transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void StartDialogue(string[] dialogueList)
    {
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(true);
        foreach(string dialogue in dialogueList) dialogueQueue.Enqueue(dialogue);
        boxDisplay.alpha = 1f;
        NextDialogue();
    }

    public void NextDialogue()
    {
        if(dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }
        dialogueText.text = dialogueQueue.Dequeue();
    }

    public void EndDialogue()
    {
        boxDisplay.alpha = 0f;
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
