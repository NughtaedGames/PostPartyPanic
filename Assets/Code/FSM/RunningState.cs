using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RunningState : State
{
    public RunningState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override IEnumerator Start()
    {
        //PlayerMovement.anim.Play("walk");
        return base.Start();
    }

    public override IEnumerator Update()
    {
        //UpdateJumpForce();

        if (PlayerMovement.collidingObjects.Count > 0)
        {
            List<MovableRigidbody> sortedList;
            sortedList = UtilsMath.OrderMovableRigidbodiesListByDistance(PlayerMovement.collidingObjects, PlayerMovement.transform);

            sortedList[0].ActivateOutline();
            for (int i = 1; i < sortedList.Count; i++)
            {
                sortedList[i].DeactivateOutline();
            }
        }

        // PlayerMovement.rightHandSplineNode.position = PlayerMovement.rightHandOriginalNodePos.position;
        // PlayerMovement.leftHandSplineNode.position = PlayerMovement.rightHandOriginalNodePos.position;
        
        return base.Update();
    }

    public override void Jump()
    {
        if (!(PlayerMovement.GetState() is JumpingState))
        {
            PlayerMovement.SetState(new JumpingState(PlayerMovement));
        }
    }
    
    // private void UpdateJumpForce()
    // {
    //     
    //     if (Input.GetKeyDown(KeyCode.Space)  && )
    //     {
    //         Debug.Log("Jump");
    //         
    //     }
    // }

    // public override IEnumerator EInteract()
    // {
    //     return base.EInteract();
    //     if (!PlayerMovement.lookDirectionoverride)
    //     {
    //         
    //     }
    //     
    //     
    //     
    //     yield break;
    // }
}
