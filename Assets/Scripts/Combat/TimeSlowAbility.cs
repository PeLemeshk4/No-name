using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeSlowAbility : MonoBehaviour
{
    [SerializeField] private float factor = 0.3f;
    [SerializeField] private float cost = 50.0f;
    [SerializeField] private float thresholdPercent = 0.2f;
    [SerializeField] private StaminaController staminaController;
    
    private bool isActive = false;

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
                else
                {
                    Time.timeScale = factor;
                }
            }
        }
    }

    private void Awake()
    {
        staminaController = GetComponent<StaminaController>();
    }

    private void FixedUpdate()
    {
        if (isActive)
        {           
            if (!staminaController.TryConsume(cost * Time.fixedDeltaTime / factor))
            {
                IsActive = false;
            }
        }
    }
}
