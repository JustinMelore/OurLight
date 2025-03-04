using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    private RawImage fade;
    private CanvasGroup respawnScreen;
    private GameManager gameManager;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        respawnScreen = transform.Find("RespawnScreen").GetComponent<CanvasGroup>();
        gameManager = FindFirstObjectByType<GameManager>();
        fade = transform.GetComponentInChildren<RawImage>();
        fade.color = new Color(0, 0, 0, 1);
    }

    private void Start()
    {
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

    private IEnumerator ShowDeathScreenRoutine(float fadeTime)
    {
        yield return StartCoroutine(Fade(1f, fadeTime));
        respawnScreen.alpha = 1f;
    }

    public void ShowDeathScreen()
    {
        Coroutine showDeathScreen = StartCoroutine(ShowDeathScreenRoutine(1f));
    }

    private IEnumerator HideDeathScreenRoutine(float fadeTime)
    {
        respawnScreen.alpha = 0f;
        yield return StartCoroutine(Fade(0f, fadeTime));
    }

    public void HideDeathScreen()
    {
        Coroutine hideDeathScreen = StartCoroutine(HideDeathScreenRoutine(1f));
    }

    public void OnConfirmRespawn()
    {
        gameManager.Respawn();
    }

    //TODO Replace this with code that takes you to the main menu
    public void OnCancelRespawn()
    {
        Application.Quit();
    }
}
