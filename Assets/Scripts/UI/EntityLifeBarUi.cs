using UnityEngine;

namespace GameCore
{
    public class EntityLifeBarUi : DecreaseAttributeBarUi
    {
        /// <summary>
        /// Atualiza o valor da barra de vida.
        /// </summary>
        public void UpdateLife(UsageAttributeValue life, bool setDecreaseTime)
        {
            UpdateAttrBar(life, setDecreaseTime);
        }
    }
}
