using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Character
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        
        [SerializeField] float attackRadius = 4f;
        [SerializeField] float damagePerShot = 9f;
        [SerializeField] float firingPeriodInSec = .5f;
        [SerializeField] float firingPeriodVariation = .2f;
        [SerializeField] float chaseRadius = 6f;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;
        [SerializeField] Vector3 aimOffset = new Vector3(0, 1f, 0);

        bool isAttacking = false;
        
        Player player = null;
            

        void Start()
        {
            player = FindObjectOfType<Player>();
        }

        public void TakeDamage(float amount)
        {

        }

        void Update()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            if (distanceToPlayer <= attackRadius && !isAttacking)
            {
                isAttacking = true;
                float randomisedDelay = Random.Range(firingPeriodInSec - firingPeriodVariation, firingPeriodInSec + firingPeriodVariation);
                InvokeRepeating("FireProjectile", 0f, randomisedDelay);

            }

            if (distanceToPlayer > attackRadius)
            {
                isAttacking = false;
                CancelInvoke();
            }


            if (distanceToPlayer <= chaseRadius)
            {
                //aiCharacterControl.SetTarget(player.transform);
            }
            else
            {
                //aiCharacterControl.SetTarget(transform);
            }
        }

        void FireProjectile()
        {
            GameObject newProjectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
            var projectileComponent = newProjectile.GetComponent<Projectile>();
            projectileComponent.SetDamage(damagePerShot);
            projectileComponent.SetShooter(gameObject);

            Vector3 unitVectorToPlayer = (player.transform.position + aimOffset - projectileSocket.transform.position).normalized;
            float projectileSpeed = projectileComponent.GetDefaultLaunchSpeed();
            newProjectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        }

        void OnDrawGizmos()
        {
            Gizmos.color = new Color(255f, 0, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, attackRadius);

            Gizmos.color = new Color(0, 255f, 0, .5f);
            Gizmos.DrawWireSphere(transform.position, chaseRadius);
        }
    }
}