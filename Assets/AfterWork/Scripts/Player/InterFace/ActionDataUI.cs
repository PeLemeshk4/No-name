using System;
using TMPro;
using UnityEngine;

public class ActionDataUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro;

    private PlayerActionTracker actionTracker;

    void Awake()
    {
        enabled = false;
    }
    public void Init(PlayerActionTracker aT)
    {
        textMeshPro = GetComponent<TextMeshProUGUI>();

        actionTracker = aT;
        actionTracker.Changed += DataChanged;
        UpdateInterface();

        enabled = true;
    }

    private void DataChanged(object o, EventArgs e)
    {
        UpdateInterface();
    }

    private void UpdateInterface()
    {
        textMeshPro.text = "Total Time: " + actionTracker.TotalTime.ToString() + "\n"
            + "Dash Count: " + actionTracker.DashCount.ToString() + "\n"
            + "Attack Count: " + actionTracker.AttackCount.ToString() + "\n"
            + "Idle Time: " + actionTracker.IdleTime.ToString();

    }
}
