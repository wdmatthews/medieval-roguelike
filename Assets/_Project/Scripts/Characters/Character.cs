using System.Collections.Generic;
using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [AddComponentMenu("Medieval Roguelike/Characters/Character")]
    public class Character : MonoBehaviour
    {
        [SerializeField] protected CharacterSO _data;
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] protected Collider2D _hitbox;
        [SerializeField] protected CharacterAnimator _animator;
        [SerializeField] protected LayerMask _groundLayer;
        [SerializeField] protected LayerMask _opponentLayer;

        protected bool _isGrounded;
        protected float _health;
        protected bool _isDead;
        protected System.Action<Character> _onDeath;
        protected Ability[] _abilities;
        protected Dictionary<AbilityType, Ability> _abilitiesByType;
        protected Ability _activeAbility;
        protected bool _isDodging;
        protected bool _isBlocking;
        protected DodgeSO _dodgeData;
        protected BlockSO _blockData;

        public CharacterSO Data => _data;
        public LayerMask OpponentLayer => _opponentLayer;
        public float Health => _health;
        public bool IsDead => _isDead;
        public Dictionary<AbilityType, Ability> AbilitiesByType => _abilitiesByType;
        public Ability ActiveAbility => _activeAbility;
        public bool IsDodging => _isDodging;
        public bool IsBlocking => _isBlocking;
        public int Points { get; set; }
        public int Kills { get; set; }

        protected virtual void Start()
        {
            _rigidbody.gravityScale = _data.Gravity;
            int abilityCount = _data.Abilities.Length;
            _abilities = new Ability[abilityCount];
            _abilitiesByType = new Dictionary<AbilityType, Ability>();

            for (int i = 0; i < abilityCount; i++)
            {
                AbilitySO abilityData = _data.Abilities[i];
                AbilityType abilityType = abilityData.GetAbilityType();
                Ability ability = Ability.Create(abilityType, abilityData);
                _abilities[i] = ability;
                _abilitiesByType.Add(abilityType, ability);
            }
        }

        protected virtual void Update()
        {
            if (_isDead) return;
            UpdateAbilities();
        }

        protected void FixedUpdate()
        {
            if (_isDead) return;
            DetectGround();
        }

        public void Spawn(Transform spawnPoint, System.Action<Character> onDeath)
        {
            _health = _data.MaxHealth;
            _onDeath = onDeath;
            Spawn(spawnPoint);
        }

        public void Spawn(Transform spawnPoint)
        {
            if (spawnPoint)
            {
                transform.position = spawnPoint.position;
                transform.eulerAngles = spawnPoint.eulerAngles;
            }
            
            _rigidbody.velocity = new Vector2();
            _hitbox.enabled = true;
            _activeAbility = null;
            _isDodging = false;
            _isBlocking = false;
            ResetAbilities();

            _animator.SetIsMoving(false);
        }

        public void Move(float direction)
        {
            if (_isDead || _isDodging) return;
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _data.MoveSpeed;
            _rigidbody.velocity = velocity;
            bool isMoving = !Mathf.Approximately(direction, 0);
            if (isMoving) FaceCorrectDirection(direction);
            AttemptToCancelActiveAbility();
            _animator.SetIsMoving(isMoving);
        }

        public void Jump()
        {
            if (_isDead || _isDodging || !_isGrounded || !_data.CanJump) return;
            Vector2 velocity = _rigidbody.velocity;
            _isGrounded = false;
            velocity.y = Mathf.Sqrt(-2 * _data.Gravity * Physics2D.gravity.y * _data.JumpHeight);
            _rigidbody.velocity = velocity;
            AttemptToCancelActiveAbility();
            _animator.SetIsJumping(true);
            _animator.SetIsInAir(true);
        }

        public void UseAbility(AbilityType type)
        {
            if (_isDead || _isDodging
                || type == AbilityType.Dodge && !_isGrounded
                || type == AbilityType.Block && !_isGrounded) return;
            Ability ability;
            if (!_abilitiesByType.TryGetValue(type, out ability) || !ability.CanUse) return;
            if (_activeAbility != null && !_activeAbility.CanBeCancelledBy(ability.Data)) return;
            _activeAbility = ability;
            ability.Use(this);
        }

        public void PlayAttackAnimation(AbilityType type)
        {
            if (type == AbilityType.Attack1) _animator.TriggerAttack1();
            else if (type == AbilityType.Attack2) _animator.TriggerAttack2();
            else if (type == AbilityType.Attack3) _animator.TriggerAttack3();
        }

        public void StartDodge()
        {
            _isDodging = true;
            _dodgeData = (DodgeSO)_activeAbility.Data;
            _rigidbody.velocity = _dodgeData.DodgeSpeed * transform.right;
            _hitbox.enabled = false;
            _animator.SetIsDodging(true);
        }

        public virtual void EndDodge()
        {
            _isDodging = false;
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            _hitbox.enabled = true;
            _animator.SetIsDodging(false);
        }

        public void StartBlock()
        {
            _isBlocking = true;
            _blockData = (BlockSO)_activeAbility.Data;
            ((Block)_activeAbility).CancelBlock = CancelBlock;
            _animator.SetIsBlocking(true);
        }

        public void EndBlock()
        {
            _isBlocking = false;
            _animator.SetIsBlocking(false);
        }

        public void CancelBlock()
        {
            if (!_isBlocking) return;
            _activeAbility.Cancel(this);
            _activeAbility = null;
        }

        public void OnAbilityAnimationTick()
        {
            _activeAbility?.OnAnimationTick(this);
        }

        public void OnAbilityAnimationEnd()
        {
            _activeAbility?.OnAnimationEnd(this);
            _activeAbility = null;
        }

        public virtual bool TakeDamage(float amount)
        {
            if (_isDead || _isDodging) return true;
            float actualAmount = _isBlocking ? (1 - _blockData.BlockPercentage) * amount : amount;
            _health = Mathf.Clamp(_health - actualAmount, 0, _data.MaxHealth);
            if (Mathf.Approximately(_health, 0)) Die();
            _animator.TriggerHit();
            return _isDead;
        }

        protected void FaceCorrectDirection(float direction)
        {
            Vector3 eulerAngles = transform.eulerAngles;

            if (direction > 0 && !Mathf.Approximately(eulerAngles.y, 0))
            {
                eulerAngles.y = 0;
                transform.eulerAngles = eulerAngles;
            }
            else if (direction < 0 && !Mathf.Approximately(eulerAngles.y, 180))
            {
                eulerAngles.y = 180;
                transform.eulerAngles = eulerAngles;
            }
        }

        protected void DetectGround()
        {
            bool wasGrounded = _isGrounded;
            _isGrounded = Mathf.Approximately(_rigidbody.velocity.y, 0)
                && Physics2D.OverlapBox((Vector2)transform.position + _data.GroundCheckPosition,
                    _data.GroundCheckSize, 0, _groundLayer);

            if (_isGrounded && !wasGrounded)
            {
                if (_isBlocking) CancelBlock();
                _animator.SetIsJumping(false);
                _animator.SetIsInAir(false);
            }
            else if (!_isGrounded && wasGrounded)
            {
                _animator.SetIsInAir(true);
            }
        }

        protected void UpdateAbilities()
        {
            foreach (Ability ability in _abilities)
            {
                ability.OnUpdate();
            }
        }

        protected void AttemptToCancelActiveAbility()
        {
            if (_activeAbility != null && _activeAbility.CanBeCancelledBy(null))
            {
                _activeAbility.Cancel(this);
                _activeAbility = null;
            }
        }

        protected void ResetAbilities()
        {
            if (_abilities == null) return;

            foreach (Ability ability in _abilities)
            {
                ability.Reset();
            }
        }

        protected void Die()
        {
            _isDead = true;
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            _hitbox.enabled = false;
            _onDeath?.Invoke(this);
            _animator.TriggerDie();
        }
    }
}
