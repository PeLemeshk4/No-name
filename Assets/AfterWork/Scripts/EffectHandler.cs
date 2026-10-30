using UnityEngine;

public class EffectHandler : MonoBehaviour 
{
    private void Awake()
    {
        enabled = false;
    }
    public void Init()
    {
        enabled = true;
    }
}
