using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.Scripts.UI
{
    [CreateAssetMenu(fileName = "MessagesDictionary", menuName = "Game/MessageDictionary", order = 0)]
    public class MessagesDictionary : ScriptableObject
    {
        [SerializeField] private List<LocalizedString> localizedMessages;
        [SerializeField] private List<MessageAction> messageActions;

        public List<LocalizedString> LocalizedMessages => localizedMessages;
        public List<MessageAction> MessageActions => messageActions;
        public int Count => messageActions.Count;
    }
}