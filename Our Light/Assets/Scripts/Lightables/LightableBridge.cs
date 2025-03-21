using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightableBridge : Lightable
{
    private List<GameObject> blocks;
    private Color crystalBaseColor;
    private float currentIntensity;
    private Coroutine glow;
    private Coroutine fade;
    [SerializeField] private float blockScale;
    [SerializeField] private float blockScaleTime;
    [SerializeField] private float crystalGlowIntensity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        specialModes.Add(LightMode.BRIDGE);
        blocks = new List<GameObject>();
        Transform blocksContainer = transform.Find("Blocks");
        for (int i = 0; i < blocksContainer.childCount; i++)
        {
            blocks.Add(blocksContainer.GetChild(i).gameObject);
            blocks[i].transform.localScale = Vector3.zero;
        }
        crystalBaseColor = GetCrystalRenderers(lightDetectors[0])[0].material.GetColor("_EmissionColor");
        currentIntensity = 1f;
    }

    private MeshRenderer[] GetCrystalRenderers(Collider lightDetector)
    {
        Transform crystalContainer = lightDetector.transform.GetChild(0).GetChild(0);
        MeshRenderer[] crystals = new MeshRenderer[crystalContainer.childCount];
        for (int i = 0; i < crystals.Length; i++)
        {
            crystals[i] = crystalContainer.GetChild(i).GetComponent<MeshRenderer>();
        }
        return crystals;
    }

    public override void ChangeLightableState(bool isRevealed)
    {
        base.ChangeLightableState(isRevealed);
        if (isRevealed && playerLight.GetCurrentMode() == LightMode.BRIDGE)
        {
            foreach (GameObject block in blocks)
            {
                Coroutine showDeathScreen = StartCoroutine(ScaleBlock(block));
            }
        }
        else if (!isRevealed)
        {
            foreach (GameObject block in blocks) block.transform.localScale = Vector3.zero;
            foreach (Collider lightDetector in lightDetectors)
            {
                MeshRenderer[] crystals = GetCrystalRenderers(lightDetector);
                foreach(MeshRenderer crystal in crystals) crystal.material.SetColor("_EmissionColor", crystalBaseColor * 1);
            }
            currentIntensity = 1f;
        }
    }

    private IEnumerator ScaleBlock(GameObject block)
    {
        float currentScaleTime = 0f;
        while (block.transform.localScale.x < blockScale)
        {
            currentScaleTime += Time.deltaTime;
            float newScale = Mathf.Lerp(0f, blockScale, currentScaleTime / blockScaleTime);
            block.transform.localScale = new Vector3(newScale, newScale, newScale);
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
}
