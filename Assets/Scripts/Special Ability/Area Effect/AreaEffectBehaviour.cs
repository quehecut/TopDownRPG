using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Character;

public class AreaEffectBehaviour : AbilityBehaviour
{
    public override void Use(GameObject target)
    {
        DealRadialDamage();
        PlayParticleEffect();
    }
    
    private void DealRadialDamage()
    {
        //static sphere cast for target
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, (config as AreaEffectConfig).GetRadius(), Vector3.up, (config as AreaEffectConfig).GetRadius());

        foreach (RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<HealthSystem>();
            bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
            if (damageable != null && !hitPlayer)
            {
                float damageToDeal = (config as AreaEffectConfig).GetDamageToEachTarget();
                damageable.TakeDamage(damageToDeal);
            }
        }
    }

    
    
}
