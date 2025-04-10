using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] GameObject resumeButton;
    [SerializeField] GameObject cancelQuitButton;
    [SerializeField] GameObject normalPauseScreen;
    [SerializeField] GameObject confirmQuitScreen;
    private PlayerController player;
    private PlayerLight playerLight;

    private void Awake()
    {
        canvasGroup.alpha = 0f;
        normalPauseScreen.SetActive(true);
        confirmQuitScreen.SetActive(false);
        gameObject.SetActive(false);
        player = FindFirstObjectByType<PlayerController>();
        playerLight = FindFirstObjectByType<PlayerLight>();
    }

    public void ShowPauseScreen()
    {
        player.enabled = false;
        playerLight.enabled = false;
        gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void HidePauseScreen()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Cursor.lockState = CursorLockMode.Locked;
        player.enabled = true;
        playerLight.enabled = true;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }

    public void TogglePauseScreen()
    {
        normalPauseScreen.SetActive(true);
        confirmQuitScreen.SetActive(false);
        EventSystem.current.SetSelectedGameObject(resumeButton);
    }

    public void ToggleQuitScreen()
    {
        normalPauseScreen.SetActive(false);
        confirmQuitScreen.SetActive(true);
        EventSystem.current.SetSelectedGameObject(cancelQuitButton);
    }
    
    public void OnConfirmQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
