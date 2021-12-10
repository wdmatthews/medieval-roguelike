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

        public CharacterSO Data => _data;
        public float Health => _health;
        public bool IsDead => _isDead;

        protected void Start()
        {
            _rigidbody.gravityScale = _data.Gravity;
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
        }

        public void Move(float direction)
        {
            if (_isDead) return;
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _data.MoveSpeed;
            _rigidbody.velocity = velocity;
            if (!Mathf.Approximately(direction, 0)) FaceCorrectDirection(direction);
        }

        public void Jump()
        {
            if (_isDead || !_isGrounded || !_data.CanJump) return;
            Vector2 velocity = _rigidbody.velocity;
            _isGrounded = false;
            velocity.y = Mathf.Sqrt(-2 * _data.Gravity * Physics2D.gravity.y * _data.JumpHeight);
            _rigidbody.velocity = velocity;
        }

        public bool TakeDamage(float amount)
        {
            if (_isDead) return true;
            _health = Mathf.Clamp(_health - amount, 0, _data.MaxHealth);
            if (Mathf.Approximately(_health, 0)) Die();
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
            _isGrounded = Mathf.Approximately(_rigidbody.velocity.y, 0)
                && Physics2D.BoxCast(transform.position, _data.GroundCheckSize,
                    0, -transform.up, _data.GroundCheckDistance, _groundLayer);
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
