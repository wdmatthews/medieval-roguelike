using UnityEngine;

namespace MedievalRoguelike.Characters
{
    [AddComponentMenu("Medieval Roguelike/Characters/Character")]
    public class Character : MonoBehaviour
    {
        [SerializeField] protected CharacterSO _data;
        [SerializeField] protected Rigidbody2D _rigidbody;
        [SerializeField] protected LayerMask _groundLayer;

        protected bool _isGrounded;

        public CharacterSO Data => _data;

        private void Start()
        {
            _rigidbody.gravityScale = _data.Gravity;
        }

        private void FixedUpdate()
        {
            DetectGround();
        }

        public void Move(float direction)
        {
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _data.MoveSpeed;
            _rigidbody.velocity = velocity;
            if (!Mathf.Approximately(direction, 0)) FaceCorrectDirection(direction);
        }

        public void Jump()
        {
            if (!_data.CanJump || !_isGrounded) return;
            Vector2 velocity = _rigidbody.velocity;
            _isGrounded = false;
            velocity.y = Mathf.Sqrt(-2 * _data.Gravity * Physics2D.gravity.y * _data.JumpHeight);
            _rigidbody.velocity = velocity;
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
    }
}
