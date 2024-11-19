using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputsBehaviour : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField]
        InputActionReference _moveAction; 

        [Header("Componentes")]
        [SerializeField]
        FixedJoystick joystick; 

        InputAction MoveAction => _moveAction.action;

        PlayerBehaviour Player => PlayerBehaviour.Instance;

        private string deviceType; 


        private void Update()
        {
            deviceType = PlayerPrefs.GetString("DeviceType", "Pc");

            SetupDeviceInputs();
        }

        void Start()
        {
            deviceType = PlayerPrefs.GetString("DeviceType", "Pc");

            SetupDeviceInputs();
        }
        
        void SetupDeviceInputs()
        {
            if (deviceType == "Mobile")
            {
                joystick.gameObject.SetActive(true);
                joystick.enabled = true;
                if (MoveAction.enabled)
                {
                    MoveAction.Disable(); 
                }
            }
            else
            {
                // Desativa o joystick e habilita a ação do teclado
                joystick.gameObject.SetActive(false);
                MoveAction.Enable(); 
            }
        }

        void Move(Vector2 direction)
        {
            if (!Player)
            {
                return;
            }

            Player.Movement.Move(direction); 
        }
        
        void OnMoveActionPerformed(InputAction.CallbackContext context)
        {
            if (!Player)
            {
                return;
            }

            var direction = context.ReadValue<Vector2>();
            Move(direction); 
        }

        void Awake()
        {
            if (MoveAction != null)
            {
                MoveAction.performed += OnMoveActionPerformed;
                MoveAction.canceled += OnMoveActionPerformed;
            }
        }

        void FixedUpdate()
        {
            if (deviceType == "Mobile")
            {
                Player.Movement.Move(joystick.Direction);
            }
            else if (deviceType == "Pc")
            {
                var direction = MoveAction.ReadValue<Vector2>();
                Move(direction);
            }
        }
        
        void OnDestroy()
        {
            if (MoveAction != null)
            {
                MoveAction.performed -= OnMoveActionPerformed;
                MoveAction.canceled -= OnMoveActionPerformed;
            }
        }
        
        void OnDisable()
        {
            if (MoveAction != null)
            {
                MoveAction.Disable();
            }
        }
        
        void OnEnable()
        {
            if (MoveAction != null)
            {
                MoveAction.Enable();
            }
        }

#if UNITY_EDITOR
        // Método para encontrar o joystick automaticamente no editor
        void Reset()
        {
            joystick = GetComponentInChildren<FixedJoystick>(false);
        }
#endif
    }
}
