using UnityEngine;

namespace Actors
{
    public class GraphicBehaviour : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [Header("Parametros")]
        [SerializeField] private string _moveParameter;
        [SerializeField] private string _swimParameter;
        [SerializeField] private string _buriedParameter;
        
        private IMovement movement;
        Vector2 direction;
        
        public Vector2 Direction
        {
            get => direction;
            set
            {
                if(Mathf.Approximately(value.sqrMagnitude, 0))
                    return;
                
                if(!Mathf.Approximately(value.x, 0))
                    transform.localScale = new Vector3(value.x > 0 ? -1 : 1, 1, 1);
                
                direction = value;
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
        
        public bool IsBuried
        {
            get => animator.GetBool(_buriedParameter);
            set => animator.SetBool(_buriedParameter, value);
        }

        void OnMove(Vector2 velocity, bool isOnWater)
        {
            IsMoving = !Mathf.Approximately(velocity.sqrMagnitude, 0);
            IsSwimming = isOnWater;
            Direction = velocity.normalized;
        }
        
        void Awake()
        {
            movement = GetComponentInParent<IMovement>(true);
            movement?.OnMoved.AddListener(OnMove);
        }

#if UNITY_EDITOR
        void Reset()
        {
            animator = GetComponentInParent<Animator>(true);
        }
#endif
    }
}