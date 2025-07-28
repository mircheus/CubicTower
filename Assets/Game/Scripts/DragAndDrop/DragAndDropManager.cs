using System;
using System.Collections;
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
        [SerializeField] private Cubic cubicPrefab;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Transform floor;

        private float _xPosition;
        private bool _isDragging;
        private float _dragSpeed = 0.1f;
        private Vector3 _velocity = Vector3.zero;

        private void OnEnable()
        {
            holdAction.action.Enable();
            screenPosition.action.Enable();
            holdAction.action.performed += OnHold;
            holdAction.action.started += OnHoldStarted;
            holdAction.action.canceled += OnHoldCanceled;
        }

        private void OnDisable()
        {
            holdAction.action.Disable();
            screenPosition.action.Disable();
            holdAction.action.performed -= OnHold;
            holdAction.action.started -= OnHoldStarted;
            holdAction.action.canceled -= OnHoldCanceled;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("ScreenPosition: " + screenPosition.action.ReadValue<Vector2>());
            }
        }

        private void OnHoldCanceled(InputAction.CallbackContext obj)
        {
            // Debug.Log("Hold canceled");
            _isDragging = false;
            _xPosition = 0f; // Reset the x position when hold is canceled
            scrollRect.horizontal = true; // Re-enable horizontal scrolling
            // StopAllCoroutines(); // Stop any ongoing drag updates
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
                        Ray ray = mainCamera.ScreenPointToRay(screenPosition.action.ReadValue<Vector2>());
                        Vector3 tempRay = ray.GetPoint(10f);
                        Vector3 target = new Vector3(tempRay.x, tempRay.y, 0);
                        Cubic cubic = Instantiate(cubicPrefab, target, Quaternion.identity);
                        StartCoroutine(DragUpdate(cubic.gameObject));
                        scrollRect.horizontal = false;
                        break;
                    }
                }
            }
        }
        
        private IEnumerator DragUpdate(GameObject clickedObject)
        {
            var position = clickedObject.transform.position;
            float initialDistance = Vector3.Distance(position, mainCamera.transform.position);
            float initialCoordinateZ = position.z;
            clickedObject.TryGetComponent<IDraggable>(out var iDraggable);
            iDraggable?.StartDrag(floor);
            _isDragging = true;
              
            while (_isDragging)
            { 
                Ray ray = mainCamera.ScreenPointToRay(screenPosition.action.ReadValue<Vector2>());
                Vector3 tempRay = ray.GetPoint(initialDistance);
                Vector3 target = new Vector3(tempRay.x, tempRay.y, initialCoordinateZ);
                // target += offsetVector;
                clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position, target, ref _velocity, _dragSpeed);
                yield return null;
            }

            Debug.Log("EndDrag Coroutine");
            iDraggable?.EndDrag();
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