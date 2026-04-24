using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TimeSlowAbility : MonoBehaviour
{
    [SerializeField] private CircleTimer timer;
    [SerializeField] private float factor = 0.3f;
    [SerializeField] private float maxSlowingTime = 3.0f;
    
    private bool isActive = false;
    private float slowingTime = 0.0f;

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
                slowingTime = 0.0f;
                timer.StopTimer();
            }
            else
            {
                timer.StartTimer(maxSlowingTime * factor);
                Time.timeScale = factor;
            }
        }
    }

    private void Update()
    {
        if (isActive)
        {
            slowingTime += Time.deltaTime / factor;
            if (slowingTime > maxSlowingTime)
            {
                IsActive = false;
            }
        }
    }
}
