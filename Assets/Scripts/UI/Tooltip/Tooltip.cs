using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System;

namespace GameCore
{
    [DisallowMultipleComponent, RequireComponent(typeof(RectTransform))]
    public class Tooltip : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI titleText;
        [SerializeField] TextMeshProUGUI descriptionText;
        [SerializeField] LayoutElement layoutElement;
        [SerializeField] RectTransform rectTransform;
        [SerializeField] CanvasGroup canvasGroup;
        [SerializeField] float offset;

        public static Action<string, string> OnCallToolTip;
        public static Action OnEndToolTip;

        #region Unity Callback
        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            OnCallToolTip += EnableTooltip;
            OnEndToolTip += DisableTooltip;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            OnCallToolTip -= EnableTooltip;
            OnEndToolTip -= DisableTooltip;
        }

        private void Update()
        {
            SetPosition();
        }
        #endregion

        /// <summary>
        /// Define a posiçao atual do tooltip.
        /// </summary>
        private void SetPosition()
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 pivot = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
            Vector2 finalPivot = new Vector2(pivot.x > 0.5f ? 1 + offset : 0 - offset, pivot.y > 0.5f ? 1 + offset : 0 - offset);

            rectTransform.pivot = finalPivot;
            transform.position = position;
        }

        /// <summary>
        /// Habilita o tooltip e atualiza seus dados.
        /// </summary>
        private void EnableTooltip(string title, string description)
        {
            SetText(title, description);
        }

        /// <summary>
        /// Desabilita o tooltip.
        /// </summary>
        private void DisableTooltip()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// Atualiza os dados do tooltip.
        /// </summary>
        private void SetText(string title, string description)
        {
            canvasGroup.alpha = 0;
            canvasGroup.DOKill();

            gameObject.SetActive(true);

            if (string.IsNullOrEmpty(title))
                titleText.gameObject.SetActive(false);
            else
            {
                titleText.gameObject.SetActive(true);
                titleText.SetText(title);
            }

            descriptionText.SetText(description);
            canvasGroup.DOFade(1, 0.5f);

            transform.DOScale(1, 0.05f).OnComplete(() => LayoutRebuilder.MarkLayoutForRebuild(rectTransform));

            //ESSE PROCESSO PODE SER CUSTUSO! Há alternativa?
            LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
        }
    }
}
