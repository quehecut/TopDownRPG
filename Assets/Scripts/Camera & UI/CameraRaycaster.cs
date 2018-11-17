using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Character;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        // INSPECTOR PROPERTIES RENDERED BY CUSTOM EDITOR SCRIPT
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D enemyCursor = null;
        [SerializeField] Vector2 cursorHotSpot = new Vector2(0, 0);

        const int POTENTIALLY_WALKABLE_LAYER = 9;
        float maxRaycastDepth = 100f; // Hard coded value
       
        //New delegates
        //OnMouseOverTerrain(Vector3)
        //OnMouseOverEnemy(enemy)

        public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverPotentiallyWalkable;

        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;
     
        void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //UI later
            }
            else
            {
                PerformRaycasts();
            }
            
        }

        void PerformRaycasts()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (RaycastForEnemy(ray)) { return; }
            if (RaycastForPotentiallyWalkable(ray)) { return; }
        }

        bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameObjectHit = hitInfo.collider.gameObject;
            var enemyHit = gameObjectHit.GetComponent<Enemy>();
            if (enemyHit)
            {
                Cursor.SetCursor(enemyCursor, cursorHotSpot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
            return false;
		}

        private bool RaycastForPotentiallyWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALKABLE_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
            if(potentiallyWalkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotSpot, CursorMode.Auto);
                onMouseOverPotentiallyWalkable(hitInfo.point);
                return true;
            }
            return false;
        }
         
        
    }

}