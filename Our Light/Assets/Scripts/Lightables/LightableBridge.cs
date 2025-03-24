using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LightableBridge : Lightable
{
    private List<GameObject> blocks;
    private List<Vector3> blockScale;
    private Color crystalBaseColor;
    private float currentIntensity;
    private Coroutine glow;
    private Coroutine fade;
    [SerializeField] private float blockScaleTime;
    [SerializeField] private float crystalGlowIntensity;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        base.Awake();
        specialModes.Add(LightMode.BRIDGE);
        blocks = new List<GameObject>();
        blockScale = new List<Vector3>();
        Transform blocksContainer = transform.Find("Blocks");
        for (int i = 0; i < blocksContainer.childCount; i++)
        {
            blocks.Add(blocksContainer.GetChild(i).gameObject);
            blockScale.Add(blocks[i].transform.localScale);
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
            for (int i = 0; i < blocks.Count; i++)
            {
                Coroutine showDeathScreen = StartCoroutine(ScaleBlock(blocks[i], blockScale[i]));
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
