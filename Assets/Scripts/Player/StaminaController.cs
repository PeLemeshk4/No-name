using UnityEngine;

public class StaminaController : MonoBehaviour
{
    [SerializeField] private float maxValue = 100.0f;
    [SerializeField] private float regenRate = 10.0f;

    public float MaxValue { get { return maxValue; }  }

    private float value;
    private bool isRegenActive;

    public float Value { get { return value; } }
    public bool IsRegenActive { get; set; }

    private void Awake()
    {
        value = maxValue;
        isRegenActive = true;
    }

    private void FixedUpdate()
    {
        if (isRegenActive)
        {          
            if (value < maxValue)
            {
                if (value + regenRate * Time.fixedDeltaTime <= maxValue)
                {
                    value += regenRate * Time.fixedDeltaTime;
                }
                else
                {
                    value = maxValue;
                }
            }            
        }
    }

    public bool TryConsume(float amount)
    {
        if (value <= amount) return false;
        value -= amount;
        return true;
    }
}
