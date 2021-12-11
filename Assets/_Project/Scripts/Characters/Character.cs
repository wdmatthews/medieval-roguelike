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
        [SerializeField] protected LayerMask _groundLayer;

        protected bool _isGrounded;
        protected float _health;
        protected bool _isDead;
        protected System.Action _onDeath;
        protected Ability[] _abilities;
        protected Dictionary<AbilityType, Ability> _abilitiesByType;
        protected Ability _activeAbility;

        public CharacterSO Data => _data;
        public float Health => _health;
        public bool IsDead => _isDead;
        public Ability ActiveAbility => _activeAbility;

        protected void Start()
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

        protected void Update()
        {
            if (_isDead) return;
            UpdateAbilities();
        }

        protected void FixedUpdate()
        {
            if (_isDead) return;
            DetectGround();
        }

        public void Spawn(Transform spawnPoint, System.Action onDeath)
        {
            transform.position = spawnPoint.position;
            transform.eulerAngles = spawnPoint.eulerAngles;
            _health = _data.MaxHealth;
            _onDeath = onDeath;
            _rigidbody.velocity = new Vector2();
            _hitbox.enabled = true;
            ResetAbilities();
        }

        public void Move(float direction)
        {
            if (_isDead) return;
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _data.MoveSpeed;
            _rigidbody.velocity = velocity;
            if (!Mathf.Approximately(direction, 0)) FaceCorrectDirection(direction);
            AttemptToCancelActiveAbility();
        }

        public void Jump()
        {
            if (_isDead || !_isGrounded || !_data.CanJump) return;
            Vector2 velocity = _rigidbody.velocity;
            _isGrounded = false;
            velocity.y = Mathf.Sqrt(-2 * _data.Gravity * Physics2D.gravity.y * _data.JumpHeight);
            _rigidbody.velocity = velocity;
            AttemptToCancelActiveAbility();
        }

        public void UseAbility(AbilityType type)
        {
            if (!_isDead) return;
            Ability ability;
            if (!_abilitiesByType.TryGetValue(type, out ability) || !ability.CanUse) return;
            if (_activeAbility != null && !_activeAbility.CanBeCancelledBy(ability.Data)) return;
            _activeAbility = ability;
            ability.Use(this);
        }

        public bool TakeDamage(float amount)
        {
            if (_isDead) return true;
            _health = Mathf.Clamp(_health - amount, 0, _data.MaxHealth);
            if (Mathf.Approximately(_health, 0)) Die();
            return _isDead;
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
            _isGrounded = Mathf.Approximately(_rigidbody.velocity.y, 0)
                && Physics2D.BoxCast(transform.position, _data.GroundCheckSize,
                    0, -transform.up, _data.GroundCheckDistance, _groundLayer);
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
            if (_activeAbility != null && _activeAbility.CanBeCancelledBy(null)) _activeAbility.Cancel(this);
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
            _onDeath?.Invoke();
        }
    }
}
