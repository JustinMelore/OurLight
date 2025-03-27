using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string[] dialogueList;
    [SerializeField] private LightMode requiredMode;
    private PlayerLight playerLight;
    private DialogueBox dialogueBox;
    private bool hasSpoken;

    private void Awake()
    {
        dialogueBox = FindFirstObjectByType<DialogueBox>();
        hasSpoken = false;
        playerLight = FindFirstObjectByType<PlayerLight>();
    }


    private void OnTriggerStay(Collider other)
    {
        if ((other.gameObject.layer != 6 && other.gameObject.layer != 9) 
            || hasSpoken 
            || !playerLight.HasUnlockedMode((LightMode)requiredMode)) return;
        dialogueBox.StartDialogue(dialogueList);
        hasSpoken = true;
    }
}
