using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [CreateAssetMenu(fileName = "New Attack", menuName = "Medieval Roguelike/Characters/Dodge")]
    public class DodgeSO : AbilitySO
    {
        [SerializeField] private float _dodgeSpeed;
        [SerializeField] private float _cooldownDuration;

        public float DodgeSpeed => _dodgeSpeed;
        public float CooldownDuration => _cooldownDuration;
    }
}
