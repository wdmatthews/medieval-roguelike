using UnityEditor;
using UnityEngine;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Tests
{
    public class CharacterSOBuilder
    {
        private float _maxHealth;
        private float _moveSpeed;
        private float _jumpHeight;
        private float _gravity;
        private bool _canJump;
        private float _groundCheckPosition;
        private Vector2 _groundCheckSize;

        public CharacterSOBuilder WithMaxHealth(float maxHealth)
        {
            _maxHealth = maxHealth;
            return this;
        }

        public CharacterSOBuilder WithMoveSpeed(float moveSpeed)
        {
            _moveSpeed = moveSpeed;
            return this;
        }

        public CharacterSOBuilder WithJumpHeight(float jumpHeight)
        {
            _jumpHeight = jumpHeight;
            return this;
        }

        public CharacterSOBuilder WithGravity(float gravity)
        {
            _gravity = gravity;
            return this;
        }

        public CharacterSOBuilder WithCanJump(bool canJump)
        {
            _canJump = canJump;
            return this;
        }

        public CharacterSOBuilder WithGroundCheckPosition(float groundCheckPosition)
        {
            _groundCheckPosition = groundCheckPosition;
            return this;
        }

        public CharacterSOBuilder WithGroundCheckSize(Vector2 groundCheckSize)
        {
            _groundCheckSize = groundCheckSize;
            return this;
        }

        public CharacterSO Build()
        {
            CharacterSO characterData = ScriptableObject.CreateInstance<CharacterSO>();

            SerializedObject serializedCharacter = new SerializedObject(characterData);
            serializedCharacter.FindProperty("_maxHealth")
                .floatValue = _maxHealth;
            serializedCharacter.FindProperty("_moveSpeed")
                .floatValue = _moveSpeed;
            serializedCharacter.FindProperty("_jumpHeight")
                .floatValue = _jumpHeight;
            serializedCharacter.FindProperty("_gravity")
                .floatValue = _gravity;
            serializedCharacter.FindProperty("_canJump")
                .boolValue = _canJump;
            serializedCharacter.FindProperty("_groundCheckPosition")
                .floatValue = _groundCheckPosition;
            serializedCharacter.FindProperty("_groundCheckSize")
                .vector2Value = _groundCheckSize;
            serializedCharacter.ApplyModifiedProperties();

            return characterData;
        }

        public static implicit operator CharacterSO(CharacterSOBuilder builder)
        {
            return builder.Build();
        }
    }
}
