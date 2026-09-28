using UnityEngine;

public interface IParryable
{
    bool CanBeParried { get; }

    void OnParried(ParryContext context);
}
