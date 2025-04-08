using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private IntroScreen introScreen;
    
    private void Awake()
    {
        introScreen = FindFirstObjectByType<IntroScreen>(FindObjectsInactive.Include);
    }

    public void OnStart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        introScreen.gameObject.SetActive(true);
        introScreen.ShowIntroScreen();
    }

    public void OnQuit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
