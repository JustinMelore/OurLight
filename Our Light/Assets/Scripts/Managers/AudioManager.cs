using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource[] bgSountracks;
    [SerializeField] private float audioFadeTime;
    private float currentAudioFadeTime;
    [SerializeField] private float audioVolumeMax;

    private void Awake()
    {
        bgSountracks = transform.GetComponents<AudioSource>();
        bgSountracks[0].volume = audioVolumeMax;
        bgSountracks[1].volume = 0f;
    }

    public void SwitchBackgroundMusic()
    {
        Coroutine fadeOut = StartCoroutine(FadeMusic(bgSountracks[0], 0f));
        Coroutine fadeIn = StartCoroutine(FadeMusic(bgSountracks[1], audioVolumeMax));
    }

    private IEnumerator FadeMusic(AudioSource music, float finalVolume)
    {
        float startingVolume = music.volume;
        while(music.volume != finalVolume)
        {
            currentAudioFadeTime += Time.deltaTime;
            float currentVolume = Mathf.Lerp(startingVolume, finalVolume, currentAudioFadeTime / audioFadeTime);
            music.volume = currentVolume;
            yield return null;
        }
    }
}
