using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Handles the behavior for the intro screen
/// </summary>
public class IntroScreen : MonoBehaviour
{
    private CanvasGroup[] introText;
    private CanvasGroup startButton;
    private CanvasGroup fade;
    [SerializeField] private float fadeTime;
    [SerializeField] private float textRevealTime;
    [SerializeField] private float buttonRevealTime;

    void Awake()
    {
        fade = transform.parent.Find("Fade").GetComponent<CanvasGroup>();
        introText = transform.Find("IntroText").GetComponentsInChildren<CanvasGroup>();
        startButton = transform.Find("StartButton").GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Fades in the intro screen and slowly reveals the opening poem text
    /// </summary>
    public void ShowIntroScreen()
    {
        fade.gameObject.SetActive(true);
        StartCoroutine(ShowIntroScreenCoroutine());
    }

    /// <summary>
    /// Coroutine that fades in the intro screen's background and slowly reveals the intro text
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowIntroScreenCoroutine()
    {
        yield return StartCoroutine(FadeInCanvasGroup(fade, fadeTime));
        float individualLineReveal = textRevealTime / introText.Length;
        foreach (CanvasGroup line in introText) yield return StartCoroutine(FadeInCanvasGroup(line, individualLineReveal));
        yield return new WaitForSeconds(buttonRevealTime);
        startButton.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        startButton.alpha = 1f;
    }

    /// <summary>
    /// Fades in a selected CanvasGroup over a given duration
    /// </summary>
    /// <param name="group">The CanvasGroup being faded in</param>
    /// <param name="neededFadeTime">How long it should take for the CanvasGroup to fade in</param>
    /// <returns></returns>
    private IEnumerator FadeInCanvasGroup(CanvasGroup group, float neededFadeTime)
    {
        float currentFadeTime = 0f;
        while(group.alpha < 1f)
        {
            currentFadeTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(0f, 1f, currentFadeTime / neededFadeTime);
            yield return null;
        }
    }

    /// <summary>
    /// Triggers when the start button is clicked. Loads the game level
    /// </summary>
    public void OnStart()
    {
        SceneManager.LoadScene("Level1Test");
    }
}
