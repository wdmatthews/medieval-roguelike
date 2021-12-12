using UnityEngine;

namespace MedievalRoguelike.Characters
{
    public class Dodge : Ability
    {
        private DodgeSO _dodgeData;
        private float _cooldownTimer;
        private bool _isDodging;

        public override AbilityType Type => AbilityType.Dodge;
        public override bool CanUse => Mathf.Approximately(_cooldownTimer, 0);

        public override void Initialize(AbilitySO data)
        {
            base.Initialize(data);
            _dodgeData = (DodgeSO)data;
        }

        public override void Use(Character character)
        {
            _isDodging = true;
            _cooldownTimer = _dodgeData.CooldownDuration;
            character.StartDodge();
        }

        public override bool CanBeCancelledBy(AbilitySO ability)
        {
            return false;
        }

        public override void OnUpdate()
        {
            if (!_isDodging && Mathf.Approximately(_cooldownTimer, 0)) return;
            _cooldownTimer = Mathf.Clamp(_cooldownTimer - Time.deltaTime, 0, _dodgeData.CooldownDuration);
        }

        public override void OnAnimationEnd(Character character)
        {
            _isDodging = false;
            character.EndDodge();
        }

        public override void Reset()
        {
            _isDodging = false;
            _cooldownTimer = 0;
        }
    }
}
