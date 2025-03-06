using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightableBridge : Lightable
{
    private List<GameObject> blocks;
    [SerializeField] private float blockScale;
    [SerializeField] private float blockScaleTime;

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
}
