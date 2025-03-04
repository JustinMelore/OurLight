using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
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
