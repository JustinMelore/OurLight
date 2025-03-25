using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System;
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
    [SerializeField] private float bridgeRevealTime;

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
        if (isRevealed)
        {
            if(playerLight.GetCurrentMode() == LightMode.BRIDGE)
            {
                for (int i = 0; i < blocks.Count; i++)
                {
                    Coroutine scaleBlock = StartCoroutine(ScaleBlock(blocks[i], blockScale[i]));
                }
            } else
            {
                Coroutine revealBridge = StartCoroutine(ExpandBridge());
            }
        }
        else
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

    private IEnumerator ExpandBridge()
    {
        Transform bridge = revealable[0].transform;
        Vector3 bridgeDirection = bridge.forward;
        float currentExpandTime = 0f;
        Vector3 originalScale = bridge.localScale;

        Debug.Log(bridge.localScale);
        Debug.Log(bridgeDirection);
        bridge.localScale = new Vector3(CalculateScaleDirection(bridge.localScale.x, bridgeDirection.x), 
            CalculateScaleDirection(bridge.localScale.y, bridgeDirection.y), CalculateScaleDirection(bridge.localScale.z, bridgeDirection.z));
        Vector3 startingScale = bridge.localScale;
        Debug.Log(bridge.localScale);

        bridge.position -= new Vector3(bridgeDirection.z, bridgeDirection.y, bridgeDirection.x) * originalScale.magnitude / 2;

        while (bridge.localScale.magnitude < originalScale.magnitude)
        {
            currentExpandTime += Time.deltaTime;
            float newX = Mathf.Lerp(startingScale.x, originalScale.x, currentExpandTime / bridgeRevealTime);
            float newY = Mathf.Lerp(startingScale.y, originalScale.y, currentExpandTime / bridgeRevealTime);
            float newZ = Mathf.Lerp(startingScale.z, originalScale.z, currentExpandTime / bridgeRevealTime);
            bridge.localScale = new Vector3(newX, newY, newZ);
            //bridge.position += bridgeDirection * originalScale.magnitude / 2 * Time.deltaTime;
            bridge.position += new Vector3(bridgeDirection.z, bridgeDirection.y, bridgeDirection.x) * originalScale.magnitude / 2 * Time.deltaTime;
            yield return null;
        }
    }

    private float CalculateScaleDirection(float currentScale, float bridgeDirection)
    {
        //Debug.Log(bridgeDirection);
        //Debug.Log(Convert.ToInt32(!Convert.ToBoolean(Math.Abs(bridgeDirection))));
        return currentScale * Convert.ToInt32(!Convert.ToBoolean(Math.Abs(bridgeDirection)));
    }
}
