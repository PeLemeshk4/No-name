using System;
using UnityEngine;

public class LookAbility : MonoBehaviour
{
    public Vector2 Direction { get; private set; } = new Vector2(1, 0);

    public event EventHandler<ValueChangedEventArgs<Vector2>> directionChanged;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void ChangeDirectionLook(Vector2 direction)
    {
        directionChanged?.Invoke(this, new ValueChangedEventArgs<Vector2>(Direction, direction));
        Direction = direction;
    }
}
