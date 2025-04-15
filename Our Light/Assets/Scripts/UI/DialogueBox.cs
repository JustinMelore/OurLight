using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    private CanvasGroup boxDisplay;
    private Queue<string> dialogueQueue;
    private TextMeshProUGUI dialogueText;
    private PlayerLight playerLight;
    private PlayerController player;
    [SerializeField] private GameObject dialogueIcon;

    void Awake()
    {
        boxDisplay = GetComponent<CanvasGroup>();
        dialogueQueue = new Queue<string>();
        dialogueText = transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        playerLight = FindFirstObjectByType<PlayerLight>();
        player = FindFirstObjectByType<PlayerController>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
        dialogueIcon.SetActive(false);
    }

    public void StartDialogue(string[] dialogueList, bool fromChild = false)
    {
        playerLight.TurnOffLight();
        playerLight.enabled = false;
        player.enabled = false;
        if (fromChild) dialogueIcon.SetActive(true);
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
        playerLight.enabled = true;
        player.enabled = true;
        dialogueIcon.SetActive(false);
        gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
    }
}
