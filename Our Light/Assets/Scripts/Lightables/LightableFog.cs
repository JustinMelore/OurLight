using UnityEngine;

/// <summary>
/// Handles the behavior for the lightable fog near the end of the level
/// </summary>
public class LightableFog : Lightable
{
    [SerializeField] private Collider wall;
    [SerializeField] private ParticleSystem fog;
    private ParticleSystem.MinMaxGradient startingFogColor;

    protected override void Awake()
    {
        base.Awake();
        startingFogColor = fog.main.startColor;
    }

    public override void ChangeLightableState(bool isRevealed)
    {
        base.ChangeLightableState(isRevealed);
        wall.enabled = !isRevealed;
        if (isRevealed) fog.Stop();
        else fog.Play();
    }

    protected override void StartLighting()
    {
        base.StartLighting();
        var main = fog.main;
        main.startColor = new Color(1f, 233f / 255f, 0f);
    }

    protected override void StopLighting()
    {
        base.StopLighting();
        var main = fog.main;
        main.startColor = startingFogColor;
    }



}
