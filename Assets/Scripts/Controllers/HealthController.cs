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
            if (gameObject.tag == "Player")
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        return true;
    }
}
