using UnityEngine;

namespace ArmyClash.Battle
{
    [CreateAssetMenu(fileName = "BaseStats", menuName = "Army Clash/Base Stats")]
    public class BaseStats : ScriptableObject
    {
        [Header("Base Unit Stats")]
        public float baseHp = 100f;
        public float baseAtk = 10f;
        public float baseSpeed = 10f;
        public float baseAtkSpd = 1f;
    }
}