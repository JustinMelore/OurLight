using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

/// <summary>
/// Handles behavior for the lantern icon that tracks the player's current light level
/// </summary>
public class LightIndicator : MonoBehaviour
{
    [SerializeField] private Image lightIcon;
    [SerializeField] Sprite[] icons = new Sprite[6];


    /// <summary>
    /// Sets the light amount that the player should be seeing
    /// </summary>
    /// <param name="currentLight">The light level being displayed. This number is expected to be between 1 and 6</param>
    public void SetLightAmount(int currentLight)
    {
        if (currentLight < 1) currentLight = 1;
        else if (currentLight > 6) currentLight = 6;
        lightIcon.sprite = icons[currentLight - 1];
    }
}
