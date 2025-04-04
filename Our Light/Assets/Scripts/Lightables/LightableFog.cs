using UnityEngine;

public class LightableFog : Lightable
{
    [SerializeField] private Collider wall;

    public override void ChangeLightableState(bool isRevealed)
    {
        base.ChangeLightableState(isRevealed);
        wall.enabled = !isRevealed;
    }
}
