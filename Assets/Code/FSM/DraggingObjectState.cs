using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Unity.Mathematics;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering;

public class DraggingObjectState : State
{
    private GameObject closestGameObject;
    private ConfigurableJoint confJoint;
    
    public DraggingObjectState(PlayerMovement playerMovement, GameObject closestGameObject) : base(playerMovement)
    {
        this.closestGameObject = closestGameObject;
    }

    public List<Transform> availableContactPoints;
    public override IEnumerator Start()
    {
        PlayerMovement.anim.Play("pull");
        Debug.Log(closestGameObject.name + " dragged around");

        DragRigidbody drb = closestGameObject.GetComponent<DragRigidbody>();
        drb.ActivateOutline();

        var joint = PlayerMovement.gameObject.AddComponent<ConfigurableJoint>();
        confJoint = joint;
        float force = 2500;
        float damping = 6;

        availableContactPoints = drb.contactPoints;

        availableContactPoints = availableContactPoints.OrderBy(
            x => Vector3.Distance(PlayerMovement.transform.position,x.transform.position)
        ).ToList();

        Debug.Log("positions1: " + PlayerMovement.rightHand.transform.position);
        Debug.Log("positions2: " + availableContactPoints[1].position);


        //Positioning hands
        
        PlayerMovement.rightHand.gameObject.SetActive(true);
        PlayerMovement.leftHand.gameObject.SetActive(true);
        PlayerMovement.originalHands.SetActive(false);

        PlayerMovement.rightHandSplineNode.SetParent(PlayerMovement.rightHand);
        PlayerMovement.leftHandSplineNode.SetParent(PlayerMovement.leftHand);
        PlayerMovement.rightHandSplineNode.position = PlayerMovement.rightHand.position;
        PlayerMovement.leftHandSplineNode.position = PlayerMovement.leftHand.position;
        
        PlayerMovement.rightHandOriginalNodePos.gameObject.SetActive(false);
        PlayerMovement.leftHandOriginalNodePos.gameObject.SetActive(false);
        
        closestGameObject.GetComponentInParent<Rigidbody>().maxAngularVelocity = 0.1f;
        PlayerMovement._RB.maxAngularVelocity = 0.1f;
        PlayerMovement._RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

        joint.connectedBody = closestGameObject.GetComponent<Rigidbody>();
        joint.configuredInWorldSpace = true;
        joint.xDrive = PlayerMovement.NewJointDrive(force, damping);
        joint.yDrive = PlayerMovement.NewJointDrive(force, damping);
        joint.zDrive = PlayerMovement.NewJointDrive(force, damping);
        joint.slerpDrive = PlayerMovement.NewJointDrive(force, damping);
        joint.rotationDriveMode = RotationDriveMode.Slerp;
        joint.enableCollision = true;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;


        //SoftJointLimit sjl = new SoftJointLimit();
        //Need to text these two
        
        SoftJointLimit softJointLimit = new SoftJointLimit();
        softJointLimit.limit = 1.5f;
        joint.linearLimit = softJointLimit;
        //
        // SoftJointLimit softJointSpringLimit = new SoftJointLimit();
        // softJointSpringLimit.spring = 1f;
        // joint.linearLimitSpring = softJointSpringLimit.spring;
        //joint.angularXMotion = ConfigurableJointMotion.Locked;
        //joint.angularYMotion = ConfigurableJointMotion.Locked;
        //joint.angularZMotion = ConfigurableJointMotion.Locked;
        
        drb.StartBeingDragged(PlayerMovement);
        
        yield break;
    }

    public override IEnumerator Update()
    {
        
        Vector3 lookingDirectionVector = closestGameObject.transform.position - PlayerMovement.transform.position;
        float lookingDirectionDegrees = math.atan2(lookingDirectionVector.x, lookingDirectionVector.y) * Mathf.Rad2Deg;
            
            
        PlayerMovement.SetUprightJointTargetRot(lookingDirectionDegrees);
        
        PlayerMovement.rightHand.position = availableContactPoints[0].position;
        PlayerMovement.leftHand.position = availableContactPoints[1].position;
        
        PlayerMovement.rightHandSplineNode.position = PlayerMovement.rightHand.position; // new Vector3(0, 0, 0);
        PlayerMovement.leftHandSplineNode.position = PlayerMovement.leftHand.position; // new Vector3(0, 0, 0);

        //PlayerMovement.grabbingRightHand.position = availableContactPoints[0].position;
        //PlayerMovement.grabbingLeftHand.position = availableContactPoints[1].position;
        //PlayerMovement.rightHand.transform.position = availableContactPoints[0].position;
        //PlayerMovement.leftHand.transform.position = availableContactPoints[1].position;
        return base.Update();
    }

    public override IEnumerator EInteract()
    {

        //PlayerMovement.rightHandSplineNode.position = PlayerMovement.rightHand.position; // new Vector3(0, 0, 0);

        //PlayerMovement.leftHandSplineNode.position = PlayerMovement.leftHand.position;
        
        PlayerMovement.rightHand.GetComponentInParent<TwoBoneIKConstraint>().weight = 0;
        PlayerMovement.leftHand.GetComponentInParent<TwoBoneIKConstraint>().weight = 0;

        PlayerMovement.rightHand.gameObject.SetActive(false);
        PlayerMovement.leftHand.gameObject.SetActive(false);
        PlayerMovement.originalHands.SetActive(true);
        
        PlayerMovement.rightHandOriginalNodePos.gameObject.SetActive(true);
        PlayerMovement.leftHandOriginalNodePos.gameObject.SetActive(true);
        PlayerMovement.rightHandSplineNode.SetParent(PlayerMovement.rightHandOriginalNodePos);
        PlayerMovement.leftHandSplineNode.SetParent(PlayerMovement.leftHandOriginalNodePos);
        PlayerMovement.rightHandSplineNode.position = PlayerMovement.rightHandOriginalNodePos.position;
        PlayerMovement.leftHandSplineNode.position = PlayerMovement.leftHandOriginalNodePos.position;

        
        DragRigidbody drb = closestGameObject.GetComponent<DragRigidbody>();
        drb.DeactivateOutline();
        drb.StopBeingDragged();
        drb.DeactivateSnappingObject();

        //PlayerMovement.Destroy(PlayerMovement.gameObject.GetComponent<ConfigurableJoint>());
        
        
        PlayerMovement.GetComponentInParent<Rigidbody>().maxAngularVelocity = 7f;

        PlayerMovement._RB.constraints = RigidbodyConstraints.None;
            
        PlayerMovement.lookDirectionoverride = false;
        
        drb.CheckForObjectSnapping();
        
        PlayerMovement.SetState(new RunningState(PlayerMovement));
        yield break;
    }

    public override void EndState()
    {
        if (confJoint)
        {
            PlayerMovement.DestroyImmediate(confJoint);
        }
        
    }
}

