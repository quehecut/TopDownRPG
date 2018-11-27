using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Character : MonoBehaviour
    {
        [Header("Animator Setup Settings")]
        [SerializeField] RuntimeAnimatorController animController;
        [SerializeField] AnimatorOverrideController animOverrideController;
        [SerializeField] Avatar characterAvatar;

        [Header("Collider Capsule Settings")]
        [SerializeField] Vector3 colliderCenter = new Vector3(0, 0.84f, 0);
        [SerializeField] float colliderRadius = 0.3f;
        [SerializeField] float colliderHeight = 1.6f;

        [Header("Movement Settings")]
        [SerializeField] float movingTurnSpeed = 360;
		[SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = 1f;

        float turnAmount;
		float forwardAmount;
        bool isAlive = true;
          
        NavMeshAgent agent;
        Animator anim;
        Rigidbody rigid;

        void Awake()
        {
            AddRequiredComponents();
        }

        private void AddRequiredComponents()
        {
            var capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            capsuleCollider.center = colliderCenter;
            capsuleCollider.radius = colliderRadius;
            capsuleCollider.height = colliderHeight;

            rigid = gameObject.AddComponent<Rigidbody>();
            rigid.constraints = RigidbodyConstraints.FreezeRotation;

            var audioSource = gameObject.AddComponent<AudioSource>();

            anim = gameObject.AddComponent<Animator>();
            anim.runtimeAnimatorController = animController;
            anim.avatar = characterAvatar;
        }

        void Start()
        {                      
            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;
            agent.updateRotation = true;
            agent.stoppingDistance = stoppingDistance;
        }

        public float GetAnimSpeedMultiplier()
        {
            return anim.speed;
        }

        public void SetDestination(Vector3 worldPos)
        {
            agent.destination = worldPos;
        }

        void Move(Vector3 movement)
		{
			SetMovement(movement);
			ApplyExtraTurnRotation();
			UpdateAnimator();
		}

        public void Kill()
        {
            isAlive = false;
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

        public AnimatorOverrideController GetOverrideController()
        {
            return animOverrideController;
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
            
            if(agent.remainingDistance > agent.stoppingDistance && isAlive)
            {
                Move(agent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
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





