using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [AddComponentMenu("Medieval Roguelike/Characters/Enemy")]
    public class Enemy : Character
    {
        private Character _target;

        public EnemySO EnemyData => (EnemySO)_data;

        protected override void Update()
        {
            base.Update();

            if (_target)
            {
                FollowTarget();
                UseAbilities();
            }
            else ChooseTarget();
        }

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
