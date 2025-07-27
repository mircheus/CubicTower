using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.Scripts.DragAndDrop
{
    public class DragAndDropManager : MonoBehaviour
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [SerializeField] private InputActionReference holdAction;
        [SerializeField] private InputActionReference screenPosition;
        [SerializeField] private LayerMask raycastLayerMask;
        [SerializeField] private float raycastDistance = 100f;

        private float _xPosition;
        
        private void OnEnable()
        {
            holdAction.action.Enable();
            screenPosition.action.Enable();
            holdAction.action.performed += OnHold;
            holdAction.action.started += OnHoldStarted;
        }

        private void OnDisable()
        {
            holdAction.action.Disable();
            screenPosition.action.Disable();
            holdAction.action.performed -= OnHold;
            holdAction.action.started -= OnHoldStarted;
        }

        private void OnHoldStarted(InputAction.CallbackContext obj)
        {
            var pos = GetPointerPosition();
            _xPosition = pos.x;
            // Debug.Log($"OnHoldStarted: {pos}");
        }

        private void OnHold(InputAction.CallbackContext context)
        {
            var pos = GetPointerPosition();
            
            if(Math.Abs(_xPosition - pos.x) < .5f)
            {
                var pointerEventData = new PointerEventData(eventSystem)
                {
                    position = pos
                };
                // Debug.Log($"Holding preformed: {pos}");
                List<RaycastResult> results = new List<RaycastResult>();
                graphicRaycaster.Raycast(pointerEventData, results);
                Debug.Log("results.length: " +results.Count);
                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                foreach (RaycastResult result in results)
                {
                    // Debug.Log("Hit " + result.gameObject.name);
                }
            }
        }

        private Vector2 GetPointerPosition()
        {
            return screenPosition.action.ReadValue<Vector2>();
        }

    
    }
}