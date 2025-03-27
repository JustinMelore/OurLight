using System.Collections;
using TMPro;
using UnityEngine;

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
        Debug.Log("Entered tutorial range");
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeText(1f));
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited tutorial range");
        if (currentFade != null) StopCoroutine(currentFade);
        currentFade = StartCoroutine(FadeText(0f));
    }

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
