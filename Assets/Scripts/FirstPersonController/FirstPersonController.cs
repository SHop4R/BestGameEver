using BestGameEver.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BestGameEver.FirstPersonController
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM
	[RequireComponent(typeof(PlayerInput))]
#endif
	public abstract class FirstPersonController : MonoBehaviour
	{
		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		[SerializeField] private float moveSpeed = 4.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		[SerializeField] private float sprintSpeed = 6.0f;
		[Tooltip("Rotation speed of the character")]
		[SerializeField] private float rotationSpeed = 1.0f;
		[Tooltip("Acceleration and deceleration")]
		[SerializeField] private float speedChangeRate = 10.0f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		[SerializeField] private float jumpHeight = 1.2f;
		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		[SerializeField] private float gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		[SerializeField] private float jumpTimeout = 0.1f;
		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		[SerializeField] private float fallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		[SerializeField] private bool grounded = true;
		[Tooltip("Useful for rough ground")]
		[SerializeField] private float groundedOffset = -0.14f;
		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		[SerializeField] private float groundedRadius = 0.5f;
		[Tooltip("What layers the character uses as ground")]
		[SerializeField] private LayerMask groundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		[SerializeField] protected Transform cinemachineCameraTarget;
		[Tooltip("How far in degrees can you move the camera up")]
		[SerializeField] private float topClamp = 90.0f;
		[Tooltip("How far in degrees can you move the camera down")]
		[SerializeField] private float bottomClamp = -90.0f;

		// cinemachine
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private const float TerminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

	
#if ENABLE_INPUT_SYSTEM
		private PlayerInput _playerInput;
#endif
		private CharacterController _controller;
		protected StarterAssetsInputs StarterInputs;
		private GameObject _mainCamera;

		private const float Threshold = 0.01f;

		private Transform _playerTransform;

		private bool IsCurrentDeviceMouse
		{
			get
			{
#if ENABLE_INPUT_SYSTEM
				return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
			}
		}

		protected virtual void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			_controller = GetComponent<CharacterController>();
			StarterInputs = GetComponent<StarterAssetsInputs>();
			_playerTransform = transform;
#if ENABLE_INPUT_SYSTEM
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			// reset our timeouts on start
			_jumpTimeoutDelta = jumpTimeout;
			_fallTimeoutDelta = fallTimeout;
		}

		protected virtual void Update()
		{
			JumpAndGravity();
			GroundedCheck();
			Move();
		}

		protected virtual void LateUpdate()
		{
			CameraRotation();
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 position = _playerTransform.position;
			Vector3 spherePosition = new(position.x, position.y - groundedOffset, position.z);
			grounded = Physics.CheckSphere(spherePosition, groundedRadius, groundLayers, QueryTriggerInteraction.Ignore);
		}

		private void CameraRotation()
		{
			// if there is an input
			if (!(StarterInputs.look.sqrMagnitude >= Threshold)) return;
			
			//Don't multiply mouse input by Time.deltaTime
			float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;
				
			_cinemachineTargetPitch += StarterInputs.look.y * rotationSpeed * deltaTimeMultiplier;
			_rotationVelocity = StarterInputs.look.x * rotationSpeed * deltaTimeMultiplier;

			// clamp our pitch rotation
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, bottomClamp, topClamp);

			// Update Cinemachine camera target pitch
			cinemachineCameraTarget.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

			// rotate the player left and right
			_playerTransform.Rotate(Vector3.up * _rotationVelocity);
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = StarterInputs.sprint ? sprintSpeed : moveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (StarterInputs.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			Vector3 velocity = _controller.velocity;
			float currentHorizontalSpeed = new Vector3(velocity.x, 0.0f, velocity.z).magnitude;

			const float speedOffset = 0.1f;
			float inputMagnitude = StarterInputs.analogMovement ? StarterInputs.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset || currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude, Time.deltaTime * speedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			// normalise input direction
			Vector3 inputDirection = new Vector3(StarterInputs.move.x, 0.0f, StarterInputs.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (StarterInputs.move != Vector2.zero)
			{
				// move
				inputDirection = _playerTransform.right * StarterInputs.move.x + _playerTransform.forward * StarterInputs.move.y;
			}

			// move the player
			_controller.Move(inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
		}

		private void JumpAndGravity()
		{
			if (grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = fallTimeout;

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (StarterInputs.jump && _jumpTimeoutDelta <= 0.0f)
				{
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = jumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}

				// if we are not grounded, do not jump
				StarterInputs.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < TerminalVelocity)
			{
				_verticalVelocity += gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		protected virtual void OnDrawGizmosSelected()
		{
			Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

			Gizmos.color = grounded ? transparentGreen : transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Vector3 position = transform.position;
			Gizmos.DrawSphere(new(position.x, position.y - groundedOffset, position.z), groundedRadius);
		}
	}
}