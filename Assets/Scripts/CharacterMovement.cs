using System;
using UnityEngine;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class CharacterMovement : MonoBehaviour
    {
        //[SerializeField] float walkMoveStopRadius = 0.2f;
        [SerializeField] float stoppingDistance = 1f;

        ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
        Vector3 clickPoint;
        GameObject walkTarget;
        NavMeshAgent agent;

        

        void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();

            walkTarget = new GameObject("walkTarget");

            agent = GetComponent<NavMeshAgent>();
            agent.updatePosition = false;
            agent.updateRotation = true;
            agent.stoppingDistance = stoppingDistance;

            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void Update()
        {
            if(agent.remainingDistance > agent.stoppingDistance)
            {
                thirdPersonCharacter.Move(agent.desiredVelocity);
            }
            else
            {
                thirdPersonCharacter.Move(Vector3.zero);
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
       
    }
}





