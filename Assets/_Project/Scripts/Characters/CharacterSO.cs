using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [CreateAssetMenu(fileName = "New Character", menuName = "Medieval Roguelike/Characters/Character")]
    public class CharacterSO : ScriptableObject
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpHeight;
        [SerializeField] private float _gravity;
        [SerializeField] private bool _canJump;
        [SerializeField] private float _groundCheckPosition;
        [SerializeField] private Vector2 _groundCheckSize;

        public float MoveSpeed => _moveSpeed;
        public float JumpHeight => _jumpHeight;
        public float Gravity => _gravity;
        public bool CanJump => _canJump;
        public float GroundCheckDistance => _groundCheckPosition;
        public Vector2 GroundCheckSize => _groundCheckSize;
    }
}
