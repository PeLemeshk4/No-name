using UnityEngine;

public abstract class Controller : MonoBehaviour 
{
    [SerializeField] protected float maxValue;

    public float MaxValue { get { return maxValue; } }

    protected float value;

    public float Value { get { return value; } }

    private void Awake()
    {
        value = maxValue;
    }

    public abstract bool TryConsume(float amount);

    public void OnTryConsume(float amount)
    {
        TryConsume(amount);
    }
}
