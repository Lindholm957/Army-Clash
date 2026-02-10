using UnityEngine;

namespace ArmyClash.Battle
{
    [CreateAssetMenu(fileName = "ShapeModifier", menuName = "Army Clash/Shape Modifier")]
    public class ShapeModifier : ScriptableObject
    {
        [Header("Shape Type")]
        public UnitShape shape;
        
        [Header("Stat Modifications")]
        public float hpModifier = 0f;
        public float atkModifier = 0f;
        public float speedModifier = 0f;
        public float atkSpdModifier = 0f;
    }
}