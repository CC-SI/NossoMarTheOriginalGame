using UnityEngine;

namespace MiniGame
{
    public class MiniGameObject : MiniGame
    {
        [SerializeField] private Collider objectCollider;
        [SerializeField] private SpriteRenderer sprite;

        private int index;
        
        public Bounds Bounds => objectCollider.bounds;

        public int Index
        {
            get => index;
            set
            {
                index = value;
                transform.SetSiblingIndex(index);
                sprite.sortingOrder = -value;
                var offset = transform.position;
                offset.z = (value - 1) * 0.01f;
                transform.position = offset;
            }
        }
        
        private void Awake()
        {
            Index = GetObjectCount();
            AddObject(this);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            objectCollider = GetComponent<Collider>();
            sprite = GetComponentInChildren<SpriteRenderer>();
        }
#endif
    }
}