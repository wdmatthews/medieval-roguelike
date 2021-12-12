using UnityEditor;
using UnityEngine;
using MedievalRoguelike.Characters;

namespace MedievalRoguelike.Tests
{
    public class BlockSOBuilder
    {
        private float _blockPercentage;
        private float _minBlockDuration;
        private float _maxBlockDuration;
        private float _cooldownDuration;

        public BlockSOBuilder WithBlockPercentage(float blockPercentage)
        {
            _blockPercentage = blockPercentage;
            return this;
        }

        public BlockSOBuilder WithMinBlockDuration(float minBlockDuration)
        {
            _minBlockDuration = minBlockDuration;
            return this;
        }

        public BlockSOBuilder WithMaxBlockDuration(float maxBlockDuration)
        {
            _maxBlockDuration = maxBlockDuration;
            return this;
        }

        public BlockSOBuilder WithCooldownDuration(float cooldownDuration)
        {
            _cooldownDuration = cooldownDuration;
            return this;
        }

        public BlockSO Build()
        {
            BlockSO blockData = ScriptableObject.CreateInstance<BlockSO>();

            SerializedObject serializedCharacter = new SerializedObject(blockData);
            serializedCharacter.FindProperty("_blockPercentage")
                .floatValue = _blockPercentage;
            serializedCharacter.FindProperty("_minBlockDuration")
                .floatValue = _minBlockDuration;
            serializedCharacter.FindProperty("_maxBlockDuration")
                .floatValue = _maxBlockDuration;
            serializedCharacter.FindProperty("_cooldownDuration")
                .floatValue = _cooldownDuration;
            serializedCharacter.ApplyModifiedProperties();

            return blockData;
        }

        public static implicit operator BlockSO(BlockSOBuilder builder)
        {
            return builder.Build();
        }
    }
}
