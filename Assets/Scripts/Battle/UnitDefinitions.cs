using UnityEngine;

namespace ArmyClash.Battle
{
    public enum UnitShape
    {
        Cube,
        Sphere
    }

    public enum UnitSize
    {
        Small,
        Big
    }

    public enum UnitColor
    {
        Blue,
        Green,
        Red
    }

    public readonly struct UnitBlueprint
    {
        public UnitBlueprint(UnitShape shape, UnitSize size, UnitColor color)
        {
            Shape = shape;
            Size = size;
            Color = color;
        }

        public UnitShape Shape { get; }
        public UnitSize Size { get; }
        public UnitColor Color { get; }
    }

    public readonly struct UnitStats
    {
        public UnitStats(float hp, float atk, float speed, float atkSpd)
        {
            Hp = hp;
            Atk = atk;
            Speed = speed;
            AtkSpd = atkSpd;
        }

        public float Hp { get; }
        public float Atk { get; }
        public float Speed { get; }
        public float AtkSpd { get; }
    }

    public readonly struct StatDelta
    {
        public StatDelta(float hp, float atk, float speed, float atkSpd)
        {
            Hp = hp;
            Atk = atk;
            Speed = speed;
            AtkSpd = atkSpd;
        }

        public float Hp { get; }
        public float Atk { get; }
        public float Speed { get; }
        public float AtkSpd { get; }
    }

    public enum GameState
    {
        Menu,
        Simulation,
        Result
    }

    public static class UnitVisuals
    {
        public static readonly Color Blue = new(0.2f, 0.45f, 1f);
        public static readonly Color Green = new(0.2f, 0.8f, 0.3f);
        public static readonly Color Red = new(1f, 0.2f, 0.2f);
    }
}
