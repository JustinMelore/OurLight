using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    private RawImage fade;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fade = transform.GetComponentInChildren<RawImage>();
        fade.color = new Color(0, 0, 0, 1);
        FadeIn(1.5f);
    }

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

    public void FadeIn(float fadeTime)
    {
        Coroutine fadeIn = StartCoroutine(Fade(0, fadeTime));
    }

    public void FadeOut(float fadeTime)
    {
        Coroutine fadeOut = StartCoroutine(Fade(1, fadeTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
