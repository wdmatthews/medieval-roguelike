using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [AddComponentMenu("Medieval Roguelike/Characters/Character Animator")]
    public class CharacterAnimator : MonoBehaviour
    {
        private static readonly int _isMovingParameter = Animator.StringToHash("Is Moving");
        private static readonly int _isInAirParameter = Animator.StringToHash("Is In Air");
        private static readonly int _isJumpingParameter = Animator.StringToHash("Is Jumping");
        private static readonly int _attack1Parameter = Animator.StringToHash("Attack 1");
        private static readonly int _attack2Parameter = Animator.StringToHash("Attack 2");
        private static readonly int _attack3Parameter = Animator.StringToHash("Attack 3");
        private static readonly int _isDodgingParameter = Animator.StringToHash("Is Dodging");
        private static readonly int _blockParameter = Animator.StringToHash("Block");
        private static readonly int _isBlockingParameter = Animator.StringToHash("Is Blocking");
        private static readonly int _hitParameter = Animator.StringToHash("Hit");
        private static readonly int _dieParameter = Animator.StringToHash("Die");

        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private Animator _animator;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _hitMaterial;

        public void SetIsMoving(bool isMoving)
        {
            _animator.SetBool(_isMovingParameter, isMoving);
        }

        public void SetIsInAir(bool isInAir)
        {
            _animator.SetBool(_isInAirParameter, isInAir);
        }

        public void SetIsJumping(bool isJumping)
        {
            _animator.SetBool(_isJumpingParameter, isJumping);
        }

        public void TriggerAttack1()
        {
            _animator.ResetTrigger(_attack1Parameter);
            _animator.SetTrigger(_attack1Parameter);
        }

        public void TriggerAttack2()
        {
            _animator.ResetTrigger(_attack2Parameter);
            _animator.SetTrigger(_attack2Parameter);
        }

        public void TriggerAttack3()
        {
            _animator.ResetTrigger(_attack3Parameter);
            _animator.SetTrigger(_attack3Parameter);
        }

        public void SetIsDodging(bool isDodging)
        {
            _animator.SetBool(_isDodgingParameter, isDodging);
        }

        public void SetIsBlocking(bool isBlocking)
        {
            if (isBlocking)
            {
                _animator.ResetTrigger(_blockParameter);
                _animator.SetTrigger(_blockParameter);
            }

            _animator.SetBool(_isBlockingParameter, isBlocking);
        }

        public void TriggerHit()
        {
            _animator.ResetTrigger(_hitParameter);
            _animator.SetTrigger(_hitParameter);
        }

        public void TriggerDie()
        {
            _animator.ResetTrigger(_dieParameter);
            _animator.SetTrigger(_dieParameter);
        }
    }
}
