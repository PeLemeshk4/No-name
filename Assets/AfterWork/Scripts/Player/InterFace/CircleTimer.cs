using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircleTimer : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    private float totalTime;
    private float currentTime;
    private bool isRunning = false;

    public event EventHandler<EventArgs> timerEnded;

    public float CompletePercent
    {
        get
        {
            return currentTime / totalTime;
        }
    }

    private void Awake()
    {
        enabled = false;
    }
    public void Init()
    {
        timerImage = GetComponent<Image>();
        timerImage.type = Image.Type.Filled;
        timerImage.fillMethod = Image.FillMethod.Radial360;
        timerImage.fillOrigin = (int)Image.Origin360.Top;

        gameObject.SetActive(false);

        enabled = true;
    }

    private void Update()
    {
        if (!isRunning) return;

        currentTime += Time.deltaTime / Time.timeScale;
        UpdateVisual();

        if (currentTime >= totalTime)
        {
            currentTime = totalTime;
            StopTimer();
        }
    }

    private void UpdateVisual()
    {
        timerImage.fillAmount = 1 - CompletePercent;
        timerImage.color = Color.Lerp(Color.green, Color.red, CompletePercent);
    }

    public void StartTimer(float time)
    {
        totalTime = time;
        currentTime = 0.0f;
        isRunning = true;
        gameObject.SetActive(true);
    }

    public void StopTimer()
    {
        gameObject.SetActive(false);
        isRunning = false;

        timerEnded?.Invoke(this, new EventArgs());
    }
}
