using UnityEngine;
using UnityEngine.EventSystems;

namespace GameCore
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] protected string title;
        [TextArea(5,10)]
        [SerializeField] protected string description;

        #region PointerEventData
        public void OnPointerEnter(PointerEventData eventData)
        {
            Tooltip.OnCallToolTip?.Invoke(title, description);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Tooltip.OnEndToolTip?.Invoke();
        }
        #endregion

        /// <summary>
        /// Define novos valores para as informaçoes do tooltip.
        /// </summary>
        public void SetText(string title, string description)
        {
            this.title = title;
            this.description = description;
        }

        /// <summary>
        /// Invoca o evento para desativar o tooltip.
        /// </summary>
        public void CloseToolTip()
        {
            Tooltip.OnEndToolTip?.Invoke();
        }
    }
}
