using UnityEngine;

public class StaminaController : Controller
{
    [SerializeField] private float regenRate = 10.0f;

    private bool isRegenActive = true;

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
        else
        {
            isRegenActive = true;
        }         
    }

    public override bool TryConsume(float amount)
    {
        if (isEndless) return true;

        isRegenActive = false;
        if (value <= amount) return false;
        value -= amount;
        return true;
    }
}
