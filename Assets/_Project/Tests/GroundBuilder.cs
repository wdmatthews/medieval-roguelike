using UnityEngine;

namespace MedievalRoguelike.Tests
{
    public class GroundBuilder
    {
        private Vector2 _position;
        private Vector2 _size;

        public GroundBuilder WithPosition(Vector2 position)
        {
            _position = position;
            return this;
        }

        public GroundBuilder WithSize(Vector2 size)
        {
            _size = size;
            return this;
        }

        public GameObject Build()
        {
            GameObject ground = new GameObject();
            ground.transform.position = _position;
            ground.layer = LayerMask.NameToLayer("Ground");
            BoxCollider2D collider = ground.AddComponent<BoxCollider2D>();
            collider.size = (!Mathf.Approximately(_size.x, 0) && !Mathf.Approximately(_size.y, 0))
                ? _size : new Vector2(1, 1);
            return ground;
        }

        public static implicit operator GameObject(GroundBuilder builder)
        {
            return builder.Build();
        }
    }
}
