using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Medieval Roguelike/Characters/Character")]
    public class CharacterSO : ScriptableObject
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _gravity;
        [SerializeField] private bool _canJump;
        [SerializeField] private Vector2 _groundCheckPosition;
        [SerializeField] private Vector2 _groundCheckSize;
        [SerializeField] private AbilitySO[] _abilities;
        [SerializeField] private int _pointValue;

        public float MaxHealth => _maxHealth;
        public float MoveSpeed => _moveSpeed;
        public float JumpHeight => _jumpHeight;
        public float Gravity => _gravity;
        public bool CanJump => _canJump;
        public Vector2 GroundCheckPosition => _groundCheckPosition;
        public Vector2 GroundCheckSize => _groundCheckSize;
        public AbilitySO[] Abilities => _abilities;
        public int PointValue => _pointValue;
    }
}
