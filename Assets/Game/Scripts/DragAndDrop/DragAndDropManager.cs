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
            var position = GetPointerPosition();
            
            if(IsHoldingPointer(position))
            {
                var pointerEventData = new PointerEventData(eventSystem)
                {
                    position = position
                };
                
                // Debug.Log($"Holding preformed: {pos}");
                List<RaycastResult> results = new List<RaycastResult>();
                graphicRaycaster.Raycast(pointerEventData, results);
                Debug.Log("results.length: " + results.Count);
                //For every result returned, output the name of the GameObject on the Canvas hit by the Ray
                for(int i = 0; i < results.Count; i++)
                {
                    var result = results[i];
                    Debug.Log($"Result {i}: {result.gameObject.name}");
                    
                    // Check if the result is a UIItem and call its OnClick method
                    var uiItem = result.gameObject.GetComponent<UIItem>();
                    if (uiItem != null)
                    {
                        uiItem.OnClick();
                        break;
                    }
                }
            }
        }

        private Vector2 GetPointerPosition()
        {
            return screenPosition.action.ReadValue<Vector2>();
        }

        private bool IsHoldingPointer(Vector2 position)
        {
            return Math.Abs(_xPosition - position.x) < .5f;
        }

    
    }
}