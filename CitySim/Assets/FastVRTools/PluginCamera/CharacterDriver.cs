using System;
using UnityEngine;
using System.Collections;

namespace SIP.FastVRTools.Cameras
{
	[RequireComponent (typeof(CharacterController))]
	[Serializable]
	public class CharacterDriver : MonoBehaviour
	{
		private bool m_canControl = true;

		public bool m_useFixedUpdate = true;

		public Vector3 m_inputMoveDirection = Vector3.zero;

		public bool m_inputJump = false;

		public CharacterDriverMove m_movement = new CharacterDriverMove();

		public CharacterDriverJumping m_jumping = new CharacterDriverJumping();

		public CharacterDriverMovingPlatform m_movingPlatform = new CharacterDriverMovingPlatform();

		public CharacterDriverSliding m_sliding = new CharacterDriverSliding();

		public bool m_grounded = true;

		public Vector3 m_groundNormal = Vector3.zero;

		public Vector3 m_lastGroundNormal = Vector3.zero;

		public Transform m_transform;

		public CharacterController m_controller;

		public void Awake()
		{
			m_controller = transform.GetComponent<CharacterController>();
			m_transform = transform;
		}

		public void Update()
		{
			if(!m_useFixedUpdate)
				CharacterUpdate();
		}

		public void FixedUpdate()
		{
			if(m_movingPlatform.m_enable)
			{
				if(m_movingPlatform.m_activePlatform != null)
				{
					if(!m_movingPlatform.m_newPlatform)
					{
						Vector3 lastVelocity = m_movingPlatform.m_platformVelocity;

						m_movingPlatform.m_platformVelocity = (
							m_movingPlatform.m_activePlatform.localToWorldMatrix.MultiplyPoint3x4(m_movingPlatform.m_activeLocalPoint)
							- m_movingPlatform.m_lastMatrix.MultiplyPoint3x4(m_movingPlatform.m_activeLocalPoint)
							) / Time.deltaTime;
					}
					m_movingPlatform.m_lastMatrix = m_movingPlatform.m_activePlatform.localToWorldMatrix;
					m_movingPlatform.m_newPlatform = false;
				}
				else
				{
					m_movingPlatform.m_platformVelocity = Vector3.zero;
				}
			}

			if(m_useFixedUpdate)
			{
				CharacterUpdate();
			}
		}

		public void CharacterUpdate()
		{
			//Copy the velocity into temp variable so we can multiply.
			Vector3 velocity = m_movement.m_velocity;

			// Update velocity based on input
			velocity = ApplyInputVelocityChange(velocity);

			//Apply gravity and jump force;
			velocity = ApplyGravityAndJumping(velocity);

			// Moving platform support
			Vector3 moveDistance = Vector3.zero;
			if(MoveWithPlatform())
			{
				Vector3 newGlobalPoint = m_movingPlatform.m_activePlatform.TransformPoint(m_movingPlatform.m_activeLocalPoint);
				moveDistance = (newGlobalPoint - m_movingPlatform.m_activeGlobalPoint);
				if(moveDistance != Vector3.zero)
				{
					m_controller.Move(moveDistance);
				}

				// Support moving platform rotation as well
				Quaternion newGlobalRotation = m_movingPlatform.m_activePlatform.rotation * m_movingPlatform.m_activeLocalRotation;
				Quaternion rotationDiff = newGlobalRotation * Quaternion.Inverse(m_movingPlatform.m_activeGlobalRotation);

				float yRotation = rotationDiff.eulerAngles.y;
				if(yRotation != 0)
				{
					m_transform.Rotate(0, yRotation, 0);
				}
			}

			// Save lastPosition for velocity calculation.
			Vector3 lastPosition = transform.position;

			// We always want the movement to be framerate independent.  Multiplying by Time.deltaTime does this.
			Vector3 currentMovementOffset = velocity * Time.deltaTime;

			// Find out how much we need to push towards the ground to avoid loosing grouning
			// when walking down a step or over a sharp change in slope.
			float pushDownOffset = Mathf.Max(m_controller.stepOffset, new Vector3(currentMovementOffset.x, 0, currentMovementOffset.z).magnitude);

			if(m_grounded)
			{
				currentMovementOffset -= pushDownOffset * Vector3.up;
			}

			// Reset variables that will be set by collision function
			m_movingPlatform.m_hitPlatform = null;
			m_groundNormal = Vector3.zero;

			// Move our character!
			m_movement.m_collisionFlags = m_controller.Move(currentMovementOffset);

			m_movement.m_lastHitPoint = m_movement.m_hitPoint;
			m_lastGroundNormal = m_groundNormal;

			if(m_movingPlatform.m_enable && m_movingPlatform.m_activePlatform != m_movingPlatform.m_hitPlatform)
			{
				if(m_movingPlatform.m_hitPlatform != null)
				{
					m_movingPlatform.m_activePlatform = m_movingPlatform.m_hitPlatform;
					m_movingPlatform.m_lastMatrix = m_movingPlatform.m_hitPlatform.localToWorldMatrix;
					m_movingPlatform.m_newPlatform = true;
				}
			}

			// Calculate the velocity based on the current and previous position.  
			// This means our velocity will only be the amount the character actually moved as a result of collisions.
			Vector3 oldHVelocity = new Vector3(velocity.x, 0, velocity.z);
			m_movement.m_velocity = (m_transform.position - lastPosition) / Time.deltaTime;
			Vector3 newHVelocity = new Vector3(m_movement.m_velocity.x, 0, m_movement.m_velocity.z);

			// The CharacterController can be moved in unwanted directions when colliding with things.
			// We want to prevent this from influencing the recorded velocity.
			if(oldHVelocity == Vector3.zero)
			{
				m_movement.m_velocity = new Vector3(0, m_movement.m_velocity.y, 0);
			}
			else
			{
				float projectedNewVelocity = Vector3.Dot(newHVelocity, oldHVelocity) / oldHVelocity.sqrMagnitude;
				m_movement.m_velocity = oldHVelocity * Mathf.Clamp01(projectedNewVelocity) + m_movement.m_velocity.y * Vector3.up;
			}

			if(m_movement.m_velocity.y < velocity.y - 0.001f)
			{
				if(m_movement.m_velocity.y < 0)
				{
					// Something is forcing the CharacterController down faster than it should.
					// Ignore this
					m_movement.m_velocity.y = velocity.y;
				}
				else
				{
					// The upwards movement of the CharacterController has been blocked.
					// This is treated like a ceiling collision - stop further jumping here.
					m_jumping.m_holdingJumpButton = false;
				}
			}

			// We were grounded but just loosed grounding
			if(m_grounded && !IsGroundedTest())
			{
				m_grounded = false;

				// Apply inertia from platform
				if(m_movingPlatform.m_enable && 
				   (m_movingPlatform.m_movementTransfer == MovementTransferOnJump.InitTransfer ||
				 	m_movingPlatform.m_movementTransfer == MovementTransferOnJump.PermaTransfer))
				{
					m_movement.m_frameVelocity = m_movingPlatform.m_platformVelocity;
					m_movement.m_velocity += m_movingPlatform.m_platformVelocity;
				}

				SendMessage("OnFall", SendMessageOptions.DontRequireReceiver);
				// We pushed the character down to ensure it would stay on the ground if there was any.
				// But there wasn't so now we cancel the downwards offset to make the fall smoother.
				m_transform.position += pushDownOffset * Vector3.up;
			}
			else if(!m_grounded && IsGroundedTest())
			{
				m_grounded = true;
				m_jumping.m_jumping = false;
				SubtractNewPlatformVelocity();

				SendMessage("OnLand", SendMessageOptions.DontRequireReceiver);
			}

			// Moving platforms support
			if(MoveWithPlatform())
			{
				// Use the center of the lower half sphere of the capsule as reference point.
				// This works best when the character is standing on moving tilting platforms. 
				m_movingPlatform.m_activeGlobalPoint = m_transform.position + Vector3.up * (m_controller.center.y - m_controller.height * 0.5f + m_controller.radius);
				m_movingPlatform.m_activeLocalPoint = m_movingPlatform.m_activePlatform.InverseTransformPoint(m_movingPlatform.m_activeGlobalPoint);

				// Support moving platform rotation as well
				m_movingPlatform.m_activeGlobalRotation = m_transform.rotation;
				m_movingPlatform.m_activeLocalRotation = Quaternion.Inverse(m_movingPlatform.m_activePlatform.rotation) * m_movingPlatform.m_activeGlobalRotation;
			}

		}

		public Vector3 ApplyInputVelocityChange(Vector3 velocity)
		{
			if(!m_canControl)
				m_inputMoveDirection = Vector3.zero;

			//Find desired velocity
			Vector3 desiredVelocity;

			if(m_grounded && TooSteep())
			{
				// The direction we're sliding in
				desiredVelocity = new Vector3(m_groundNormal.x, 0, m_groundNormal.z).normalized;
				// Find the input movement direction projected onto the sliding direction
				Vector3 projectedMoveDir = Vector3.Project(m_inputMoveDirection, desiredVelocity);
				// Add the sliding direction, the spped control, and the sideways control vectors
				desiredVelocity = desiredVelocity + projectedMoveDir * m_sliding.m_speedControl + (m_inputMoveDirection - projectedMoveDir) * m_sliding.m_sidewayControl;
				// Multiply with the sliding speed
				desiredVelocity *= m_sliding.m_slidingSpeed;
			}
			else
			{
				desiredVelocity = GetDesiredHorizontalVelocity();
			}

			if(m_movingPlatform.m_enable && m_movingPlatform.m_movementTransfer == MovementTransferOnJump.PermaTransfer)
			{
				desiredVelocity += m_movement.m_frameVelocity;
				desiredVelocity.y = 0;
			}

			if(m_grounded)
			{
				desiredVelocity = AdjustGroundVelocityToNormal(desiredVelocity, m_groundNormal);
			}
			else
			{
				velocity.y = 0;
			}

			// The max velocity change
			float maxVelocityChange = GetMaxAcceleration(m_grounded) * Time.deltaTime;
			Vector3 velocityChangeVector = desiredVelocity - velocity;
			if(velocityChangeVector.sqrMagnitude > maxVelocityChange * maxVelocityChange)
			{
				velocityChangeVector = velocityChangeVector.normalized * maxVelocityChange;
			}

			// If we're in the air and don't have control, don't apply any velocity change at all.
			// If we're on the ground and don't have control we do apply it - it will correspond to friction.
			if (m_grounded || m_canControl)
				velocity += velocityChangeVector;

			if (m_grounded) 
			{
				// When going uphill, the CharacterController will automatically move up by the needed amount.
				// Not moving it upwards manually prevent risk of lifting off from the ground.
				// When going downhill, DO move down manually, as gravity is not enough on steep hills.
				velocity.y = Mathf.Min(velocity.y, 0);
			}

			return velocity;
		}

		public Vector3 ApplyGravityAndJumping(Vector3 velocity)
		{
			if(!m_inputJump || !m_canControl)
			{
				m_jumping.m_holdingJumpButton = false;
				m_jumping.m_lastButtonDownTime = -100.0f;
			}

			//Get jumping button down time.
			if(m_inputJump && m_jumping.m_lastButtonDownTime < 0 && m_canControl)
			{
				m_jumping.m_lastButtonDownTime = Time.time;
			}

			//Check if it is on the ground.
			if(m_grounded)
			{
				velocity.y = Mathf.Min(0, velocity.y) - m_movement.m_gravity * Time.deltaTime;
			}
			else
			{
				velocity.y = m_movement.m_velocity.y - m_movement.m_gravity * Time.deltaTime;

				// When jumping up we don't apply gravity for some time when the user is holding the jump button.
				// This gives more control over jump height by pressing the button longer.
				if(m_jumping.m_jumping && m_jumping.m_holdingJumpButton)
				{
					// Calculate the duration that the extra jump force should have effect.
					// If we're still less than that duration after the jumping time, apply the force.
					if(Time.time < m_jumping.m_lastStartTime + m_jumping.m_extraHeight / CalculateJumpVerticalSpeed(m_jumping.m_baseHeight))
					{
						// Negate the gravity we just applied, except we push in jumpDir rather than jump upwards.
						velocity += m_jumping.m_jumpDir * m_movement.m_gravity * Time.deltaTime;
					}
				}

				// Make sure we don't fall any faster than maxFallSpeed. This gives our character a terminal velocity.
				velocity.y = Mathf.Max (velocity.y, -m_movement.m_maxFallSpeed);
			}

			if(m_grounded)
			{
				// Jump only if the jump button was pressed down in the last 0.2 seconds.
				// We use this check instead of checking if it's pressed down right now
				// because players will often try to jump in the exact moment when hitting the ground after a jump
				// and if they hit the button a fraction of a second too soon and no new jump happens as a consequence,
				// it's confusing and it feels like the game is buggy.
				if(m_jumping.m_enable && m_canControl && (Time.time - m_jumping.m_lastButtonDownTime < 0.2f))
				{
					m_grounded = false;
					m_jumping.m_jumping = true;
					m_jumping.m_lastStartTime = Time.time;
					m_jumping.m_lastButtonDownTime = -100.0f;
					m_jumping.m_holdingJumpButton = true;

					// Calculate the jumping direction
					if(TooSteep())
					{
						m_jumping.m_jumpDir = Vector3.Slerp(Vector3.up, m_groundNormal, m_jumping.m_steepPerpAmount);
					}
					else
					{
						m_jumping.m_jumpDir = Vector3.Slerp(Vector3.up, m_groundNormal, m_jumping.m_perpAmount);
					}

					// Apply the jumping force to the velocity. Cancel any vertical velocity first.
					velocity.y = 0;
					velocity += m_jumping.m_jumpDir * CalculateJumpVerticalSpeed(m_jumping.m_baseHeight);

					if(m_movingPlatform.m_enable && 
					   (m_movingPlatform.m_movementTransfer == MovementTransferOnJump.InitTransfer ||
					    m_movingPlatform.m_movementTransfer == MovementTransferOnJump.PermaTransfer))
					{
						m_movement.m_frameVelocity = m_movingPlatform.m_platformVelocity;
						velocity += m_movingPlatform.m_platformVelocity;
					}

					SendMessage("OnJump", SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					m_jumping.m_holdingJumpButton = false;
				}
			}

			return velocity;
		}

		public void OnControllerColliderHit(ControllerColliderHit hit)
		{
			if(hit.normal.y > 0 && hit.normal.y > m_groundNormal.y && hit.moveDirection.y < 0)
			{
				if((hit.point - m_movement.m_lastHitPoint).sqrMagnitude > 0.001 || m_lastGroundNormal == Vector3.zero)
					m_groundNormal = hit.normal;
				else
					m_groundNormal = m_lastGroundNormal;

				m_movingPlatform.m_hitPlatform = hit.collider.transform;
				m_movement.m_hitPoint = hit.point;
				m_movement.m_frameVelocity = Vector3.zero;
			}
		}

		public IEnumerator SubtractNewPlatformVelocity()
		{
			// When landing, subtract the velocity of the new ground from the character's velocity
			// since movement in ground is relative to the movement of the ground.
			if(m_movingPlatform.m_enable &&
			   (m_movingPlatform.m_movementTransfer == MovementTransferOnJump.InitTransfer ||
			 	m_movingPlatform.m_movementTransfer == MovementTransferOnJump.PermaTransfer))
			{
				if(m_movingPlatform.m_newPlatform)
				{
					Transform platform = m_movingPlatform.m_activePlatform;
					yield return new WaitForFixedUpdate();
					yield return new WaitForFixedUpdate();
					if(m_grounded && platform == m_movingPlatform.m_activePlatform)
					{
						yield return 1;
					}
				}
				m_movement.m_velocity -= m_movingPlatform.m_platformVelocity;
			}
		}


		private bool MoveWithPlatform()
		{
			return (m_movingPlatform.m_enable && (m_grounded || m_movingPlatform.m_movementTransfer == MovementTransferOnJump.PermaLocked)
			        && m_movingPlatform.m_activePlatform != null);
		}

		private Vector3 GetDesiredHorizontalVelocity()
		{
			Vector3 desiredLocalDirection = m_transform.InverseTransformDirection(m_inputMoveDirection);
			float maxSpeed = MaxSpeedInDirection(desiredLocalDirection);
			if(m_grounded)
			{
				// Modify max speed on slopes based on slope speed multiplier curve
				float movementSlopeAngle = Mathf.Asin(m_movement.m_velocity.normalized.y) * Mathf.Rad2Deg;
				maxSpeed *= m_movement.m_slopSpeedMultiplier.Evaluate(movementSlopeAngle);
			}
			return m_transform.TransformDirection(desiredLocalDirection * maxSpeed);
		}

		private Vector3 AdjustGroundVelocityToNormal(Vector3 hVelocity, Vector3 groundNormal)
		{
			Vector3 sideways = Vector3.Cross(Vector3.up, hVelocity);
			return Vector3.Cross(sideways, groundNormal).normalized * hVelocity.magnitude;
		}

		private bool IsGroundedTest()
		{
			return (m_groundNormal.y > 0.01f);
		}

		public float GetMaxAcceleration(bool grounded)
		{
			if(grounded)
				return m_movement.m_maxGroundAcceleration;
			else
				return m_movement.m_maxAirAcceleration;
		}


		public float CalculateJumpVerticalSpeed(float targetJumpHeight)
		{
			// From the jump height and gravity we deduce the upwards speed 
			// for the character to reach at the apex.
			return Mathf.Sqrt( 2 * targetJumpHeight * m_movement.m_gravity);
		}

		public bool IsJumping()
		{
			return m_jumping.m_jumping;
		}

		public bool IsSliding()
		{
			return (m_grounded && m_sliding.m_enable && TooSteep());
		}

		public bool IsTouchingCeiling()
		{
			return (m_movement.m_collisionFlags & CollisionFlags.CollidedAbove) != 0;
		}

		public bool IsGrounded()
		{
			return m_grounded;
		}

		public bool TooSteep()
		{
			return (m_groundNormal.y <= Mathf.Cos(m_controller.slopeLimit * Mathf.Deg2Rad));
		}

		public Vector3 GetDirection()
		{
			return m_inputMoveDirection;
		}

		public void SetControllable(bool controllable)
		{
			m_canControl = controllable;
		}


		// Project a direction onto elliptical quater segments based on forward, sideways, and backwards speed.
		// The function returns the length of the resulting vector.
		public float MaxSpeedInDirection(Vector3 desiredMovementDirection)
		{
			if(desiredMovementDirection == Vector3.zero)
			{
				return 0.0f;
			}
			else
			{
				float zAxisEllipseMultiplier = (desiredMovementDirection.z > 0 ? m_movement.m_maxForwardSpeed : m_movement.m_maxBackwardSpeed) / m_movement.m_maxSidewaySpeed;
				Vector3 temp = new Vector3(desiredMovementDirection.x, 0, desiredMovementDirection.z / zAxisEllipseMultiplier).normalized;
				float length = new Vector3(temp.x, 0, temp.z * zAxisEllipseMultiplier).magnitude * m_movement.m_maxSidewaySpeed;
				return length;
			}
		}

		public void SetVelocity(Vector3 velocity)
		{
			m_grounded = false;
			m_movement.m_velocity = velocity;
			m_movement.m_frameVelocity = Vector3.zero;
			SendMessage("OnExternalVelocity");
		}
	}
}
