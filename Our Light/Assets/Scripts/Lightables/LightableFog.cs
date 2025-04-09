using UnityEngine;

public class LightableFog : Lightable
{
    [SerializeField] private Collider wall;
    [SerializeField] private ParticleSystem fog;

    public override void ChangeLightableState(bool isRevealed)
    {
        base.ChangeLightableState(isRevealed);
        wall.enabled = !isRevealed;
        if (isRevealed) fog.Stop();
        else fog.Play();
    }

}
