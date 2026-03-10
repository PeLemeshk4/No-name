using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class StaminaSlider : MonoBehaviour
{
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private StaminaController staminaController;

    private void Start()
    {
        staminaSlider.minValue = 0.0f;
        staminaSlider.maxValue = 100.0f;
    }

    private void FixedUpdate()
    {
        staminaSlider.value =
            staminaController.Value / staminaController.MaxValue * staminaSlider.maxValue;
    }
}
