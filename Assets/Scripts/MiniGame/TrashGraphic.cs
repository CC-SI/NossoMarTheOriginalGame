using System;
using UnityEngine;
using UnityEngine.Events;

namespace MiniGame
{
    public class TrashGraphic : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [Header("Parametros")]
        [SerializeField] private string dragParameter;
        [SerializeField] private string trashBinParameter;
        [SerializeField] private string alertTrigger;

        private IDragAndDrop dragAndDrop;

        private void Start()
        {
            IsOnTrashBin = true;
        }

        public bool IsDragging
        {
            get => animator.GetBool(dragParameter);
            set => animator.SetBool(dragParameter, value);
        }
        
        public bool IsOnTrashBin
        {
            get => animator.GetBool(trashBinParameter);
            set => animator.SetBool(trashBinParameter, value);
        }
        
        private void OnInteracted(bool isDragging, bool isOnTrashBin)
        {
            IsDragging = isDragging;
            IsOnTrashBin = isOnTrashBin;
        }
        
        private void AlertSuperImposing()
        {
            animator.SetTrigger(alertTrigger);
        }
        
        private void Awake()
        {
            dragAndDrop = GetComponentInParent<IDragAndDrop>(true);
            dragAndDrop?.OnTouched.AddListener(OnInteracted);
            dragAndDrop?.OnSuperimposed.AddListener(AlertSuperImposing);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            animator = GetComponentInParent<Animator>(true);
        }
#endif
    }
}