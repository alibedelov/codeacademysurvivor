using System;
using TMPro;
using UnityEngine;

namespace Code.Scripts.UI
{
    public class QuittingPopup : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        private Action onYesEvent;
        private Action onNoEvent;

        public void Initialize(string message, Action onYes, Action onNo = null)
        {
            this.messageText.text = message;
            this.onYesEvent = onYes;
            this.onNoEvent = onNo;

            gameObject.SetActive(true);
        }

        public void OnClickNo()
        {
            gameObject.SetActive(false);
            onNoEvent?.Invoke();
        }

        public void OnClickYes()
        {
            gameObject.SetActive(false);
            onYesEvent?.Invoke();
        }
    }
}