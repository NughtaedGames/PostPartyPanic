using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.VFX;


[System.Serializable]
public abstract class State
{

    protected PlayerMovement PlayerMovement;
    protected GameObject closestObject;

    private bool isFootstepdustPlaying;

    public State(PlayerMovement playerMovement)
    {
        PlayerMovement = playerMovement;
    }

    public virtual void EndState() { }
    

    public virtual IEnumerator Start() { yield break; }

    public virtual IEnumerator Update()
    {
        UpdateDownForce();
        yield break;
    }
    
    public virtual IEnumerator FixedUpdate()
    {
        UpdateUprightForce();
        Movement();
        yield break;
    }

    public virtual IEnumerator EInteract()
    {
        if (PlayerMovement.collidingObjects.Count < 1)
        {
            yield break;
        }
        List<MovableRigidbody> sortedList;
        sortedList = UtilsMath.OrderMovableRigidbodiesListByDistance(PlayerMovement.collidingObjects, PlayerMovement.transform);

        sortedList[0].ActivateSnappingObject();
        for (int i = 1; i < sortedList.Count; i++)
        {
            sortedList[i].DeactivateSnappingObject();
        }
        
        if(sortedList[0] is CarriableRigidbody cr && !cr.isBeingHeld)
        //if (sortedList[0].GetComponent<Rigidbody>().mass > PlayerMovement.carriableMass)
        {            
            PlayerMovement.SetState(new CarryingObjectState(PlayerMovement, sortedList[0].gameObject));
            
        }
        else if(sortedList[0] is DragRigidbody)
        {
            PlayerMovement.SetState(new DraggingObjectState(PlayerMovement, sortedList[0].gameObject));
        }
        
        yield break;
    }

    public virtual IEnumerator StartUse()
    {
        yield break;
    }

    public virtual IEnumerator StopUse()
    {
        yield break;
    }

    public void Movement()
    {
        
        // input
        if (PlayerMovement.move.magnitude > 1.0f)
        {
            PlayerMovement.move.Normalize();
        }

        if (PlayerMovement.move.x > 0.2 || PlayerMovement.move.x < - 0.2 || PlayerMovement.move.z > 0.2 || PlayerMovement.move.z < - 0.2)
        {
            PlayerMovement.anim.Play("walk");
            
            
            if (!isFootstepdustPlaying)
            {
                PlayDust();
                
                isFootstepdustPlaying = true;
            }
            
            float degrees = math.atan2(PlayerMovement.move.x, PlayerMovement.move.z) * Mathf.Rad2Deg;
        
            
            PlayerMovement.SetUprightJointTargetRot(degrees);
        }
        else
        {
            PlayerMovement.SetToCurrentUprightJointTargetRot();
            StopPlayingDust();
            isFootstepdustPlaying = false;
            PlayerMovement.anim.Play("idle");
        }


        PlayerMovement.m_UnitGoal = PlayerMovement.move;

        // calculate new goal vel
        Vector3 unitVel = PlayerMovement.m_GoalVel.normalized;

        float velDot = Vector3.Dot(PlayerMovement.m_UnitGoal, unitVel);
        float accel = PlayerMovement.Acceleration * PlayerMovement.AccelerationFactorFromDot.Evaluate(velDot);
        Vector3 goalVel = PlayerMovement.m_UnitGoal * (PlayerMovement.MaxSpeed * PlayerMovement.speedFactor);

        PlayerMovement.m_GoalVel = Vector3.MoveTowards(PlayerMovement.m_GoalVel, (goalVel) + (PlayerMovement.groundVel),
            accel * Time.fixedDeltaTime); //* PhysicsOneUpdatePerFrame.currentTimeStep);
        
        //actual force
        Vector3 neededAccel = (PlayerMovement.m_GoalVel - PlayerMovement._RB.velocity) / Time.fixedDeltaTime; //PhysicsOneUpdatePerFrame.currentTimeStep;

        float maxAccel = PlayerMovement.MaxAccelForce * PlayerMovement.MaxAccelerationForceFactorFromDot.Evaluate(velDot) * PlayerMovement.maxAccelForceFactor;

        neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);

        
        
        Vector3 vec = Vector3.Scale(neededAccel * PlayerMovement._RB.mass, PlayerMovement.ForceScale);
        
        //Needs to be fixed

        
        PlayerMovement._RB.AddForce(vec);
        
        //_RB.AddForce(Vector3.Scale(move * _RB.mass, ForceScale));
    }
    
    protected void UpdateDownForce()
    {
        bool _rayDidHit;

        _rayDidHit = Physics.Raycast(PlayerMovement.transform.position, PlayerMovement.transform.TransformDirection(Vector3.down),
            out RaycastHit _rayHit, PlayerMovement.MaxRayDist);
            
        Debug.DrawRay(PlayerMovement.transform.position, PlayerMovement.transform.TransformDirection(Vector3.down), Color.blue);
        
        if (_rayDidHit)
        {
            Vector3 vel = PlayerMovement._RB.velocity;
            Vector3 rayDir = PlayerMovement.transform.TransformDirection(PlayerMovement.DownDir);
            
            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = _rayHit.rigidbody;
            
            if (hitBody != null)
            {
                otherVel = hitBody.velocity;
            }

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;

            float x = _rayHit.distance - PlayerMovement.RideHeight;

            float springForce = (x * PlayerMovement.RideSpringStrength) - (relVel * PlayerMovement.RideSpringDamper);

            var trans = PlayerMovement.transform.position;
            Debug.DrawLine(trans, trans + (rayDir * springForce), Color.yellow);
            
            PlayerMovement._RB.AddForce(rayDir * springForce);

            //Debug.LogError("Force: " + springForce);
            
            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(rayDir * - springForce, _rayHit.point);
            }
        }
    }

    public virtual void Jump() { }
    
    protected void UpdateUprightForce()  //float elapsed)
    {
        Quaternion characterCurrent = PlayerMovement.transform.rotation;
        Quaternion toGoal = UtilsMath.ShortestRotation(PlayerMovement._uprightJointTargetRot, characterCurrent);

        if (Quaternion.Angle(characterCurrent, PlayerMovement._uprightJointTargetRot) > 150)
        {
            PlayerMovement.transform.rotation = PlayerMovement._uprightJointTargetRot;
            //PlayerMovement._RB.maxAngularVelocity = PlayerMovement.maxAngularVelocity;
            return;
        }

        Vector3 rotAxis;
        float rotDegrees;
        
    
        toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
        rotAxis.Normalize();
    
        float rotRadians = rotDegrees * Mathf.Deg2Rad;
        
        PlayerMovement._RB.AddTorque((rotAxis * (rotRadians * PlayerMovement._uprightJointSpringStrength)) - (PlayerMovement._RB.angularVelocity * PlayerMovement._uprightJointSpringDamper));

    }

    public virtual void PlayDust()
    {
        PlayerMovement.footstepsDust.Play();
    }

    public virtual void StopPlayingDust()
    {
        PlayerMovement.footstepsDust.Stop();
    }
}
