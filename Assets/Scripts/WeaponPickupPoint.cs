using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Character;

namespace RPG.Character
{
    public class WeaponPickupPoint : MonoBehaviour
    {
        [SerializeField] WeaponConfig weaponConfig;
        [SerializeField] AudioClip pickUpSFX;

        AudioSource audioSource;

        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        // void DestroyChildren()
        // {
        //     foreach (Transform child in transform)
        //     {
        //         DestroyImmediate(child.gameObject);
        //     }
        // }

        // // Update is called once per frame
        // void Update()
        // {
        //     if(!Application.isPlaying)
        //     {
        //         DestroyChildren();
        //         InstantiateWeapon();
        //     }
        // }

        // void InstantiateWeapon()
        // {
        //     var weapon = weaponConfig.GetWeaponPrefab();
        //     weapon.transform.position = Vector3.zero;
        //     Instantiate(weapon, gameObject.transform);
        // }

        void OnTriggerEnter()
        {
            FindObjectOfType<WeaponSystem>().PutWeaponInHand(weaponConfig);
            audioSource.PlayOneShot(pickUpSFX);
            
        }
    }
}

