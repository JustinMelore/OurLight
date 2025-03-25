using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private IntroScreen introScreen;
    
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        introScreen = FindFirstObjectByType<IntroScreen>(FindObjectsInactive.Include);
    }

    public void OnStart()
    {
        introScreen.gameObject.SetActive(true);
        introScreen.ShowIntroScreen();
    }

    public void OnQuit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
