using UnityEngine;
using UnityEngine.UI;

public class CircleTimer : MonoBehaviour
{
    [SerializeField] private Image timerImage;

    private float totalTime;
    private float currentTime;
    private bool isRunning = false;


    private void Awake()
    {
        timerImage = GetComponent<Image>();
        timerImage.type = Image.Type.Filled;
        timerImage.fillMethod = Image.FillMethod.Radial360;
        timerImage.fillOrigin = (int)Image.Origin360.Top;
    }

    private void Update()
    {
        if (!isRunning) return;

        currentTime -= Time.deltaTime;
        UpdateVisual();

        if (currentTime <= 0)
        {
            StopTimer();
        }
    }

    private void UpdateVisual()
    {
        float fillAmount = currentTime / totalTime;
        timerImage.fillAmount = fillAmount;
        timerImage.color = Color.Lerp(Color.red, Color.green, fillAmount);
    }

    public void StartTimer(float time)
    {
        totalTime = time;
        currentTime = totalTime;
        isRunning = true;
        gameObject.SetActive(true);
    }

    public void StopTimer()
    {
        gameObject.SetActive(false);
        isRunning = false;
    }
}
