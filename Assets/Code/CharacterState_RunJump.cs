using UnityEngine;

public class CharacterState_RunJump : MonoBehaviour
{

    [Header("Locomotion")]
    [SerializeField] float MaxSpeed;
    [SerializeField] float Acceleration;
    [SerializeField] private AnimationCurve AccelerationFactorFromDot;
    [SerializeField] float MaxAccelForce;
    [SerializeField] private AnimationCurve MaxAccelerationForceFactorFromDot;
    [SerializeField] Vector3 ForceScale;
    [SerializeField] float GravityScaleDrop;


    [Header("Jumping")]
    [SerializeField] private float JumpUpVel;
    [SerializeField] private AnimationCurve JumpUpVelFactorFromExistingY;
    [SerializeField] private AnimationCurve AnalogJumpUpForce;
    [SerializeField] private float JumpTerminalVelocity;
    [SerializeField] private float JumpDuration;
    [SerializeField] private float CoyoteTimeThreshold;
    [SerializeField] private float AutoJumpAfterLandThreshold;
    [SerializeField] private float JumpFallFactor;
    [SerializeField] private float JumpSkipGroundCheckDuration;
    
    [Header("ContextActions")]
    [SerializeField] private float LandingContectThreshold;
    [SerializeField] private float MinLandingContextDuration;


    [SerializeField] private CharacterController _controller;

    //Test
    [SerializeField] private Vector3 m_UnitGoal;
    [SerializeField] private Vector3 m_GoalVel;
    [SerializeField] private float speedFactor;
    [SerializeField] private float maxAccelForceFactor;
    [SerializeField]private Vector3 groundVel; 
    
    [SerializeField] private Rigidbody _RB;

    [SerializeField] private Vector3 move;
    
    [SerializeField] private PlayerMovement _playerMovement;
    
}
