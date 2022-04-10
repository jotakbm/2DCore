using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    public abstract class DecreaseAttributeBarUi : MonoBehaviour
    {
        [SerializeField] Image attrFillBar;
        [SerializeField] Image attrDecreaseBar;
        [SerializeField] TextMeshProUGUI attrText;
        [SerializeField] float decreaseSpeed = 1;
        [SerializeField] float decreaseWaitingTime = 1;
        float decreaseTimeC;

        private void Update()
        {
            UpdateAttrDecreaseBar();
        }

        /// <summary>
        /// Atualiza a barra de atributo.
        /// </summary>
        protected void UpdateAttrDecreaseBar()
        {
            decreaseTimeC -= Time.deltaTime;

            if (decreaseTimeC <= 0 && attrFillBar.fillAmount < attrDecreaseBar.fillAmount)
                attrDecreaseBar.fillAmount -= decreaseSpeed * Time.deltaTime;
        }

        /// <summary>
        /// Atualiza a barra de atributo.
        /// </summary>
        protected void UpdateAttrBar(UsageAttributeValue attr, bool setDecreaseTime)
        {
            attrText.SetText($"{attr.Value.ToString("00")}/{attr.FinalValue.ToString("00")}");
            attrFillBar.fillAmount = attr.Percentage;

            if (attrFillBar.fillAmount > attrDecreaseBar.fillAmount)
                attrDecreaseBar.fillAmount = attrFillBar.fillAmount;
            else if (setDecreaseTime)
                decreaseTimeC = decreaseWaitingTime;
        }
    }
}
