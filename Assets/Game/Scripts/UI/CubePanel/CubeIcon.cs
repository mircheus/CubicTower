using System;
using Game.Scripts.Cubes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.Scripts
{
    public class CubeIcon : MonoBehaviour
    {
        [SerializeField] private Image image;

        private CubeType _cubeType;

        public CubeType CubeType => _cubeType;
        
        public void Initialize(CubeType cubeType)
        {
            _cubeType = cubeType;
            image.sprite = _cubeType.CubeSprite;
        }
    }
}