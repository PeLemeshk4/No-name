using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private float damage;
    [SerializeField] private float length;
    [SerializeField] private float width;
    [Range(0.01f, 5f)]
    [SerializeField] private float attackDuration;
    [SerializeField] private Vector3Int attackStages = new(20, 60, 20);

    public float Damage {  get { return damage; } }
    public float Length { get { return length; } }
    public float Width { get { return width; } }
    public float AttackDuration { get { return attackDuration; } }
    public Vector3Int AttackStages { get { return attackStages; } }
}
