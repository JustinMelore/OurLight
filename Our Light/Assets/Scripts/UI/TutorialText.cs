using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles behavior for the tutorial billboards placed throughout the level
/// </summary>
public class TutorialText : MonoBehaviour
{
    private TextMeshPro text;
    private Coroutine currentFade;
    [SerializeField] private float textFadeTime;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        text.alpha = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeText(1f));
    }

    private void OnTriggerExit(Collider other)
    {
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeText(0f));
    }

    /// <summary>
    /// Fades the tutorials text to a given alpha
    /// </summary>
    /// <param name="newAlpha">The new alpha value of the tutorial text</param>
    /// <returns></returns>
    private IEnumerator FadeText(float newAlpha)
    {
        float currentFadeTime = 0f;
        float startingAlpha = text.alpha;
        while(text.alpha != newAlpha)
        {
            currentFadeTime += Time.deltaTime;
            text.alpha = Mathf.Lerp(startingAlpha, newAlpha, currentFadeTime / textFadeTime);
            yield return null;
        }
    }
}
