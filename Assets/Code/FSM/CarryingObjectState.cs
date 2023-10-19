using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class CarryingObjectState : State
{
        private GameObject closestGameObject;
        private CarriableRigidbody drb;
        public CarryingObjectState(PlayerMovement playerMovement, GameObject closestGameObject) : base(playerMovement)
        {
                this.closestGameObject = closestGameObject;
        }

        public override IEnumerator Start()
        {
                //PlayerMovement.anim.Play("walk");
                drb = closestGameObject.GetComponent<CarriableRigidbody>();
                drb.IsBeingCarried(PlayerMovement);
                yield break;
        }

        public override IEnumerator Update()
        {
                drb.UpdateObject(PlayerMovement);
                return base.Update();
        }

        public override IEnumerator EInteract()
        {
                drb.StoppedBeingCarried();
                drb.DeactivateSnappingObject();
                
                drb.CheckForObjectSnapping();
                
                PlayerMovement.SetState(new RunningState(PlayerMovement));
                yield break;
        }

        public override IEnumerator StartUse()
        {
                if (drb.isUsable || drb.canBeThrown)
                {
                        PlayerMovement.StartCoroutine(drb.Use());
                        //drb.Use();
                        
                }
                return base.StartUse();
        }

        public override IEnumerator StopUse()
        {
                if (drb.isUsable || drb.canBeThrown)
                {
                        PlayerMovement.StartCoroutine(drb.StopUsing());
                        //drb.StopUsing();
                }
                return base.StopUse();
        }
}
