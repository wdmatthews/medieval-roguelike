using UnityEditor;
using UnityEngine;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Tests
{
    public class DodgeSOBuilder
    {
        private float _dodgeSpeed;
        private float _cooldownDuration;

        public DodgeSOBuilder WithDodgeSpeed(float dodgeSpeed)
        {
            _dodgeSpeed = dodgeSpeed;
            return this;
        }

        public DodgeSOBuilder WithCooldownDuration(float cooldownDuration)
        {
            _cooldownDuration = cooldownDuration;
            return this;
        }

        public DodgeSO Build()
        {
            DodgeSO dodgeData = ScriptableObject.CreateInstance<DodgeSO>();

            SerializedObject serializedCharacter = new SerializedObject(dodgeData);
            serializedCharacter.FindProperty("_dodgeSpeed")
                .floatValue = _dodgeSpeed;
            serializedCharacter.FindProperty("_cooldownDuration")
                .floatValue = _cooldownDuration;
            serializedCharacter.ApplyModifiedProperties();

            return dodgeData;
        }

        public static implicit operator DodgeSO(DodgeSOBuilder builder)
        {
            return builder.Build();
        }
    }
}
