using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(menuName = ("RPG/Weapon"))]
    public class WeaponConfig: ScriptableObject
    {

        public Transform gripTransform;

        [SerializeField] float minTimeBetweenHit = .5f;
        [SerializeField] float maxAttackRange = 2f;
        [SerializeField] float additionalDamage = 10f;
        [SerializeField] GameObject weaponPrefab;
        [SerializeField] AnimationClip attackAnimation;

        public float GetMinTimeBetweenHits()
        {
            return minTimeBetweenHit;
        }

        public float GetMaxAttackRange()
        {
            return maxAttackRange;
        }

        public GameObject GetWeaponPrefab()
        {
            return weaponPrefab;
        }

        public AnimationClip GetAttackAnimClip()
        {
            return attackAnimation;
        }

        public float GetAdditionalDamage()
        {
            return additionalDamage;
        }
    }
}