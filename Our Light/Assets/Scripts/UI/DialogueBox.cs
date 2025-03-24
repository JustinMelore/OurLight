using UnityEngine;
using UnityEngine.EventSystems;
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
        gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(transform.Find("DialogueButton").gameObject);
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
        EventSystem.current.SetSelectedGameObject(null);
    }
}
