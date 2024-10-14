using UnityEngine;

namespace Actors
{
    public class GraphicBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [Header("Parametros")]
        [SerializeField] private string _moveParameter;
        [SerializeField] private string _swimParameter;
        [SerializeField] private string _xParameter;
        [SerializeField] private string _yParameter;

        private IMovement movement;
        
        public Vector2 Direction
        {
            get => new(animator.GetFloat(_xParameter), animator.GetFloat(_yParameter));
            set
            {
                if (value.x != 0)
                    animator.SetFloat(_xParameter, value.x);
                
                if (value.x != 0)
                    animator.SetFloat(_yParameter, value.y);
            }
        }

        public bool IsMoving
        {
            get => animator.GetBool(_moveParameter);
            set => animator.SetBool(_moveParameter, value);
        }
        
        public bool IsSwimming
        {
            get => animator.GetBool(_swimParameter);
            set => animator.SetBool(_swimParameter, value);
        }

        private void OnMove(Vector2 velocity, bool isOnWater)
        {
            IsMoving = velocity.sqrMagnitude != 0;
            IsSwimming = isOnWater;
            Direction = velocity.normalized;
        }

        private void Awake()
        {
            movement = GetComponentInParent<IMovement>(true);
            movement?.OnMoved.AddListener(OnMove);
        }

#if UNITY_EDITOR
        private void Reset()
        {
            animator = GetComponentInParent<Animator>(true);
        }
#endif
    }
}