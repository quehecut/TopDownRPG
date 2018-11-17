﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Character
{
    [CreateAssetMenu(menuName = ("RPG/Special Ability/Power Attack"))]
    public class PowerAttackConfig : SpecialAbilityConfig
    {
         [Header("Power Attack Specific")]
         [SerializeField] float extraDamage = 10f;
         
        
        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();
            behaviourComponent.SetConfig(this);
            behaviour = behaviourComponent;
        }
    }
}
