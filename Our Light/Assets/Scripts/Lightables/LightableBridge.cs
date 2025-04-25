using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Handles the behavior for the lightable bridges
/// </summary>
public class LightableBridge : Lightable
{
    private List<GameObject> blocks;
    private List<Vector3> blockScale;
    private Color crystalBaseColor;
    private float currentIntensity;
    private Coroutine glow;
    private Coroutine fade;
    private AudioSource audioSource;
    private float startingVolume;
    [SerializeField] private AudioClip playerLightRevealSound;
    [SerializeField] private AudioClip npcLightRevealSound;
    [SerializeField] private float blockScaleTime;
    [SerializeField] private float crystalGlowIntensity;
    [SerializeField] private float bridgeRevealTime;

    protected override void Awake()
    {
        base.Awake();
        specialModes.Add(LightMode.BRIDGE);
        blocks = new List<GameObject>();
        blockScale = new List<Vector3>();
        Transform blocksContainer = transform.Find("Blocks");
        audioSource = GetComponent<AudioSource>();
        startingVolume = audioSource.volume;
        for (int i = 0; i < blocksContainer.childCount; i++)
        {
            blocks.Add(blocksContainer.GetChild(i).gameObject);
            blockScale.Add(blocks[i].transform.localScale);
            blocks[i].transform.localScale = Vector3.zero;
        }
        crystalBaseColor = GetCrystalRenderers(lightDetectors[0])[0].material.GetColor("_EmissionColor");
        currentIntensity = 1f;
    }

    /// <summary>
    /// Returns the mesh renderers for this bridge's crystals
    /// </summary>
    /// <param name="lightDetector"></param>
    /// <returns></returns>
    private MeshRenderer[] GetCrystalRenderers(Collider lightDetector)
    {
        Transform crystalContainer = lightDetector.transform.GetChild(0);
        MeshRenderer[] crystals = new MeshRenderer[crystalContainer.childCount];
        for (int i = 0; i < crystals.Length; i++)
        {
            crystals[i] = crystalContainer.GetComponent<MeshRenderer>();
        }
        return crystals;
    }

    public override void ChangeLightableState(bool isRevealed)
    {
        base.ChangeLightableState(isRevealed);
        if (isRevealed)
        {
            if(playerLight.GetCurrentMode() == LightMode.BRIDGE)
            {
                PlaySound(npcLightRevealSound);
                for (int i = 0; i < blocks.Count; i++)
                {
                    Coroutine scaleBlock = StartCoroutine(ScaleBlock(blocks[i], blockScale[i]));
                }
            } else
            {
                PlaySound(playerLightRevealSound);
                Coroutine revealBridge = StartCoroutine(ExpandBridge());
            }
        }
        else
        {
            foreach (GameObject block in blocks) block.transform.localScale = Vector3.zero;
            revealable[0].GetComponent<MeshRenderer>().enabled = false;
            foreach (Collider lightDetector in lightDetectors)
            {
                MeshRenderer[] crystals = GetCrystalRenderers(lightDetector);
                foreach(MeshRenderer crystal in crystals) crystal.material.SetColor("_EmissionColor", crystalBaseColor * 1);
            }
            currentIntensity = 1f;
        }
    }

    /// <summary>
    /// Scales a given block in the bridge from 0 to a given size
    /// </summary>
    /// <param name="block">The block to be scaled</param>
    /// <param name="scale">The final scale the block should have</param>
    /// <returns></returns>
    private IEnumerator ScaleBlock(GameObject block, Vector3 scale)
    {
        float currentScaleTime = 0f;
        while (block.transform.localScale.x < scale.x)
        {
            currentScaleTime += Time.deltaTime;
            Vector3 newScale = new Vector3();
            newScale.x = Mathf.Lerp(0f, scale.x, currentScaleTime / blockScaleTime);
            newScale.y = Mathf.Lerp(0f, scale.y, currentScaleTime / blockScaleTime);
            newScale.z = Mathf.Lerp(0f, scale.z, currentScaleTime / blockScaleTime);
            block.transform.localScale = newScale;
            yield return null;
        }
    }

    protected override void StartLighting()
    {
        base.StartLighting();
        if (fade != null) StopCoroutine(fade);
        glow = StartCoroutine(LerpLightIntensity(1, crystalGlowIntensity, requiredTime));
    }

    protected override void StopLighting()
    {
        base.StopLighting();
        if (glow != null) StopCoroutine(glow);
        fade = StartCoroutine(LerpLightIntensity(currentIntensity, 1, cameraZoomOutDuration));
    }

    /// <summary>
    /// Lerps the emissive intensity of the crystals attatched to this lightable bridge
    /// </summary>
    /// <param name="startingIntensity">The intiial emissive intensity of the crystals</param>
    /// <param name="finalIntensity">The desired final intensity of the crystals</param>
    /// <param name="finalGlowTime">How long the lerping should take</param>
    /// <returns></returns>
    private IEnumerator LerpLightIntensity(float startingIntensity, float finalIntensity, float finalGlowTime)
    {

        float currentGlowTime = 0f;
        while(currentIntensity != finalIntensity)
        {
            currentGlowTime += Time.deltaTime;
            currentIntensity = Mathf.Lerp(startingIntensity, finalIntensity, currentGlowTime / finalGlowTime);
            foreach (Collider lightDetector in lightDetectors)
            {
                MeshRenderer[] crystals = GetCrystalRenderers(lightDetector);
                foreach(MeshRenderer crystal in crystals) crystal.material.SetColor("_EmissionColor", crystalBaseColor * currentIntensity);
            }
            yield return null;
        }
    }

    /// <summary>
    /// Expands the light bridge out for when the player lights it
    /// </summary>
    /// <returns></returns>
    private IEnumerator ExpandBridge()
    {
        revealable[0].GetComponent<MeshRenderer>().enabled = true;
        Transform bridge = revealable[0].transform;
        Vector3 bridgeDirection = bridge.rotation * transform.right;
        float currentExpandTime = 0f;
        Vector3 originalScale = bridge.localScale;

        bridge.localScale = new Vector3(CalculateScaleDirection(bridge.localScale.x, bridgeDirection.x),
           bridge.localScale.y, CalculateScaleDirection(bridge.localScale.z, bridgeDirection.z));
        Vector3 startingScale = bridge.localScale;

        bridge.Translate(Vector3.right * originalScale.x / 2, Space.Self);

        while (bridge.localScale.magnitude < originalScale.magnitude)
        {
            currentExpandTime += Time.deltaTime;
            float newX = Mathf.Lerp(startingScale.x, originalScale.x, currentExpandTime / bridgeRevealTime);
            float newY = Mathf.Lerp(startingScale.y, originalScale.y, currentExpandTime / bridgeRevealTime);
            float newZ = Mathf.Lerp(startingScale.z, originalScale.z, currentExpandTime / bridgeRevealTime);
            bridge.localScale = new Vector3(newX, originalScale.y, newZ);
            bridge.Translate(-Vector3.right * originalScale.x / 2 * Time.deltaTime, Space.Self);
            yield return null;
        }
    }

    /// <summary>
    /// Determines the direction in which the light bridge should scale in when revealed
    /// </summary>
    /// <param name="currentScale">The bridge's current scale</param>
    /// <param name="bridgeDirection">The direction of the bridge</param>
    /// <returns>A float representing how much the bridge should scale in a certain direction</returns>
    private float CalculateScaleDirection(float currentScale, float bridgeDirection)
    {
        return currentScale * Convert.ToInt32(!Convert.ToBoolean(Math.Round(bridgeDirection, 5)));
    }

    /// <summary>
    /// Plays a given audio clip with randomized pitch
    /// </summary>
    /// <param name="sound">The clip to play</param>
    private void PlaySound(AudioClip sound)
    {
        audioSource.pitch = Random.Range(0.6f, 1f);
        audioSource.volume = startingVolume;
        audioSource.PlayOneShot(sound);
        StartCoroutine(FadeSound(sound));
    }

    /// <summary>
    /// Fades out a given audio clip
    /// </summary>
    /// <param name="sound">The clip to fade out</param>
    /// <returns></returns>
    private IEnumerator FadeSound(AudioClip sound)
    {
        float currentAudioTime = 0f;
        while(audioSource.volume != 0)
        {
            currentAudioTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startingVolume, 0, currentAudioTime / sound.length);
            yield return null;
        }
    }
}
