using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    public class SelfHealBehaviour : AbilityBehaviour
    {
        SelfHealConfig config;
        Player player;
        AudioSource audioSource;

        void Start()
        {
            player = GetComponent<Player>();
            audioSource = GetComponent<AudioSource>();
        }

        public void SetConfig(SelfHealConfig configToSet)
        {
            this.config = configToSet;
        }
        
        public override void Use(AbilityUseParams useParams)
        {
            player.Heal(config.GetExtraHealth());
            audioSource.clip = config.GetAudioClip();
            audioSource.Play();
        }
    }
}

