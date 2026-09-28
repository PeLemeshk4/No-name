using UnityEngine;

public struct ParryContext
{
    public GameObject Parrier;    
    public Vector2 Direction;     
    public Vector2 Point;         
    public IParryable Target;     

    public ParryContext(GameObject parrier, Vector2 direction, Vector2 point, IParryable target)
    {
        Parrier = parrier;
        Direction = direction;
        Point = point;
        Target = target;
    }
}
