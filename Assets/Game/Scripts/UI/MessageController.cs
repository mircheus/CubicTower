using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using Zenject;

namespace Game.Scripts.UI
{
    public class MessageController : MonoBehaviour, IMessageActionEvents
    {
        [SerializeField] private MessageView messageView;
        
        private Dictionary<MessageAction, LocalizedString> _localizedMessages = new Dictionary<MessageAction, LocalizedString>();

        [Inject]
        public void Construct(MessagesDictionary messagesDictionary)
        {
            for (int i = 0; i < messagesDictionary.Count; i++)
            {
                _localizedMessages.Add(messagesDictionary.MessageActions[i], messagesDictionary.LocalizedMessages[i]);
            }
        }
        
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void Show(MessageAction action)
        {
            messageView.Show(_localizedMessages[action]);
        }
    }
}
