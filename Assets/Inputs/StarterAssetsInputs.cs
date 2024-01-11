using UnityEngine;
using UnityEngine.InputSystem;

namespace BestGameEver.Inputs
{
    public sealed class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;

        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool fireLeft;
        public bool fireRight;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;

        public bool cursorInputForLook = true;

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        private void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        private void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        private void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }
        
        private void FireLeftInput(bool newFireLeftState)
        {
            fireLeft = newFireLeftState;
        }
        
        private void FireRightInput(bool newFireRightState)
        {
            fireRight = newFireRightState;
        }

        private static void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook) LookInput(value.Get<Vector2>());
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }
        
        public void OnFireLeft(InputValue value)
        {
            FireLeftInput(value.isPressed);
        }
        
        public void OnFireRight(InputValue value)
        {
            FireRightInput(value.isPressed);
        }
        
#endif
    }
}