using UnityEngine;

namespace MedievalRoguelike.Characters
{
    public class Block : Ability
    {
        private BlockSO _blockData;
        private float _blockTimeLeft;
        private float _cooldownTimer;
        private bool _isBlocking;

        public override AbilityType Type => AbilityType.Block;
        public override bool CanUse => _blockTimeLeft > _blockData.MinBlockDuration + float.Epsilon;
        public System.Action CancelBlock { get; set; }

        public override void Initialize(AbilitySO data)
        {
            base.Initialize(data);
            _blockData = (BlockSO)data;
        }

        public override void Use(Character character)
        {
            _isBlocking = true;
            _cooldownTimer = _blockData.CooldownDuration;
            character.StartBlock();
        }

        public override bool CanBeCancelledBy(AbilitySO ability)
        {
            return ability == null || ability is AttackSO || ability is DodgeSO;
        }

        public override void OnUpdate()
        {
            float maxBlockDuration = _blockData.MaxBlockDuration;

            if (_isBlocking)
            {
                _blockTimeLeft = Mathf.Clamp(_blockTimeLeft - Time.deltaTime, 0, maxBlockDuration);
                if (Mathf.Approximately(_blockTimeLeft, 0)) CancelBlock?.Invoke();
            }
            else if (!Mathf.Approximately(_cooldownTimer, 0))
            {
                _cooldownTimer = Mathf.Clamp(_cooldownTimer - Time.deltaTime, 0, _blockData.CooldownDuration);
            }
            else if (!Mathf.Approximately(_blockTimeLeft, maxBlockDuration))
            {
                _blockTimeLeft = Mathf.Clamp(_blockTimeLeft + Time.deltaTime, 0, maxBlockDuration);
            }
        }

        public override void Cancel(Character character)
        {
            _isBlocking = false;
            character.EndBlock();
        }

        public override void Reset()
        {
            _isBlocking = false;
            _blockTimeLeft = _blockData.MaxBlockDuration;
            _cooldownTimer = 0;
        }
    }
}
