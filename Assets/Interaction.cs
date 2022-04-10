using UnityEngine;
using TMPro;
using System.Collections.Generic;

namespace GameCore
{
    [DisallowMultipleComponent]
    public abstract class Interaction : MonoBehaviour
    {
        [SerializeField] private bool _isEnabled = true;
        [SerializeField, Min(0)] private float distanceToShowInfo = 3;
        [SerializeField, TextArea(2, 4)] private string messageText = $"Press ''E'' to interact";
        [SerializeField] private float extraTextPosY = 0.3f;

        [SerializeField] protected SpriteRenderer interactionObjectSprite;
        [SerializeField] private TextMeshProUGUI textElement;

        private static List<Interaction> interactionList = new List<Interaction>();

        private InteractionManager interactionManager;
        public Transform playerTrans;

        private bool IsThisTheClosestOne
        {
            get => this == interactionManager.GetClosestInteraction();
        }
        public bool CanInteract
        {
            get => Utils.GetDistance(transform.position, playerTrans.position) <= distanceToShowInfo && IsEnabled;
        }
        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (!value)
                    RemoveInteraction();

                _isEnabled = value;
            }
        }
        protected virtual void OnDisable()
        {
            RemoveInteraction();
        }
        protected virtual void OnDestroy()
        {
            RemoveInteraction();
        }
        protected virtual void OnValidate()
        {
            if (messageText.Length == 0) 
                messageText = $"Press ''E'' to interact";

            if (!textElement)
                textElement = transform.Find("InteractionTxt").GetComponent<TextMeshProUGUI>();
            SetText();

            //if (!interactionObjectSprite)
            //    interactionObjectSprite = transform.Find("ObjectSprite").GetComponent<SpriteRenderer>();
        }

        protected virtual void Update()
        {
            if (IsEnabled)
                DistanceToInteract();

            textElement.gameObject.SetActive(CanInteract && IsThisTheClosestOne);
        }

        private void DistanceToInteract()
        {
            if (CanInteract && !interactionManager.HasInteraction(this))
                interactionManager.possibleInteractions.Add(this);
            else if (!CanInteract && interactionManager.HasInteraction(this))
                RemoveInteraction();
        }

        public void RemoveInteraction()
        {
            interactionManager.RemoveInteraction(this);
            textElement.gameObject.SetActive(false);
        }

        public abstract void Interact();

        protected virtual void SetText()
        {
            if (interactionObjectSprite && interactionObjectSprite.sprite)
                textElement.transform.position = Utils.Get2DOffset(transform.position, interactionObjectSprite, 0, extraTextPosY);
            textElement.SetText(messageText);
        }
    }
}