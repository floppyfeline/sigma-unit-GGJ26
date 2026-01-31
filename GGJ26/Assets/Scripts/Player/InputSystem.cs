using UnityEngine;
using UnityEngine.InputSystem;



    public struct PlayerInputs
    {
        public Quaternion CameraRotation;
        public Vector2 Move;
        public Vector2 Look;
    }

    public class InputSystem : MonoBehaviour
    {
        #region vars
        public InputActions Actions;

        // INPUT ACTIONS
        public InputAction Move { get; private set; }
        public InputAction Jump { get; private set; }
        public InputAction LaunchTongue { get; private set; }
        public InputAction Color1 { get; private set; }
        public InputAction Color2 { get; private set; }
        public InputAction Color3 { get; private set; }
        public InputAction Color4 { get; private set; }
        public InputAction ToggleMenu { get; private set; }

        private PlayerController playerController;
        [SerializeField] private TongueControl tongueControl;

        #endregion
        void Awake()
        {
            Actions = new InputActions();
            Actions.Player.Enable();
            MapInputActions();
            SetCursorLock(true);

            playerController = GetComponent<PlayerController>();
        }
        private void OnDestroy()
        {
            SetCursorLock(false);
        }
        private void MapInputActions()
        {
            if (Actions != null) Actions.Player.Enable();

            // Default Inputs
            Move = Actions.Player.Move;
            Jump = Actions.Player.Jump;
            LaunchTongue = Actions.Player.LaunchTongue;
            Color1 = Actions.Player.Color1;
            Color2 = Actions.Player.Color2;
            Color3 = Actions.Player.Color3;
            Color4 = Actions.Player.Color4;
            ToggleMenu = Actions.Player.Menu;
        }

        private void OnEnable()
        {
            if(Actions != null) Actions.Player.Enable();

            Jump.performed += ctx => playerController.Jump();
            LaunchTongue.performed += ctx => tongueControl.LaunchTongue();
        }
        private void OnDisable()
        {
            if (Actions != null) Actions.Player.Disable();
        }
        public void SetCursorLock(bool cursorLocked)
        {
            if (cursorLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        private void UpdatePlayerInputs()
        {
            PlayerInputs inputs = new PlayerInputs
            {   
                CameraRotation = Camera.main.transform.rotation,
                Move = Move.ReadValue<Vector2>()
            };
            playerController.SetInputs(inputs);
        }

        void Update()
        {
            UpdatePlayerInputs();
        }
    }

