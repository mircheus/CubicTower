﻿using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Cubes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Game.Scripts.DragAndDrop
{
    public class DragAndDrop : MonoBehaviour
    {
        [Header("References: ")]
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private GraphicRaycaster graphicRaycaster;
        [SerializeField] private InputActionReference holdAction;
        [SerializeField] private InputActionReference screenPosition;
        [SerializeField] private InputActionReference touchAction;
        [SerializeField] private LayerMask cubesLayerMask;
        [SerializeField] private float raycastDistance = 100f;
        [SerializeField] private ScrollRect scrollRect;

        private Camera _mainCamera;
        private float _xPosition;
        private bool _isDragging;
        private float _dragSpeed = 0.1f;
        private Vector3 _velocity = Vector3.zero;
        private CubeSpawner _cubeSpawner;

        [Inject]
        public void Construct(CubeSpawner cubeSpawner, Camera mainCamera)
        {
            _cubeSpawner = cubeSpawner;
            _mainCamera = mainCamera;
        }
        
        private void OnEnable()
        {
            holdAction.action.Enable();
            screenPosition.action.Enable();
            touchAction.action.Enable();
            holdAction.action.performed += OnHold;
            holdAction.action.started += OnHoldStarted;
            holdAction.action.canceled += OnHoldCanceled;
            touchAction.action.performed += OnTouchPerformed;
            touchAction.action.canceled += OnTouchCanceled;
        }

        private void OnDisable()
        {
            holdAction.action.Disable();
            screenPosition.action.Disable();
            holdAction.action.performed -= OnHold;
            holdAction.action.started -= OnHoldStarted;
            holdAction.action.canceled -= OnHoldCanceled;
            touchAction.action.performed -= OnTouchPerformed;
            touchAction.action.canceled -= OnTouchCanceled;
        }

        private void OnHoldCanceled(InputAction.CallbackContext obj)
        {
            _isDragging = false;
            _xPosition = 0f; 
            scrollRect.horizontal = true; 
        }

        private void OnHoldStarted(InputAction.CallbackContext obj)
        {
            var pos = GetPointerPosition();
            _xPosition = pos.x;
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
                
                List<RaycastResult> results = new List<RaycastResult>();
                graphicRaycaster.Raycast(pointerEventData, results);

                for(int i = 0; i < results.Count; i++)
                {
                    var result = results[i];

                    if (result.gameObject.TryGetComponent(out CubeIcon cubeIcon))
                    {
                        Ray ray = _mainCamera.ScreenPointToRay(screenPosition.action.ReadValue<Vector2>());
                        Vector3 tempRay = ray.GetPoint(10f);
                        Vector3 target = new Vector3(tempRay.x, tempRay.y, 0);
                        Cube cube = _cubeSpawner.SpawnCube(cubeIcon.CubeType, target);
                        StartCoroutine(DragUpdate(cube.gameObject));
                        scrollRect.horizontal = false; 
                        break;
                    }
                }
            }
        }

        private void OnTouchPerformed(InputAction.CallbackContext obj)
        {
            Ray ray = _mainCamera.ScreenPointToRay(screenPosition.action.ReadValue<Vector2>());
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray, raycastDistance, cubesLayerMask);
            
            if (hit2D.collider != null && hit2D.collider.gameObject.TryGetComponent(out IDraggable draggable))
            {
                if(draggable.IsAbleToDrag == false)
                {
                    return;
                }
                
                draggable.StartDrag();
                StartCoroutine(DragUpdate(hit2D.collider.gameObject));
            }
        }

        private void OnTouchCanceled(InputAction.CallbackContext obj)
        {
            _isDragging = false;
        }

        private IEnumerator DragUpdate(GameObject clickedObject)
        {
            var position = clickedObject.transform.position;
            float initialDistance = Vector3.Distance(position, _mainCamera.transform.position);
            float initialCoordinateZ = position.z;
            clickedObject.TryGetComponent<IDraggable>(out var iDraggable);
            iDraggable?.StartDrag();
            _isDragging = true;
              
            while (_isDragging)
            { 
                Ray ray = _mainCamera.ScreenPointToRay(screenPosition.action.ReadValue<Vector2>());
                Vector3 tempRay = ray.GetPoint(initialDistance);
                Vector3 target = new Vector3(tempRay.x, tempRay.y, initialCoordinateZ);
                // target += offsetVector;
                clickedObject.transform.position = Vector3.SmoothDamp(clickedObject.transform.position, target, ref _velocity, _dragSpeed);
                yield return null;
            }
            
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