using UnityEngine;

public class LightableNPC : Lightable
{
    [SerializeField] private LightMode unlockableMode;
    private AudioManager audioManager;

    protected override void Awake()
    {
        base.Awake();
        audioManager = FindFirstObjectByType<AudioManager>();
    }

    public override void ChangeLightableState(bool isRevealed)
    {
        base.ChangeLightableState(isRevealed);
        if(isRevealed)
        {
            playerLight.UnlockMode(unlockableMode);
            audioManager.SwitchBackgroundMusic();
            Destroy(this.gameObject);
        }
    }

}
