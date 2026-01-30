using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using System;
namespace Klex.Player
{
    public struct PlayerInputs
    {
        public Quaternion CameraRotation;
        public Vector2 Move;
        public Vector2 Look;
    }

    public class KlexInputSystem : MonoBehaviour
    {
        #region vars

        public InputActions Actions;

        // INPUT ACTIONS
        public InputAction Move { get; private set; }
        public InputAction Look { get; private set; }
        public InputAction Interact { get; private set; }
        public InputAction Jump { get; private set; }
        public InputAction Crouch { get; private set; }
        public InputAction Dash { get; private set; }
        public InputAction Attack { get; private set; }
        public InputAction Meow { get; private set; }
        public InputAction Ball { get; private set; }
        public InputAction Sprint { get; private set; }
        public InputAction ToggleMenu { get; private set; }


        public PlayerController Klex;

        public Action OnInputsInitialised;
        #endregion
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            Actions = new InputActions();
            Actions.Player.Enable();
            MapInputActions();
            SetCursorLock(true);
            Klex.Inputs = this;
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
            Look = Actions.Player.Look;
            Interact = Actions.Player.Interact;
            Jump = Actions.Player.Jump;
            Crouch = Actions.Player.Crouch;
            Dash = Actions.Player.Dash;
            Attack = Actions.Player.Attack;
            Meow = Actions.Player.Meow;
            //Ball = Actions.Player.Ball;
            Ball = new InputAction();
            ToggleMenu = Actions.Player.Menu;
            Sprint = Actions.Player.Sprint;

            OnInputsInitialised?.Invoke();
        }

        private void OnEnable()
        {
            if(Actions != null) Actions.Player.Enable();
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
            // Processed in KlexPlayerController
            PlayerInputs inputs = new PlayerInputs
            {   
                CameraRotation = Camera.main.transform.rotation,
                Move = Move.ReadValue<Vector2>(),
                Look = Look.ReadValue<Vector2>()
            };
            Klex.ProcessPlayerInputs(inputs);
        }
      
        void Update()
        {
            if(Klex != null)
            {
                UpdatePlayerInputs();
            }
        }
    }
}
