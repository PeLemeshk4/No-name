using UnityEngine;

public class TimeSlowAbility : MonoBehaviour
{
    [SerializeField] private float factor = 0.3f;
    [SerializeField] private float cost = 50.0f;
    [SerializeField] private float thresholdPercent = 0.2f;
    [SerializeField] private StaminaController staminaController;

    private bool isActive;

    public bool IsActive
    {
        get 
        { 
            return isActive;
        }
        set
        {
            if (value == false || staminaController.Value >= staminaController.MaxValue * thresholdPercent)
            {
                isActive = value;
                if (!isActive)
                {
                    Time.timeScale = 1.0f;
                }
            }
        }
    }

    private void Awake()
    {
        isActive = false;

        staminaController = GetComponent<StaminaController>();
    }

    private void FixedUpdate()
    {
        if (isActive)
        {           
            if (staminaController.TryConsume(cost * Time.fixedDeltaTime / factor))
            {
                Time.timeScale = factor;
            }
            else
            {
                IsActive = false;
            }
        }
    }
}
