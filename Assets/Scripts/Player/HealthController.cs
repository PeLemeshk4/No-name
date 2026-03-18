using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class HealthController : Controller
{
    public override bool TryConsume(float amount)
    {
        value -= amount;
        if (value <= 0)
        {
            // Метод для смерти (пока перезагрузка сцены)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return false;
        }
        return true;
    }
}
