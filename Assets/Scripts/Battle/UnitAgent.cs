using UnityEngine;
using ArmyClash.Events;

namespace ArmyClash.Battle
{
    public class UnitAgent : MonoBehaviour
    {
        private const float AttackDistance = 1.3f;

        private UnitStats _stats;
        private float _currentHp;
        private float _attackCooldown;
        private BattleArmy _myArmy;
        private BattleArmy _enemyArmy;

        public bool IsDead => _currentHp <= 0f;

        public void Initialize(UnitBlueprint blueprint, UnitStats stats, BattleArmy myArmy, BattleArmy enemyArmy)
        {
            Blueprint = blueprint;
            _stats = stats;
            _currentHp = stats.Hp;
            _myArmy = myArmy;
            _enemyArmy = enemyArmy;
        }

        public UnitBlueprint Blueprint { get; private set; }

        private void Update()
        {
            if (IsDead || _enemyArmy == null || _enemyArmy.AliveCount == 0)
            {
                return;
            }

            _attackCooldown -= Time.deltaTime;

            var target = _enemyArmy.GetClosestAlive(transform.position);
            if (target == null)
            {
                return;
            }

            var toTarget = target.transform.position - transform.position;
            var distance = toTarget.magnitude;

            if (distance > AttackDistance)
            {
                var movement = toTarget.normalized * (_stats.Speed * Time.deltaTime);
                transform.position += movement;
                transform.LookAt(target.transform.position);
                return;
            }

            transform.LookAt(target.transform.position);
            if (_attackCooldown > 0f)
            {
                return;
            }

            _attackCooldown = 1f / _stats.AtkSpd;
            target.ReceiveDamage(_stats.Atk);
        }

        public void ReceiveDamage(float damage)
        {
            if (IsDead)
            {
                return;
            }

            _currentHp -= damage;
            if (_currentHp <= 0f)
            {
                _currentHp = 0f;
                
                Debug.Log($"Юнит погиб: {Blueprint.Color}_{Blueprint.Size}_{Blueprint.Shape}");
                EventBus.Publish(new UnitDiedEvent 
                { 
                    unit = this, 
                    army = _myArmy 
                });
                
                Destroy(gameObject);
            }
        }
    }
}
