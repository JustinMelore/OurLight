using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the behavior of the death screen that pops up when the player falls out of the world or runs out of light
/// </summary>
public class DeathScreen : MonoBehaviour
{
    private RawImage fade;
    private CanvasGroup respawnScreen;
    private CanvasGroup confirmationScreen;
    private GameManager gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        respawnScreen = transform.Find("RespawnScreen").GetComponent<CanvasGroup>();
        confirmationScreen = transform.Find("ConfirmationScreen").GetComponent<CanvasGroup>();
        gameManager = FindFirstObjectByType<GameManager>();
        fade = transform.GetComponentInChildren<RawImage>();
        fade.color = new Color(0, 0, 0, 1);
    }

    private void Start()
    {
        FadeIn(1.5f);
        confirmationScreen.gameObject.SetActive(false);
        respawnScreen.gameObject.SetActive(false);
    }

    /// <summary>
    /// Switches the death screen to the quit confirmation screen
    /// </summary>
    private void SwitchToConfirmScreen()
    {
        respawnScreen.alpha = 0;
        respawnScreen.gameObject.SetActive(false);
        confirmationScreen.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(confirmationScreen.gameObject.transform.Find("CancelQuitButton").gameObject);
        confirmationScreen.alpha = 1;
    }

    /// <summary>
    /// Switches the quit confirmation screen to the death screen
    /// </summary>
    private void SwitchToRespawnScreen()
    {
        confirmationScreen.alpha = 0;
        confirmationScreen.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(respawnScreen.gameObject.transform.Find("RespawnButton").gameObject);
        respawnScreen.alpha = 1;
        respawnScreen.gameObject.SetActive(true);
    }

    /// <summary>
    /// Fades the game screen in or out
    /// </summary>
    /// <param name="finalAlpha">The final alpha of the black fade</param>
    /// <param name="fadeTime">How long the fade should take</param>
    /// <returns></returns>
    private IEnumerator Fade(float finalAlpha, float fadeTime)
    {
        float currentFadeTime = 0f;
        float originalAlpha = fade.color.a;
        while(fade.color.a != finalAlpha)
        {
            currentFadeTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(originalAlpha, finalAlpha, currentFadeTime / fadeTime);
            fade.color = new Color(0, 0, 0, newAlpha);
            yield return null;
        }
    }

    /// <summary>
    /// Fades in the screen
    /// </summary>
    /// <param name="fadeTime">How long fading in should take</param>
    public void FadeIn(float fadeTime)
    {
        Coroutine fadeIn = StartCoroutine(Fade(0, fadeTime));
    }

    /// <summary>
    /// Fades out the screen
    /// </summary>
    /// <param name="fadeTime">How long fading out should take</param>
    public void FadeOut(float fadeTime)
    {
        Coroutine fadeOut = StartCoroutine(Fade(1, fadeTime));
    }

    /// <summary>
    /// Coroutine that reveals the death screen
    /// </summary>
    /// <param name="fadeTime">How long fading in should take</param>
    /// <returns></returns>
    private IEnumerator ShowDeathScreenRoutine(float fadeTime)
    {
        yield return StartCoroutine(Fade(1f, fadeTime));
        Cursor.lockState = CursorLockMode.None;
        EventSystem.current.SetSelectedGameObject(respawnScreen.gameObject.transform.Find("RespawnButton").gameObject);
        respawnScreen.gameObject.SetActive(true);
        respawnScreen.alpha = 1f;
    }

    /// <summary>
    /// Shows the player's death screen
    /// </summary>
    public void ShowDeathScreen()
    {
        Coroutine showDeathScreen = StartCoroutine(ShowDeathScreenRoutine(1f));
    }

    /// <summary>
    /// Coroutine that hides the death screen
    /// </summary>
    /// <param name="fadeTime">How long fading should take</param>
    /// <returns></returns>
    private IEnumerator HideDeathScreenRoutine(float fadeTime)
    {
        respawnScreen.alpha = 0f;
        respawnScreen.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(null);
        Cursor.lockState = CursorLockMode.Locked;
        yield return StartCoroutine(Fade(0f, fadeTime));
    }

    /// <summary>
    /// Hides the death screen from the player
    /// </summary>
    public void HideDeathScreen()
    {
        Coroutine hideDeathScreen = StartCoroutine(HideDeathScreenRoutine(1f));
    }

    /// <summary>
    /// Triggers when the player presses the respawn button
    /// </summary>
    public void OnConfirmRespawn()
    {
        gameManager.Respawn();
    }

    /// <summary>
    /// Triggers when the player clicks the button to not respawn
    /// </summary>
    public void OnCancelRespawn()
    {
        SwitchToConfirmScreen();
    }

    /// <summary>
    /// Triggers when the player affirms that they want to quit the game
    /// </summary>
    public void OnConfirmQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Triggers when the player decides to not quit the game
    /// </summary>
    public void OnCancelQuit()
    {
        SwitchToRespawnScreen();
    }
}
