using UnityEngine;
using System.Collections.Generic;

namespace ArmyClash.Battle
{
    [CreateAssetMenu(fileName = "StatConfiguration", menuName = "Army Clash/Stat Configuration")]
    public class StatConfiguration : ScriptableObject
    {
        [Header("Base Stats")]
        public BaseStats baseStats;
        
        [Header("Shape Modifiers")]
        public List<ShapeModifier> shapeModifiers;
        
        [Header("Size Modifiers")]
        public List<SizeModifier> sizeModifiers;
        
        [Header("Color Modifiers")]
        public List<ColorModifier> colorModifiers;
        
        public float CalculateFinalStat(float baseValue, UnitShape shape, UnitSize size, UnitColor color, 
            System.Func<ShapeModifier, float> shapeFunc, 
            System.Func<SizeModifier, float> sizeFunc, 
            System.Func<ColorModifier, float> colorFunc)
        {
            float result = baseValue;
            
            var shapeMod = shapeModifiers?.Find(m => m.shape == shape);
            if (shapeMod != null)
            {
                result += shapeFunc(shapeMod);
            }
            
            var sizeMod = sizeModifiers?.Find(m => m.size == size);
            if (sizeMod != null)
            {
                result += sizeFunc(sizeMod);
            }
            
            var colorMod = colorModifiers?.Find(m => m.color == color);
            if (colorMod != null)
            {
                result += colorFunc(colorMod);
            }
            
            return result;
        }
        
        public Material GetUnitMaterial(UnitColor color)
        {
            var colorMod = colorModifiers?.Find(m => m.color == color);
            return colorMod?.unitMaterial;
        }
    }
}