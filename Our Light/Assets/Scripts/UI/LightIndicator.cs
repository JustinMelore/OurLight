using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class LightIndicator : MonoBehaviour
{
    [SerializeField] private Image lightIcon;
    [SerializeField] Sprite[] icons = new Sprite[6];


    public void SetLightAmount(int currentLight)
    {
        if (currentLight < 1) currentLight = 1;
        else if (currentLight > 6) currentLight = 6;
        lightIcon.sprite = icons[currentLight - 1];
    }
}
