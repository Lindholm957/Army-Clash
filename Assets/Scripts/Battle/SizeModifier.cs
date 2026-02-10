using UnityEngine;

namespace ArmyClash.Battle
{
    [CreateAssetMenu(fileName = "SizeModifier", menuName = "Army Clash/Size Modifier")]
    public class SizeModifier : ScriptableObject
    {
        [Header("Size Type")]
        public UnitSize size;
        
        [Header("Stat Modifications")]
        public float hpModifier = 0f;
        public float atkModifier = 0f;
        public float speedModifier = 0f;
        public float atkSpdModifier = 0f;
    }
}