﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(Character))]
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2.0f;
        [SerializeField] float waypointStayTime = 0.5f;
      

        enum State { idle, attacking, patrolling, chasing }
        State state = State.idle;

        Player player;
        Character character;
        int nextWaypointIndex;
        float currentWeaponRange;
        float distanceToPlayer;
           
        void Start()
        {
            player = FindObjectOfType<Player>();
            character = GetComponent<Character>();
        }
    
        void Update()
        {
            distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrrentWeapon().GetMaxAttackRange();  

            bool inWeaponCircle = distanceToPlayer <= currentWeaponRange;
            bool inChaseCircle = distanceToPlayer > currentWeaponRange && distanceToPlayer <= chaseRadius;
            bool outsideChaseRing = distanceToPlayer > chaseRadius; 

            if(outsideChaseRing)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(Patrol());
            }  

            if(inChaseCircle)
            {
                StopAllCoroutines();
                weaponSystem.StopAttacking();
                StartCoroutine(ChasePlayer());
            }

            if(inWeaponCircle)
            {
                StopAllCoroutines();
                state = State.attacking;
                weaponSystem.AttackTarget(player.gameObject);
            }     
        }  

        IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while(distanceToPlayer >= currentWeaponRange)
            {
                character.SetDestination(player.transform.position);
                yield return new WaitForEndOfFrame();
            }
        }  

        IEnumerator Patrol()
        {
            state = State.patrolling;

            while (patrolPath != null)
            {
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                character.SetDestination(nextWaypointPos);
                CycleWaypointWhenClose(nextWaypointPos);

                yield return new WaitForSeconds(waypointStayTime);
            }
        }

        private void CycleWaypointWhenClose(Vector3 nextWaypointPos)
        {
            if(Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
        }  

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, currentWeaponRange);

            Gizmos.color = new Color(0, 255f, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}