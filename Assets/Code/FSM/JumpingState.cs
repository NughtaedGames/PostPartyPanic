using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingState : State
{
    private float airTime;
    
    public JumpingState(PlayerMovement playerMovement) : base(playerMovement)
    {
        var vel = PlayerMovement._RB.velocity;
        PlayerMovement._RB.AddForce(new Vector3(0, PlayerMovement.JumpForce, 0), ForceMode.Impulse);
        //PlayerMovement._RB.velocity = new Vector3(vel.x, 5f, vel.z);
        airTime = 0;
        PlayerMovement.jumpDust.Play();
    }


    public override IEnumerator Update()
    {
        yield break;
    }

    public override IEnumerator FixedUpdate()
    {
        airTime += Time.fixedDeltaTime;
        if (airTime > 1.5)
        {
            if (CheckIfGrounded())
            {
                yield return new WaitForSecondsRealtime(0.2f);
                PlayerMovement.SetState(new RunningState(PlayerMovement));
            }
        }
        UpdateUprightForce();
        Movement();
        yield break;
    }
    
    protected bool CheckIfGrounded()
    {
        bool _rayDidHit;
        float offset = 0.1f;
        
        var pos = new Vector3(PlayerMovement.transform.position.x, PlayerMovement.transform.position.y + offset,
            PlayerMovement.transform.position.z);
        _rayDidHit = Physics.Raycast(pos, PlayerMovement.transform.TransformDirection(Vector3.down),
            out RaycastHit _rayHit, PlayerMovement.MaxRayDist);
            
        Debug.DrawRay(PlayerMovement.transform.position, PlayerMovement.transform.TransformDirection(Vector3.down), Color.blue);
        
        if (_rayDidHit)
        {
            
            float x = _rayHit.distance - offset - PlayerMovement.RideHeight;
            if (x < 0.1)
            {
                return true;
            }
        }

        return false;
    }

    public override void PlayDust() { }

    public override void StopPlayingDust() { }

    public override IEnumerator EInteract() { yield break; }
}
