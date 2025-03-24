using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    }

    private void SwitchToConfirmScreen()
    {
        respawnScreen.alpha = 0;
        respawnScreen.gameObject.SetActive(false);
        confirmationScreen.gameObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(confirmationScreen.gameObject.transform.Find("CancelQuitButton").gameObject);
        confirmationScreen.alpha = 1;
    }

    private void SwitchToRespawnScreen()
    {
        confirmationScreen.alpha = 0;
        confirmationScreen.gameObject.SetActive(false);
        EventSystem.current.SetSelectedGameObject(respawnScreen.gameObject.transform.Find("RespawnButton").gameObject);
        respawnScreen.alpha = 1;
        respawnScreen.gameObject.SetActive(true);
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
        EventSystem.current.SetSelectedGameObject(respawnScreen.gameObject.transform.Find("RespawnButton").gameObject);
        respawnScreen.alpha = 1f;
    }

    public void ShowDeathScreen()
    {
        Coroutine showDeathScreen = StartCoroutine(ShowDeathScreenRoutine(1f));
    }

    private IEnumerator HideDeathScreenRoutine(float fadeTime)
    {
        respawnScreen.alpha = 0f;
        EventSystem.current.SetSelectedGameObject(null);
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

    public void OnCancelRespawn()
    {
        SwitchToConfirmScreen();
    }

    public void OnConfirmQuit()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OnCancelQuit()
    {
        SwitchToRespawnScreen();
    }
}
