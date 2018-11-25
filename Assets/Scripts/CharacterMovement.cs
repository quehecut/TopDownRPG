using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float movingTurnSpeed = 360;
		[SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = 1f;

        float turnAmount;
		float forwardAmount;
          
        Vector3 clickPoint;
        NavMeshAgent agent;

        Animator anim;
        Rigidbody rigid;
    

        void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            
            anim = GetComponent<Animator>();
            anim.applyRootMotion = true;
            rigid = GetComponent<Rigidbody>();

            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;
            agent.updateRotation = true;
            agent.stoppingDistance = stoppingDistance;

            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void Move(Vector3 movement)
		{
			SetMovement(movement);
			ApplyExtraTurnRotation();
			UpdateAnimator();
		}

        public void Kill()
        {
            
        }

		private void SetMovement(Vector3 movement)
		{
			if (movement.magnitude > 1f)
			{
				movement.Normalize();
			} 
			var localMove = transform.InverseTransformDirection(movement);
			
			turnAmount = Mathf.Atan2(localMove.x, localMove.z);
			forwardAmount = localMove.z;
		}

	
		void UpdateAnimator()
		{
			// update the animator parameters
			anim.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
			anim.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
						
		}

		void ApplyExtraTurnRotation()
		{
			// help the character turn faster (this is in addition to root rotation in the animation)
			float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
			transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
		}

        void Update()
        {
            
            if(agent.remainingDistance > agent.stoppingDistance)
            {
                Move(agent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                agent.SetDestination(destination);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
               agent.SetDestination(enemy.transform.position);
            }
        }

        void OnAnimatorMove()
        {
            if(Time.deltaTime > 0)
            {
                Vector3 velocity = (anim.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                velocity.y = rigid.velocity.y;
                rigid.velocity = velocity;
            }
        }
       
    }
}





