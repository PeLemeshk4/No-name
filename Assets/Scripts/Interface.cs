using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public Slider staminaS;

    private void Awake()
    {
        
    }

    public void SetStamina(float value)
    {
        staminaS.value = value;
    }

    public void SetMinMax(float min, float max)
    {
        staminaS.minValue = min;
        staminaS.maxValue = max;
    }

    public void SetBackgroundColor(Color color)
    {

    }
}
