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

        public void Initialize(CubeType cubeType)
        {
            image.sprite = cubeType.CubeSprite;
        }
    }
}