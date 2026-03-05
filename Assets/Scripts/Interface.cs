using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    public Slider staminaS;

    private void Awake()
    {
        staminaS.minValue = 0f;
        staminaS.maxValue = 100f;
    }

    public void SetBackgroundColor(Color color)
    {

    }
}
