using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the behavior for the screen that shows up at the end of the game
/// </summary>
public class EndScreen : MonoBehaviour
{
    private CanvasGroup[] badEndingText;
    private CanvasGroup[] goodEndingText;
    [SerializeField] private CanvasGroup endButton;
    [SerializeField] private CanvasGroup fade;
    [SerializeField] private float fadeTime;
    [SerializeField] private float textRevealTime;
    [SerializeField] private float buttonRevealTime;


    void Awake()
    {
        badEndingText = transform.Find("BadEnding").GetComponentsInChildren<CanvasGroup>();
        goodEndingText = transform.Find("GoodEnding").GetComponentsInChildren<CanvasGroup>();
    }

    /// <summary>
    /// Shows the end screen
    /// </summary>
    public void ShowEndingScreen()
    {
        gameObject.SetActive(true);
        fade.gameObject.SetActive(true);
        StartCoroutine(ShowEndScreenCoroutine());
    }

    /// <summary>
    /// Coroutine that reveals the end screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowEndScreenCoroutine()
    {
        yield return StartCoroutine(FadeInCanvasGroup(fade, fadeTime));
        PlayerLight playerLight = FindFirstObjectByType<PlayerLight>();
        CanvasGroup[] endingText = (playerLight.HasUnlockedMode(LightMode.BRIDGE) ? goodEndingText : badEndingText);
        playerLight.enabled = false;
        FindFirstObjectByType<PlayerController>().enabled = false;
        float individualLineReveal = textRevealTime / endingText.Length;
        foreach (CanvasGroup line in endingText) yield return StartCoroutine(FadeInCanvasGroup(line, individualLineReveal));
        yield return new WaitForSeconds(buttonRevealTime);
        endButton.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(endButton.gameObject);
        endButton.alpha = 1f;
    }

    /// <summary>
    /// Fades in a given canvas group object
    /// </summary>
    /// <param name="group">The canvas group being faded in</param>
    /// <param name="neededFadeTime">How long the fade in should take</param>
    /// <returns></returns>
    private IEnumerator FadeInCanvasGroup(CanvasGroup group, float neededFadeTime)
    {
        float currentFadeTime = 0f;
        while (group.alpha < 1f)
        {
            currentFadeTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(0f, 1f, currentFadeTime / neededFadeTime);
            yield return null;
        }
    }

    /// <summary>
    /// Triggers when the player hits the continue button
    /// </summary>
    public void OnContinue()
    {
        SceneManager.LoadScene("MainMenu");
    }


}
