using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [CreateAssetMenu(fileName = "New Attack", menuName = "Medieval Roguelike/Characters/Block")]
    public class BlockSO : AbilitySO
    {
        [Range(0, 1)] [SerializeField] private float _blockPercentage;
        [SerializeField] private float _minBlockDuration;
        [SerializeField] private float _maxBlockDuration;
        [SerializeField] private float _cooldownDuration;

        public float BlockPercentage => _blockPercentage;
        public float MinBlockDuration => _minBlockDuration;
        public float MaxBlockDuration => _maxBlockDuration;
        public float CooldownDuration => _cooldownDuration;
    }
}
