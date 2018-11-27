using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        Player player;
       
        void Start()
        {
            player = GetComponent<Player>();
        }
              
        public override void Use(GameObject target)
        {
            PlayAbilitySound();
            var playerHealth = player.GetComponent<HealthSystem>();
            playerHealth.Heal((config as SelfHealConfig).GetExtraHealth());
            
        }
    }
}

