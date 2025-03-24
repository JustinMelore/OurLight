using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OnStart()
    {
        SceneManager.LoadScene("Level1Test");
    }

    public void OnQuit()
    {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
