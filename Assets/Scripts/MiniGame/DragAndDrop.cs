using System;
using UnityEngine;
using UnityEngine.Events;

namespace MiniGame
{
    public class DragAndDrop : MonoBehaviour, IDragAndDrop
    {
        [SerializeField] private MiniGameObject draggableObject;
        [SerializeField] private AudioSource trashSound;
        public AudioClip dragged, dropped, superimposed;
        [field: SerializeField] public UnityEvent<bool, bool> OnTouched { get; private set; }
        [field: SerializeField] public UnityEvent OnSuperimposed { get; private set; }
        [field: SerializeField] public UnityEvent OnSuperimposing { get; private set; }
        
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

                if (!value) return;
                
                SuperimposingMessage.Show();
                trashSound.PlayOneShot(superimposed);
                OnSuperimposed.Invoke();
                miniGame.AlertSuperimposing(draggableObject.Bounds, draggableObject.Index);
            }
        }
        
        void OnGameFinished()
        {
            enabled = false;
        }

        void OnDestroy()
        {
            MiniGame.OnGameFinished -= OnGameFinished;
        }

        private void Start()
        {
            originalPosition = transform.position;
            MiniGame.OnGameFinished += OnGameFinished;
        }

        private void Update()
        {
            if (!IsDragging) return;
        
            var mousePosition = GetMousePosition();
            mousePosition.z = transform.position.z;
            
            if(!MiniGame.Instance.Limit.bounds.Contains(mousePosition))
                return;

            transform.position = mousePosition;
        }
        
        private void OnMouseDown()
        {
            if(!enabled)
                return;
            
            IsSuperimposed = miniGame.IsObjectSuperimposed(draggableObject.Bounds, draggableObject.Index);

            if (IsSuperimposed) return;
            
            trashSound.PlayOneShot(dragged);
            
            IsDragging = true;
            
            MiniGame.UpdateTrashIndex(draggableObject);
        }
        
        private void OnMouseUp()
        {
            if(!enabled)
                return;
            
            isOnTrashBin = TrashBin.ContainsObject(draggableObject.Bounds);
            
            if(!IsSuperimposed)
                trashSound.PlayOneShot(dropped);
            
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
            trashSound = GetComponent<AudioSource>();
        }
#endif
    }
}