using System;
using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// Atributo com utilização de valor. Geralmente é utilizado por atributos como Vida, Mana, Energia, etc.
    /// </summary>
    [Serializable]
    public class UsageAttributeValue : AttributeValue
    {
        [SerializeField] private float _useValue;

        public UsageAttributeValue(float baseValue) : base(baseValue)
        {
            _useValue = baseValue;
        }

        /// <summary>
        /// Valor de uso atual.
        /// </summary>
        public float Value
        {
            get { return _useValue; }
            set
            {
                if (value < 0) 
                    value = 0;
                else if (value > FinalValue) 
                    value = FinalValue;

                _useValue = value;
            }
        }

        /// <summary>
        /// Retorna quantos porcentos está o UseValue em relação a Value (valor máximo).
        /// </summary>
        public float Percentage => _useValue / FinalValue;

        /// <summary>
        /// Retorna verdadeiro se UseValue for igual a Value (valor máximo).
        /// </summary>
        public bool IsFull => _useValue >= FinalValue;

        /// <summary>
        /// Retorna verdadeiro se UseValue for maior que zero.
        /// </summary>
        public bool HasValue => _useValue > 0;

        /// <summary>
        /// Torna UseValue igual FinalValue (valor máximo).
        /// </summary>
        public void SetToMaxValue() => _useValue = FinalValue;

        /// <summary>
        /// Chama a propriedade UseValue e passa seu próprio valor como referencia para atualiza-lo. Este método deve ser utilizado após diminuir o Value
        /// para um valor inferior ao UseValue, garantindo que o valor atual do UseValue nunca seja maior que Value.
        /// </summary>
        public void RefreshValue() => Value = Value;
    }
}