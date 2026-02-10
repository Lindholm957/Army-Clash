using UnityEngine;

namespace ArmyClash.Battle
{
    [CreateAssetMenu(fileName = "ColorModifier", menuName = "Army Clash/Color Modifier")]
    public class ColorModifier : ScriptableObject
    {
        [Header("Color Type")]
        public UnitColor color;
        
        [Header("Stat Modifications")]
        public float hpModifier = 0f;
        public float atkModifier = 0f;
        public float speedModifier = 0f;
        public float atkSpdModifier = 0f;
        
        [Header("Visual")]
        public Material unitMaterial;
    }
}