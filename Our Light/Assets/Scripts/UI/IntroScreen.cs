using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        //introText = transform.Find("IntroText").GetComponent<CanvasGroup>();
        introText = transform.Find("IntroText").GetComponentsInChildren<CanvasGroup>();
        startButton = transform.Find("StartButton").GetComponent<CanvasGroup>();
    }

    public void ShowIntroScreen()
    {
        fade.gameObject.SetActive(true);
        StartCoroutine(ShowIntroScreenCoroutine());
    }

    private IEnumerator ShowIntroScreenCoroutine()
    {
        yield return StartCoroutine(FadeInCanvasGroup(fade, fadeTime));
        float individualLineReveal = textRevealTime / introText.Length;
        //yield return StartCoroutine(FadeInCanvasGroup(introText, textRevealTime));
        foreach (CanvasGroup line in introText) yield return StartCoroutine(FadeInCanvasGroup(line, individualLineReveal));
        yield return new WaitForSeconds(buttonRevealTime);
        startButton.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        startButton.alpha = 1f;
    }

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

    public void OnStart()
    {
        SceneManager.LoadScene("Level1Test");
    }
}
