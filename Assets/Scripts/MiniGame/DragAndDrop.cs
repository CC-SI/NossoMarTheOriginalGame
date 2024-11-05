using System;
using MiniGame;
using UnityEngine;
using UnityEngine.Events;

namespace MiniGame
{
    public class DragAndDrop : MonoBehaviour, IDragAndDrop
    {
        [SerializeField] private MiniGameObject draggableObject;
        [field: SerializeField] public UnityEvent<bool, bool> OnTouched { get; private set; }
        [field: SerializeField] public UnityEvent OnSuperimposed { get; private set; }
        
        public Vector3 originalPosition;
        
        private bool isDragging;
        private bool isOnTrashBin;
        private bool isSuperimposed;

        private static TrashBinBehaviour TrashBin => TrashBinBehaviour.Instance;
        private static MiniGame miniGame => MiniGame.Instance;
        
        public bool IsDragging
        {
            get => isDragging;
            private set
            {
                isDragging = value;
                OnTouched.Invoke(isDragging, isOnTrashBin);
            }
        }
        
        public bool IsSuperimposed
        {
            get => isSuperimposed;
            private set
            {
                isSuperimposed = value;

                if (value)
                {
                    miniGame.AlertSuperimposing(draggableObject.Bounds, draggableObject.Index);
                }
            }
        }

        private void Start()
        {
            originalPosition = transform.position;
        }

        private void Update()
        {
            if (!IsDragging) return;
        
            var mousePosition = GetMousePosition();

            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
        
        private void OnMouseDown()
        {
            IsSuperimposed = miniGame.IsObjectSuperimposed(draggableObject.Bounds, draggableObject.Index);
            
            if (IsSuperimposed) return;
        
            IsDragging = true;
            
            MiniGame.UpdateTrashIndex(draggableObject);
        }
        
        private void OnMouseUp()
        {
            isOnTrashBin = TrashBin.ContainsObject(draggableObject.Bounds);
            IsDragging = false;

            if (isOnTrashBin)
                transform.position = new Vector3(originalPosition.x, originalPosition.y, transform.position.z);
            
            miniGame.CheckObjective(null);
        }
        
        private static Vector3 GetMousePosition()
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            draggableObject = GetComponent<MiniGameObject>();
        }
#endif
    }
}