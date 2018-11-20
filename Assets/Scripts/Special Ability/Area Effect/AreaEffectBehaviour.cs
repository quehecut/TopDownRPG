using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Character;
using RPG.Core;

public class AreaEffectBehaviour : AbilityBehaviour
{
    AreaEffectConfig config;
        
    public void SetConfig(AreaEffectConfig configToSet)
    {
        this.config = configToSet;
    }

    public override void Use(AbilityUseParams useParams)
    {
        DealRadialDamage(useParams);
        PlayParticleEffect();
    }
    
    private void DealRadialDamage(AbilityUseParams useParams)
    {
        //static sphere cast for target
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, config.GetRadius(), Vector3.up, config.GetRadius());

        foreach (RaycastHit hit in hits)
        {
            var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
            bool hitPlayer = hit.collider.gameObject.GetComponent<Player>();
            if (damageable != null && !hitPlayer)
            {
                float damageToDeal = useParams.baseDamage + config.GetDamageToEachTarget();
                damageable.TakeDamage(damageToDeal);
            }
        }
    }

    private void PlayParticleEffect()
    {
        var prefab = Instantiate(config.GetParticlePrefab(), transform.position, Quaternion.identity);
        ParticleSystem myParticleSystem = prefab.GetComponent<ParticleSystem>();
        myParticleSystem.Play();
        Destroy(prefab, myParticleSystem.main.duration);
    }
    
}
