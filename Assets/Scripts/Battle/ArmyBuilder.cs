using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ArmyClash.Battle
{
    public static class ArmyBuilder
    {
        public const int ArmySize = 20;

        public static List<UnitBlueprint> CreateRandomArmy(int? seed = null)
        {
            var random = seed.HasValue ? new System.Random(seed.Value) : new System.Random();
            var units = new List<UnitBlueprint>(ArmySize);

            for (var i = 0; i < ArmySize; i++)
            {
                var shape = (UnitShape)random.Next(Enum.GetValues(typeof(UnitShape)).Length);
                var size = (UnitSize)random.Next(Enum.GetValues(typeof(UnitSize)).Length);
                var color = (UnitColor)random.Next(Enum.GetValues(typeof(UnitColor)).Length);
                units.Add(new UnitBlueprint(shape, size, color));
            }

            return units;
        }

        public static string BuildSummary(IReadOnlyList<UnitBlueprint> army)
        {
            var colorSummary = string.Join(", ", Enum.GetValues(typeof(UnitColor))
                .Cast<UnitColor>()
                .Select(c => $"{c}:{army.Count(u => u.Color == c)}"));

            var shapeSummary = string.Join(", ", Enum.GetValues(typeof(UnitShape))
                .Cast<UnitShape>()
                .Select(s => $"{s}:{army.Count(u => u.Shape == s)}"));

            return $"{colorSummary}\n{shapeSummary}";
        }

        public static GameObject CreateUnitObject(UnitBlueprint blueprint)
        {
            var primitiveType = blueprint.Shape == UnitShape.Cube ? PrimitiveType.Cube : PrimitiveType.Sphere;
            var go = GameObject.CreatePrimitive(primitiveType);
            go.name = $"{blueprint.Color}_{blueprint.Size}_{blueprint.Shape}";

            var scale = blueprint.Size == UnitSize.Big ? 1.4f : 0.8f;
            go.transform.localScale = new Vector3(scale, scale, scale);

            var renderer = go.GetComponent<Renderer>();
            
            var material = UnitStatCalculator.GetUnitMaterial(blueprint.Color);
            if (material != null)
            {
                renderer.material = material;
            }
            else
            {
                renderer.material = new Material(Shader.Find("Standard"));
                renderer.material.color = Color.white;
                Debug.LogWarning($"No material found for color {blueprint.Color}, using default white material");
            }

            return go;
        }
    }
}
