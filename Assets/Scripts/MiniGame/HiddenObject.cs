using UnityEngine;
using UnityEngine.Events;

namespace MiniGame
{
    public class HiddenObject : ObjectManager
    {
        [SerializeField] private Collider objectCollider;
        [SerializeField] private SpriteRenderer sprite;
    
        private int index;
        protected override Bounds Bounds => objectCollider.bounds;

        //private bool isObjectCollected;

        protected override int Index
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
    
        private void OnMouseDown()
        {
            if (IsTrashSuperimposed(Bounds, Index)) return;
        
            //isObjectCollected = true;
            
            Debug.Log("Object collected");
            sprite.enabled = false;

            RemoveObject(this);
        }
    
        private void Reset()
        {
            objectCollider = GetComponent<Collider>();
            sprite = GetComponentInChildren<SpriteRenderer>();
        }
    }
}