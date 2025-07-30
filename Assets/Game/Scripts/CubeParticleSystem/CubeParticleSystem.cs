using System;
using Game.Scripts.Events;
using UnityEngine;

namespace Game.Scripts.CubeParticleSystem
{
    public class CubeParticleSystem : MonoBehaviour, IDestroyCubeEvents
    {
        [SerializeField] private ParticleSystem destroyFx;

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
            destroyFx.gameObject.transform.position = position;
            destroyFx.gameObject.SetActive(true);
        }
    }
}