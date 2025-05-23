using UnityEngine;

/// <summary>
/// Handles behavior for dialogue triggers. Dialogue triggers can either be attatched to NPCs or can be independently placed in the environment.
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private string[] dialogueList;
    [SerializeField] private LightMode requiredMode;
    [SerializeField] private bool showIcon = false;
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
        dialogueBox.StartDialogue(dialogueList, showIcon);
        hasSpoken = true;
    }
}
