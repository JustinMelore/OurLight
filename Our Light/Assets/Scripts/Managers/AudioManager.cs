using System.Collections;
using UnityEngine;

/// <summary>
/// Handles the background music that plays during the level
/// </summary>
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

    /// <summary>
    /// Swaps out the first background soundtrack for the second
    /// </summary>
    public void SwitchBackgroundMusic()
    {
        Coroutine fadeOut = StartCoroutine(FadeMusic(bgSountracks[0], 0f));
        Coroutine fadeIn = StartCoroutine(FadeMusic(bgSountracks[1], audioVolumeMax));
    }

    /// <summary>
    /// Fades a background track's volume to a new volume
    /// </summary>
    /// <param name="music">The audio source whose volume is being changed</param>
    /// <param name="finalVolume">THe final volume level for the music</param>
    /// <returns></returns>
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
