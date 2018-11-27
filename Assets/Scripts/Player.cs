﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.CameraUI;

namespace RPG.Character
{
    public class Player : MonoBehaviour
    {             
        Enemy enemy = null;
        SpecialAbilities abilities;
        WeaponSystem weaponSystem;
        CameraRaycaster cameraRaycaster;
        Character character;        

        void Start()
        {
            character = GetComponent<Character>();
            abilities = GetComponent<SpecialAbilities>();
            weaponSystem = GetComponent<WeaponSystem>();

            RegisterForMouseEvents();                                      
        }

        private void RegisterForMouseEvents()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverPotentiallyWalkable += OnMouseOverPotentiallyWalkable;
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }       

        void Update()
        {
            ScanForAbilityKeyDown();
        }

        void ScanForAbilityKeyDown()
        {
            for(int keyIndex = 1; keyIndex < abilities.GetNumberOfAbilities(); keyIndex++)
            {
                if(Input.GetKeyDown(keyIndex.ToString()))
                {
                   abilities.AttemptSpecialAbility(keyIndex);
                }
            }
        }     
       
        void OnMouseOverPotentiallyWalkable(Vector3 destination)
        {
            if(Input.GetMouseButton(0))
            {
                character.SetDestination(destination);
            }
        }
        
        void OnMouseOverEnemy(Enemy enemyToSet)
        {
            this.enemy = enemyToSet;
            if(Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                weaponSystem.AttackTarget(enemy.gameObject);
            }
            else if(Input.GetMouseButtonDown(1))
            {
                abilities.AttemptSpecialAbility(0);
            }
        }           
        
        bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
             return distanceToTarget <= weaponSystem.GetCurrrentWeapon().GetMaxAttackRange();
        }

        
    }
}