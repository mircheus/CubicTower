using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game.Scripts
{
    public class UIItem : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public void OnClick()
        {
            Debug.Log("Clicked on UIItem: " + gameObject.name);
        }
    }
}