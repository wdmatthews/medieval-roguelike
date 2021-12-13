using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [AddComponentMenu("Medieval Roguelike/Characters/Enemy")]
    public class Enemy : Character
    {
        private Character _target;

        public EnemySO EnemyData => (EnemySO)_data;

        private void ChooseTarget()
        {

        }

        private void FollowTarget()
        {

        }

        private void UseAbilities()
        {

        }
    }
}
