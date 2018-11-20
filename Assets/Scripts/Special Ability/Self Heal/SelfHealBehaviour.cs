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
              
        public override void Use(AbilityUseParams useParams)
        {
            player.Heal((config as SelfHealConfig).GetExtraHealth());
            PlayAbilitySound();
        }
    }
}

