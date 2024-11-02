using UnityEngine;
using UnityEngine.Events;

namespace MiniGame
{
    public class DraggableObject : ObjectManager
    {
        [SerializeField] private Collider objectCollider;
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Animator animator;
    
        private Vector3 originalPosition;

        //private bool isSupperImposing = false;
        private bool isDragging;
        private int index;

        private static TrashBinBehaviour TrashBin => TrashBinBehaviour.Instance;

        protected override Bounds Bounds => objectCollider.bounds;

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
        
        // public bool IsSupperImposing
        // {
        //     get => isSupperImposing;
        //     set
        //     {
        //         isSupperImposing = value;
        //
        //         if (value)
        //         {
        //             animator.ResetTrigger("PlaySuperImposing"); // Reseta o trigger
        //             animator.SetTrigger("PlaySuperImposing");    // Ativa o trigger
        //         }
        //     }
        // }
        
        private void Awake()
        {
            Index = GetObjectCount();
            AddObject(this);
        }

        private void Start()
        {
            originalPosition = transform.position;
        }
        
        private void Update()
        {
            if (!isDragging) return;
        
            var mousePosition = GetMousePosition();

            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }

        private void OnMouseDown()
        {
            if (IsTrashSuperimposed(Bounds, Index)) return;
        
            isDragging = true;

            RemoveObject(this);
            AddObjectByIndex(0, this);
            
            UpdateIndex();
            
            animator.SetBool("isDragging", isDragging);
        }
        
        private void OnMouseUp()
        {
            isDragging = false;
            animator.SetBool("isDragging", isDragging);
        
            if (TrashBin.ContainsObject(Bounds))
            {
                transform.position = new Vector3(originalPosition.x, originalPosition.y, transform.position.z);
                animator.SetBool("isOnTrashBin", true);
                return;
            }
        
            animator.SetBool("isOnTrashBin", false);
        }

        private static Vector3 GetMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        private void Reset()
        {
            objectCollider = GetComponent<Collider>();
            sprite = GetComponentInChildren<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }
    }
}