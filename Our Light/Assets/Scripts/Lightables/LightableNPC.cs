using UnityEngine;

public class LightableNPC : Lightable
{
    [SerializeField] private LightMode unlockableMode;

    public override void ChangeLightableState(bool isRevealed)
    {
        base.ChangeLightableState(isRevealed);
        if(isRevealed)
        {
            playerLight.UnlockMode(unlockableMode);
            Destroy(this.gameObject);
        }
    }

}
