using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace GameCore
{
    [Serializable]
    public class AttributeValue
    {
        [SerializeField] private float _baseValue;
        private readonly List<AttributeValueMod> statModifiers;
        public readonly ReadOnlyCollection<AttributeValueMod> StatModifiers;
        private bool isDirty = true;
        private float _value;

        ///<Summary>
        /// Retorna o valor inicial do atributo.
        ///</Summary>
        public float BaseValue
        {
            get => _baseValue; private set => _baseValue = value;
        }

        ///<Summary>
        /// Retorna o valor calculado do atributo com todos seus modificadores.
        ///</Summary>
        public float FinalValue
        {
            get
            {
                if (isDirty)
                {
                    _value = CalculateFinalValue<AttributeValueMod>();
                    isDirty = false;
                }

                return _value;
            }
        }

        public AttributeValue()
        {
            statModifiers = new List<AttributeValueMod>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public AttributeValue(float baseValue) : this()
        {
            BaseValue = baseValue;
        }

        #region Modifiers
        /// <summary>
        /// Adiciona um novo modificador ao atributo.
        /// </summary>
        public void AddModifier(AttributeValueMod modifier)
        {
            if (modifier != null && modifier.value != 0)
            {
                isDirty = true;
                statModifiers.Add(modifier);
            }
        }

        /// <summary>
        /// Remove um modificador específico do atributo.
        /// </summary>
        public bool RemoveModifier(AttributeValueMod modifier)
        {
            if (statModifiers.Remove(modifier))
            {
                isDirty = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Remove todos os modificadores deste atributo, providos pela mesma Source.
        /// </summary>
        public bool RemoveAllModifierFromSource(object source)
        {
            bool didRemove = false;
            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                AttributeValueMod mod = statModifiers[i];
                if (mod.source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        ///<Summary>
        /// Checa se um modificador identico ao passado já existe.
        ///</Summary>
        public bool HasEqualMod(AttributeValueMod modifier)
        {
            return statModifiers.Exists(x => x == modifier);
        }

        ///<Summary>
        /// Adiciona um modificador caso não haja outro identico a ele.
        ///</Summary>
        public void AddUnicMod(AttributeValueMod modifier)
        {
            if (!HasEqualMod(modifier))
            AddModifier(modifier);
        }

        #endregion

        #region Calculation
        /// <summary>
        /// Calcula o valor final com a soma de todos os seus modificadores.
        /// </summary>
        private float CalculateFinalValue<T>() where T : AttributeValueMod
        {
            float finalValue = BaseValue;
            for (int i = 0; i < statModifiers.Count; i++)
            {
                AttributeValueMod mod = (T)statModifiers[i];

                switch (mod.type)
                {
                    case AttributeValueMod.StatModType.Flat:
                        finalValue += mod.value;
                        break;
                    case AttributeValueMod.StatModType.Percent:
                        finalValue += BaseValue * mod.value;
                        break;
                    case AttributeValueMod.StatModType.PercentOverAll:
                        finalValue += (BaseValue + GetAllFlat()) * mod.value;
                        break;
                }
            }
            return (float)Math.Round(finalValue, 4);
        }

        /// <summary>
        /// Retorna o valor de todos os modificadores do tipo "Flat".
        /// </summary>
        private float GetAllFlat()
        {
            float finalValue = 0;
            for (int i = 0; i < statModifiers.Count; i++)
            {
                AttributeValueMod mod = statModifiers[i];

                if (mod.type == AttributeValueMod.StatModType.Flat)
                    finalValue += mod.value;
            }

            return finalValue;
        }
        #endregion
    }
}