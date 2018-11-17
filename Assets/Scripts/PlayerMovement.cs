using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Character
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {
        //[SerializeField] float walkMoveStopRadius = 0.2f;
        //[SerializeField] float attackMoveStopRadius = 5f;

        ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster = null;
        Vector3 clickPoint;
        AICharacterControl aiCharacterControl = null;
        GameObject walkTarget = null;

        //bool isInDirectMode = false;

        void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();

            aiCharacterControl = GetComponent<AICharacterControl>();
            walkTarget = new GameObject("walkTarget");

            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                aiCharacterControl.SetTarget(walkTarget.transform);
            }
        }

        void OnMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
            {
                aiCharacterControl.SetTarget(enemy.transform);
            }
        }

       
        void ProcessDirectMovement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 cameraForward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 movement = v * cameraForward + h * Camera.main.transform.right;

            thirdPersonCharacter.Move(movement, false, false);
        }
    }
}

// Fixed update is called in sync with physics
//    void FixedUpdate()
//    {
//        if (Input.GetKeyDown(KeyCode.G))//TODO add to menu. G for Gamepad 
//        {
//            isInDirectMode = !isInDirectMode;
//            currentDestination = transform.position; //clearing click target
//        }
//        if (isInDirectMode)
//        {
//            ProcessDirectMovement();
//        }
//        else
//        {
//            //ProcessMouseMovement();
//        }
//    }
//}



//private void ProcessMouseMovement()
//{
//    if (Input.GetMouseButton(0))
//    {
//        clickPoint = cameraRaycaster.hit.point;
//        switch (cameraRaycaster.currentLayerHit)
//        {
//            case Layer.Walkable:
//                currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
//                break;
//            case Layer.Enemy:
//                currentDestination = ShortDestination(clickPoint, attackMoveStopRadius);
//                break;
//            default:
//                print("Unexpected layer");
//                return;
//        }

//    }
//    WalkToDestination();
//}

//private void WalkToDestination()
//{
//    var playerToClickPoint = currentDestination - transform.position;
//    if (playerToClickPoint.magnitude >= walkMoveStopRadius)
//    {
//        thirdPersonCharacter.Move(playerToClickPoint, false, false);
//    }
//    else
//    {
//        thirdPersonCharacter.Move(Vector3.zero, false, false);
//    }
//}

//Vector3 ShortDestination(Vector3 destination, float shortening)
//{
//    Vector3 reductionVector = (destination - transform.position).normalized * shortening;
//    return destination - reductionVector;
//} 

//void OnDrawGizmos()
//{
//    Gizmos.color = Color.black;
//    Gizmos.DrawLine(transform.position, currentDestination);
//    Gizmos.DrawSphere(currentDestination, 0.1f);
//    Gizmos.DrawSphere(clickPoint, 0.15f);

//    Gizmos.color = new Color(255f, 0f, 0, .5f);
//    Gizmos.DrawWireSphere(transform.position, attackMoveStopRadius);
//}



