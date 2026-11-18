using System;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class TagMapPuddles : EntityComponentDefinition
{
    public int PlacesCount;
    public float P;
    public Vector2Int MinMaxLength;
}
