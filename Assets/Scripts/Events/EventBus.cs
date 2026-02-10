using UnityEngine;
using System;
using ArmyClash.Battle;

namespace ArmyClash.Events
{
    public interface IGameEvent { }

    public struct UnitDiedEvent : IGameEvent
    {
        public UnitAgent unit;
        public BattleArmy army;
    }

    public struct BattleEndedEvent : IGameEvent
    {
        public string winner;
    }

    public struct ArmyCountChangedEvent : IGameEvent
    {
        public int leftCount;
        public int rightCount;
    }

    public interface IEventHandler<in T> where T : IGameEvent
    {
        void Handle(T eventData);
    }

    public static class EventBus
    {
        private static readonly System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.List<object>> _handlers = 
            new System.Collections.Generic.Dictionary<System.Type, System.Collections.Generic.List<object>>();

        public static void Subscribe<T>(IEventHandler<T> handler) where T : IGameEvent
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type))
            {
                _handlers[type] = new System.Collections.Generic.List<object>();
            }
            _handlers[type].Add(handler);
        }

        public static void Unsubscribe<T>(IEventHandler<T> handler) where T : IGameEvent
        {
            var type = typeof(T);
            if (_handlers.ContainsKey(type))
            {
                _handlers[type].Remove(handler);
            }
        }

        public static void Publish<T>(T eventData) where T : IGameEvent
        {
            var type = typeof(T);
            if (_handlers.ContainsKey(type))
            {
                foreach (var handler in _handlers[type])
                {
                    if (handler is IEventHandler<T> typedHandler)
                    {
                        typedHandler.Handle(eventData);
                    }
                }
            }
        }
    }
}