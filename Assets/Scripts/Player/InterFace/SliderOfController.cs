using UnityEngine;
using UnityEngine.UI;

public class SliderOfController : MonoBehaviour 
{
    [SerializeField] private Controller controller;

    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        slider.minValue = 0.0f;
        slider.maxValue = controller.MaxValue;
    }

    private void FixedUpdate()
    {
        slider.value =
            controller.Value / controller.MaxValue * slider.maxValue;
    }
}
