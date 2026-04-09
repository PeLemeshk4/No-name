using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{
    [SerializeField] private float damage;
    [SerializeField] private float length;
    [Range(0.01f, 5f)]
    [SerializeField] private float attackDuration;

    public float Damage {  get { return damage; } }
    public float Length { get { return length; } }
    public float AttackDuration { get { return attackDuration; } }
}
