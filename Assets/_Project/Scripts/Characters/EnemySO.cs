using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Medieval Roguelike/Characters/Enemy")]
    public class EnemySO : CharacterSO
    {
        [SerializeField] private int _difficulty;

        public int Difficulty => _difficulty;
    }
}
