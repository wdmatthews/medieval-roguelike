using UnityEngine;

namespace MedievalRoguelike.Characters
{
    public class Attack : Ability
    {
        private AttackSO _attackData;
        private Collider2D[] _opponentColliders;

        public override AbilityType Type => _attackData.Type;

        public override void Initialize(AbilitySO data)
        {
            base.Initialize(data);
            _attackData = (AttackSO)data;
            _opponentColliders = new Collider2D[10];
        }

        public override void Use(Character character)
        {
            character.PlayAttackAnimation(_attackData.Type);
            // TODO REMOVE TEMPORARY
            OnAnimationTick(character);
        }

        public override bool CanBeCancelledBy(AbilitySO ability)
        {
            return ability != null && (ability is DodgeSO || ability is BlockSO);
        }

        public override void OnAnimationTick(Character character)
        {
            Vector2 hitboxPosition = _attackData.HitboxPosition;
            if (Mathf.Approximately(character.transform.eulerAngles.y, 180)) hitboxPosition.x *= -1;
            int opponentCount = Physics2D.OverlapBoxNonAlloc((Vector2)character.transform.position + hitboxPosition,
                _attackData.HitboxSize, 0, _opponentColliders, character.OpponentLayer);

            for (int i = 0; i < opponentCount; i++)
            {
                Collider2D opponentCollider = _opponentColliders[i];
                if (!opponentCollider.isTrigger) continue;
                Character opponent = opponentCollider.GetComponent<Character>();
                bool opponentDied = opponent.TakeDamage(_attackData.Damage);

                if (opponentDied)
                {
                    character.Points += opponent.Data.PointValue;
                    character.Kills++;
                }
            }
        }
    }
}
