using UnityEngine;
using UnityEngine.UI;

public class LightIndicator : MonoBehaviour
{

    //THIS IS ONLY TEMPORARY. WILL DELETE ONCE PROPER UI ASSET IS MADE
    [SerializeField] private int lightMax;
    private Slider lightSlider;

    void Start()
    {
        lightSlider = GetComponent<Slider>();
    }

    public void SetLightAmount(int currentLight)
    {
        float lightPercentage = (float)currentLight / (float)lightMax;
        lightSlider.value = lightPercentage;
    }
}
