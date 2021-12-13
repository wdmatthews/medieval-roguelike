using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [CreateAssetMenu(fileName = "New Attack", menuName = "Medieval Roguelike/Characters/Attack")]
    public class AttackSO : AbilitySO
    {
        [SerializeField] private AbilityType _type;
        [SerializeField] private float _damage;
        [SerializeField] private Vector2 _hitboxPosition;
        [SerializeField] private Vector2 _hitboxSize;

        public AbilityType Type => _type;
        public float Damage => _damage;
        public Vector2 HitboxPosition => _hitboxPosition;
        public Vector2 HitboxSize => _hitboxSize;
    }
}
