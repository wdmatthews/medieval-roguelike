using System.Collections.Generic;
using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [AddComponentMenu("Medieval Roguelike/Characters/Enemy")]
    public class Enemy : Character
    {
        [SerializeField] private PlayerListSO _alivePlayers;

        private EnemySO _enemyData;
        private Dodge _dodge;
        private Block _block;
        private int _attackCount;
        private Player _target;
        private float _retargetTimer;
        private List<Player> _possibleTargets;

        public EnemySO EnemyData => _enemyData ? _enemyData : (EnemySO)_data;

        protected override void Start()
        {
            base.Start();
            _enemyData = (EnemySO)_data;
            _dodge = (Dodge)_abilitiesByType[AbilityType.Dodge];
            _block = (Block)_abilitiesByType[AbilityType.Block];
            _retargetTimer = _enemyData.RetargetCooldown;
            _possibleTargets = new List<Player>();

            foreach (Ability ability in _abilities)
            {
                if (ability is Attack) _attackCount++;
            }
        }

        protected override void Update()
        {
            base.Update();

            if (_target)
            {
                if (_target.IsDead)
                {
                    ChooseTarget();
                    return;
                }

                FollowTarget();
                UseAbilities();

                if (_enemyData.CanRetarget)
                {
                    if (Mathf.Approximately(_retargetTimer, 0)) ChooseTarget();
                    else _retargetTimer = Mathf.Clamp(_retargetTimer - Time.deltaTime, 0, _enemyData.RetargetCooldown);
                }
            }
            else ChooseTarget();
        }

        private void ChooseTarget()
        {
            _retargetTimer = _enemyData.RetargetCooldown;
            _possibleTargets.Clear();
            _possibleTargets.AddRange(_alivePlayers.Players);
            _possibleTargets.Sort(CompareTargetDistances);

            if (_possibleTargets.Count == 0) return;
            _target = _possibleTargets[0];
        }

        private void FollowTarget()
        {
            Vector3 position = transform.position;
            Vector3 targetPosition = _target.transform.position;

            if (Mathf.Abs(position.x - targetPosition.x) > _enemyData.FollowStopDistance)
            {
                if (position.x < targetPosition.x) Move(1);
                else Move(-1);
            }
            else Move(0);

            if (_data.CanJump && targetPosition.y - position.y > _enemyData.DistanceNeededToJump) Jump();
        }

        private void UseAbilities()
        {
            Vector3 position = transform.position;
            Vector3 targetPosition = _target.transform.position;
            float xDistance = Mathf.Abs(position.x - targetPosition.x);

            Ability targetAbility = _target != null ? _target.ActiveAbility : null;
            bool isBeingAttacked = targetAbility != null && targetAbility is Attack;
            bool needsToDefend = xDistance < _enemyData.DistanceNeededToDefend;

            if (isBeingAttacked && needsToDefend && _dodge != null && _dodge.CanUse)
            {
                if (position.x < targetPosition.x) Move(-1);
                else Move(1);
                UseAbility(AbilityType.Dodge);
            }
            else if (isBeingAttacked && needsToDefend && _block != null && _block.CanUse)
            {
                UseAbility(AbilityType.Block);
            }
            else if (xDistance < _enemyData.DistanceNeededToAttack)
            {
                int attackNumber = Random.Range(0, _attackCount);
                if (attackNumber == 0) UseAbility(AbilityType.Attack1);
                else if (attackNumber == 1) UseAbility(AbilityType.Attack2);
                else if (attackNumber == 2) UseAbility(AbilityType.Attack3);
            }
        }

        private int CompareTargetDistances(Player a, Player b)
        {
            return Vector3.Distance(transform.position, a.transform.position)
                .CompareTo(Vector3.Distance(transform.position, b.transform.position));
        }
    }
}
