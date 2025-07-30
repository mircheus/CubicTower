using System;
using System.Linq;
using Game.Scripts.Events;
using UnityEngine;

namespace Game.Scripts.CubeParticleSystem
{
    public class CubeParticleSystem : MonoBehaviour, IDestroyCubeEvents
    {
        [SerializeField] private ParticleSystem[] destroyFx;

        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnCubeDestroyed(Vector3 position)
        {
            var availableFx = destroyFx.FirstOrDefault(fx => fx.gameObject.activeSelf == false);
            {
                if (availableFx != null)
                {
                    availableFx.gameObject.transform.position = position;
                    availableFx.gameObject.SetActive(true);
                }
            }
        }
    }
}