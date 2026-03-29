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
    [SerializeField] private GameObject parryZone;

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
                    parryZone.gameObject.SetActive(false);
                }
                else
                {
                    Time.timeScale = factor;
                    parryZone.gameObject.SetActive(true);
                }
            }
        }
    }

    private void Start()
    {
        staminaController = GetComponent<StaminaController>();
        parryZone.gameObject.SetActive(false);      
    }

    private void FixedUpdate()
    {
        if (isActive)
        {           
            if (!staminaController.TryConsume(cost * Time.fixedDeltaTime / factor))
            {
                IsActive = false;
            }
            Vector2 playerPosition = new Vector2(transform.position.x, transform.position.y);
            Vector2 cursorPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (cursorPosition - playerPosition).normalized;
            parryZone.transform.localPosition = direction;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            parryZone.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void TryParry()
    {
        if (!isActive) return;

        List<GameObject> attacks = parryZone.GetComponent<ParryZone>().Attacks;

        for (int i = attacks.Count - 1; i >= 0; i--)
        { 
            GameObject attack = attacks[i];
            if (attack != null)
            {
                attacks.RemoveAt(i);
                Destroy(attack);
            }
        }
    }
}
