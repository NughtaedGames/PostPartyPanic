using System;
using System.Collections;
using System.Collections.Generic;
using GameEvents;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.VFX;

public class PlayerMovement : StateMachine
{
    
    private Vector3 startPosition;
    private Quaternion startRotation;
    
    [SerializeField] private Vector3 centerOfMass;
    
    // Start is called before the first frame update
    [SerializeField] public Rigidbody _RB;
    
    [SerializeField] public float RideHeight;
    [SerializeField] public float RideSpringStrength;
    [SerializeField] public float RideSpringDamper;
    
    [SerializeField] public Vector3 DownDir;
    
    [SerializeField] public Quaternion _uprightJointTargetRot;
    [SerializeField] public float _uprightJointSpringStrength;
    [SerializeField] public float _uprightJointSpringDamper;

    public float maxAngularVelocity;
    
    public bool lookDirectionoverride;
    public bool isCarrying;

    public VisualEffect footstepsDust;
    public VisualEffect jumpDust;
    
    [Header("Raycast Setup")]
    [SerializeField] public float MaxRayDist;
    //[SerializeField] private Mask RayMask;
    [SerializeField] public float IsGroundThreshold;
    
    
    //public List<GameObject> collidingObjects;
    public List<MovableRigidbody> collidingObjects;
    

    [Header("Locomotion")]
    public float MaxSpeed;
    public float Acceleration;
    public AnimationCurve AccelerationFactorFromDot;
    public float MaxAccelForce;
    public  AnimationCurve MaxAccelerationForceFactorFromDot;
    public Vector3 ForceScale;
    public float GravityScaleDrop;
    public float JumpForce;

    
    
    // [Header("Jumping")]
    // public float JumpUpVel;
    // public AnimationCurve JumpUpVelFactorFromExistingY;
    // public AnimationCurve AnalogJumpUpForce;
    // public float JumpTerminalVelocity;
    // public float JumpDuration;
    // public float CoyoteTimeThreshold;
    // public float AutoJumpAfterLandThreshold;
    // public float JumpFallFactor;
    // public float JumpSkipGroundCheckDuration;
    //
    // [Header("ContextActions")]
    // public float LandingContectThreshold;
    // public float MinLandingContextDuration;
    //
    //
    // public CharacterController _controller;

    //Test
    public Vector3 m_UnitGoal;
    public Vector3 m_GoalVel;
    public float speedFactor;
    public float maxAccelForceFactor;
    public Vector3 groundVel;

    public Vector3 move;

    public Vector3 carrySpot;
    public Transform vacuumCleanerCarryHandle;
    public Transform liftedVacuumCleanerCarryHandle;
    public int carriableMass;

     private InputMaster controls;

     public GameObject originalHands;
     
     public Transform rightHand;
     public Transform leftHand;

     public Transform rightHandSplineNode;
     public Transform leftHandSplineNode;

     public Transform rightHandOriginalNodePos;
     public Transform leftHandOriginalNodePos;

     public Animator anim;
     public Transform costumeList;
     [SerializeField]
     private CostumeListInstance costumeListInstance;
    
     private void Awake()
    {
        GameEventManager.AddListener<RestartGame>(RemovePlayer);
        GameEventManager.Raise(new PlayerJoined(this.transform));
        
        startPosition = this.transform.position;
        startRotation = this.transform.rotation;
        //controls = new InputMaster();
        //controls.Player.Interact.performed += ctx => EInteract();
        //controls.Player.Movement.performed += ctx => Move(ctx.ReadValue<Vector2>());
    }

    // private void OnDestroy()
    // {
    //     controls.Player.Interact.performed -= ctx => EInteract();
    //     controls.Player.Movement.performed -= ctx => Move(ctx.ReadValue<Vector2>());
    // }

    private void RemovePlayer(RestartGame e)
    {
        Destroy(this.gameObject);
    }

    private void OnEnable()
    {
        //controls.Enable();
        SetState(new RunningState(this));
    }

    private void OnDisable()
    {
        //controls.Disable();
    }

    private void OnDestroy()
    {
        //this.GetComponent<PlayerInput>().notificationBehavior = PlayerNotifications.SendMessages;
    }

    private void Start()
    {
        _RB.centerOfMass = centerOfMass;
        
        SetState(new RunningState(this));
    }

    private void Update()
    {
        //HandleButtonInputs();
        StartCoroutine(GetState().Update());


        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.LogError("TETETETTSTSTTST");
            GameEventManager.Raise(new HighlightObjects());
        }

        //float vertical = Input.GetAxis("Vertical");
        //float horizontal = Input.GetAxis("Horizontal");

        //move = new Vector3(horizontal, 0f, vertical);

    }
    

    void FixedUpdate()
    {

        //_uprightJointTargetRot.y = Camera.main.transform.rotation.y;

        StartCoroutine(State.FixedUpdate());
    }

    public void SetUprightJointTargetRot(float targetrotation)
    {
        //Debug.LogError("Rot:" + targetrotation);
        if (!lookDirectionoverride)
        {
            _uprightJointTargetRot = Quaternion.Euler(0,targetrotation,0);
        }
    }

    public void SetToCurrentUprightJointTargetRot()
    {
        _uprightJointTargetRot = Quaternion.Euler(0, transform.eulerAngles.y, 0);;
    }
    

    private void HandleButtonInputs()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{

        //}
    }

    // private void EInteract()
    // {
    //     Debug.LogError("Test2");
    //     StartCoroutine(GetState().EInteract());
    // }
    
    // private void Move(Vector2 direction)
    // {
    //     Debug.LogError("Moving" + direction);
    //     move = new Vector3(direction.x, 0f, direction.y);
    //     move = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * move;
    // }
    
    public void Move(InputAction.CallbackContext context)
    {
        if (gameObject.scene.IsValid())
        {
            //Debug.LogError("STATE: " + GetState());
            
            move = new Vector3(context.ReadValue<Vector2>().x , 0, context.ReadValue<Vector2>().y) ;
            if (Camera.main)
            {
                move = Quaternion.AngleAxis(Camera.main.transform.rotation.eulerAngles.y, Vector3.up) * move;
            }
            
        }

    }

    public void EInteract(InputAction.CallbackContext context)
    {
        if (context.action.triggered && gameObject.scene.IsValid())
        {
            Debug.Log("INTERACT");
            StartCoroutine(GetState().EInteract());
        }
    }
    
    public void Use(InputAction.CallbackContext context)
    {
        if (context.action.triggered && gameObject.scene.IsValid())
        {
            Debug.Log("USE");
            StartCoroutine(GetState().StartUse());
        }
        
        // if (context.started && gameObject.scene.IsValid())
        // {
        //     Debug.LogError("Started");
        // }

        if (context.canceled && gameObject.scene.IsValid())
        {
            Debug.LogError("Cancled");
            StartCoroutine(GetState().StopUse());
        }
        
        // if (context.performed && gameObject.scene.IsValid())
        // {
        //     Debug.LogError("performed");
        // }
    }
    
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.action.triggered && gameObject.scene.IsValid())
        {
            GetState().Jump();
        }
    }

    public void SwitchPauseMenu(InputAction.CallbackContext context)
    {
        if (context.action.triggered && gameObject.scene.IsValid())
        {
            GameEventManager.Raise(new SwitchPauseMenu());
        }
    }

    public static JointDrive NewJointDrive(float force, float damping)
    {
        JointDrive drive = new JointDrive();
        drive.mode = JointDriveMode.Position;
        drive.positionSpring = force;
        drive.positionDamper = damping;
        drive.maximumForce = Mathf.Infinity;
        return drive;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        if (other.gameObject.GetComponent<MovableRigidbody>() is MovableRigidbody mrb)
        {
            collidingObjects.Add(mrb);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (collidingObjects.Contains(other.gameObject.GetComponent<MovableRigidbody>()))
        {
            collidingObjects.Remove(other.gameObject.GetComponent<MovableRigidbody>());
            other.gameObject.GetComponent<MovableRigidbody>().DeactivateOutline();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere( transform.TransformPoint(centerOfMass) , 0.2f);
        Gizmos.DrawSphere( transform.TransformPoint(carrySpot) , 0.2f);
        // Draw a yellow sphere at the transform's position

    }

    public void SetPlayerCostume(int index)
    {

        // for (int i = 0; i < costumeList.childCount; i++)
        // {
        //     if (i != index)
        //     {
        //         costumeList.GetChild(i).gameObject.SetActive(false);
        //     }
        //     else
        //     {
        //         costumeList.GetChild(i).gameObject.SetActive(true);
        //     }
        // }
        //
        for (int i = 0; i < costumeList.childCount; i++)
        {
            if (costumeList.GetChild(i).name == costumeListInstance.value.costumeList[index].modelNames)
            {
                costumeList.GetChild(i).gameObject.SetActive(true);
            }
            else
            {
                costumeList.GetChild(i).gameObject.SetActive(false);
            }
        }
        
    }

    public void ResetObject()
    {
        _RB.isKinematic = true;
        transform.position = startPosition + new Vector3(0,2,0);
        transform.rotation = startRotation;
        _RB.isKinematic = false;
    }
    
    // private void OnLevelUnloading(Scene scene)
    // {
    //     Destroy(this.gameObject);
    // }
    //
    //
    // void OnLevelFinishedLoading (Scene scene, LoadSceneMode newScene)
    // {
    //     if (scene.buildIndex == 0)
    //     {
    //         Debug.LogError("DELETE");
    //         Destroy(this.gameObject);
    //     }
    // }
}
