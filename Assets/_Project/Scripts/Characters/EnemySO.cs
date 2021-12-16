using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [CreateAssetMenu(fileName = "New Enemy", menuName = "Medieval Roguelike/Characters/Enemy")]
    public class EnemySO : CharacterSO
    {
        [SerializeField] private int _difficulty;
        [SerializeField] private float _followStopDistance;
        [SerializeField] private float _distanceNeededToJump;
        [SerializeField] private bool _canRetarget;
        [SerializeField] private float _retargetCooldown;
        [SerializeField] private float _distanceNeededToDefend;
        [SerializeField] private float _distanceNeededToAttack;

        public int Difficulty => _difficulty;
        public float FollowStopDistance => _followStopDistance;
        public float DistanceNeededToJump => _distanceNeededToJump;
        public bool CanRetarget => _canRetarget;
        public float RetargetCooldown => _retargetCooldown;
        public float DistanceNeededToDefend => _distanceNeededToDefend;
        public float DistanceNeededToAttack => _distanceNeededToAttack;
    }
}
