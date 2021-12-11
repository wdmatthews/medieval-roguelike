using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [CreateAssetMenu(fileName = "New Attack", menuName = "Medieval Roguelike/Characters/Attack")]
    public class AttackSO : AbilitySO
    {
        [SerializeField] private AbilityType _type;

        public AbilityType Type => _type;
    }
}
