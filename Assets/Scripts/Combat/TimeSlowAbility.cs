using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeSlowAbility : MonoBehaviour
{
    [SerializeField] private float factor = 0.3f;
    
    private bool isActive = false;

    public bool IsActive
    {
        get 
        { 
            return isActive;
        }
        set
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
