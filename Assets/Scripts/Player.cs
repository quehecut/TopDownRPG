using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;
using UnityEngine.SceneManagement;

namespace RPG.Character
{
    public class Player : MonoBehaviour, IDamageable
    {
        
        [SerializeField] float baseDamage = 10f;
       
        [SerializeField] Weapon currentWeaponConfig = null;
        [SerializeField] AnimatorOverrideController animatorOverrideController = null;
        
        [Range(.1f, 1.0f)][SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;

        [SerializeField] AbilityConfig[] abilities;

        
        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT ATTACK";

        Enemy enemy = null;
        
        Animator animator;
       
        CameraRaycaster cameraRaycaster;
        float lastHitTime = 0f;
        GameObject weaponObject;

        

        void Start()
        {
            

            RegisterForMouseClick();
            SetCurrentMaxHealth();
            PutWeaponInHand(currentWeaponConfig);
            SetAttackAnimation();
            AttachInitialAbilities();
                        
        }

        public void PutWeaponInHand(Weapon weaponToUse)
        {
            currentWeaponConfig = weaponToUse;
            var weaponPrefab = weaponToUse.GetWeaponPrefab();
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject);
            weaponObject = Instantiate(weaponPrefab, dominantHand.transform);
            weaponObject.transform.localPosition = currentWeaponConfig.gripTransform.localPosition;
            weaponObject.transform.localRotation = currentWeaponConfig.gripTransform.localRotation;
        }

        void Update()
        {
            if(healthAsPercentage > Mathf.Epsilon)
            {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown()
        {
            for(int keyIndex = 1; keyIndex < abilities.Length; keyIndex++)
            {
                if(Input.GetKeyDown(keyIndex.ToString()))
                {
                    AttemptSpecialAbility(keyIndex);
                }
            }
        }

        

        

        private void AttachInitialAbilities()
        {
            for(int abilityIndex = 0; abilityIndex < abilities.Length; abilityIndex++)
            {
                abilities[abilityIndex].AttachAbilityTo(gameObject);
            }
        }

        

        private void SetAttackAnimation()
        {
            animator = GetComponent<Animator>();
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = currentWeaponConfig.GetAttackAnimClip();
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }
   
        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            int numberofDominantHands = dominantHands.Length;
            Assert.IsFalse(numberofDominantHands <= 0, "No DominantHand found on Player");
            Assert.IsFalse(numberofDominantHands > 1, "Multiple DominantHand scripts on Player");
            return dominantHands[0].gameObject;
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.onMouseOverEnemy += OnMouseOverEnemy;
        }

        void OnMouseOverEnemy(Enemy enemyToSet)
        {
            this.enemy = enemyToSet;
            if(Input.GetMouseButton(0) && IsTargetInRange(enemy.gameObject))
            {
                AttackTarget();
            }
            else if(Input.GetMouseButtonDown(1))
            {
                AttemptSpecialAbility(0);
            }
        }

        void AttemptSpecialAbility(int abilityIndex)
        {
            var energyComponent = GetComponent<Energy>();
            var energyCost = abilities[abilityIndex].GetEnergyCost();

            if(energyComponent.IsEnergyAvailable(energyCost))
            {
                energyComponent.ConsumeEnergy(energyCost);
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[abilityIndex].Use(abilityParams);
            }
        }
   
        private void AttackTarget()
        {
            if (Time.time - lastHitTime > currentWeaponConfig.GetMinTimeBetweenHits())
            {
                SetAttackAnimation();
                animator.SetTrigger(ATTACK_TRIGGER);
                enemy.TakeDamage(CalculateDamage());
                lastHitTime = Time.time;
            }
        }

        private float CalculateDamage()
        {
            bool isCriticalHit = UnityEngine.Random.Range(0f, 1f) <= criticalHitChance;
            float damageBeforeCritical = baseDamage + currentWeaponConfig.GetAdditionalDamage();
            if(isCriticalHit)
            {
                return damageBeforeCritical * criticalHitMultiplier;
            }
            else
            {
                return damageBeforeCritical;
            }
        }
        
        private bool IsTargetInRange(GameObject target)
        {
            float distanceToTarget = (target.transform.position - transform.position).magnitude;
             return distanceToTarget <= currentWeaponConfig.GetMaxAttackRange();
        }

        
    }
}