using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Game.Scripts.UI
{
    public class MessageView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private TMP_Text messageText;

        private bool _isShowing;
        private Coroutine _hideCoroutine;
        
        private void Awake()
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            _isShowing = false;
        }

        public void Show(LocalizedString localizedString)
        {
            if (_isShowing == false)
            {
                _isShowing = true;
                canvasGroup.alpha = 0f;
                messageText.text = localizedString.GetLocalizedString();
                canvasGroup.DOFade(1f, 0.5f);
                
                if (_hideCoroutine != null)
                {
                    StopCoroutine(_hideCoroutine);
                }
                
                _hideCoroutine = StartCoroutine(HideWithDelay());
            }
        }

        public void Hide()
        {
            canvasGroup.DOFade(0f, 0.5f).OnComplete(() =>
            {
                _isShowing = false;
            });
        }

        private IEnumerator HideWithDelay()
        {
            yield return new WaitForSeconds(1f);
            Hide();
        }
    }
}