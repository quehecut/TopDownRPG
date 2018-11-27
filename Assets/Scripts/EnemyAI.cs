using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] float chaseRadius = 6f;
      

        bool isAttacking = false;
        Player player = null;
        float currentWeaponRange;
           
        void Start()
        {
            player = FindObjectOfType<Player>();
        }
    
        void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetCurrrentWeapon().GetMaxAttackRange();          
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