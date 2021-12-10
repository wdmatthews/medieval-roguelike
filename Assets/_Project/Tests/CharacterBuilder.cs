using UnityEditor;
using UnityEngine;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Tests
{
    public class CharacterBuilder
    {
        private CharacterSO _data;

        public CharacterBuilder WithData(CharacterSO data)
        {
            _data = data;
            return this;
        }

        public Character Build()
        {
            if (_data == null) _data = A.Default.CharacterSO;
            Character character = new GameObject().AddComponent<Character>();

            SerializedObject serializedCharacter = new SerializedObject(character);
            serializedCharacter.FindProperty("_data")
                .objectReferenceValue = _data;
            serializedCharacter.FindProperty("_rigidbody")
                .objectReferenceValue = character.gameObject.AddComponent<Rigidbody2D>();
            serializedCharacter.FindProperty("_hitbox")
                .objectReferenceValue = character.gameObject.AddComponent<BoxCollider2D>();
            serializedCharacter.FindProperty("_groundLayer")
                .intValue = LayerMask.GetMask("Ground");
            serializedCharacter.ApplyModifiedProperties();

            return character;
        }

        public static implicit operator Character(CharacterBuilder builder)
        {
            return builder.Build();
        }
    }
}
