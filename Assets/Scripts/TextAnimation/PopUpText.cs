using UnityEngine;
using DG.Tweening;
using TMPro;

namespace GameCore
{
    public class PopUpText : MonoBehaviour
    {
        [SerializeField] private TextMeshPro text;
        PopUpTextData data;

        private void OnValidate()
        {
            if (!text) text = GetComponent<TextMeshPro>();
        }

        /// <summary>
        /// Inicializa os valores e animaçao do texto.
        /// </summary>
        public void Initialize(string textValue, Vector3 initialPosition, PopUpTextData animData)
        {
            data = animData;

            text.SetText(textValue);
            text.color = data.color;

            StartAnimation(initialPosition);
        }

        /// <summary>
        /// Inicia a animaçao do texto.
        /// </summary>
        private void StartAnimation(Vector3 initialPos)
        {
            transform.position = initialPos;

            float yTargetPos = initialPos.y + data.yMove;
            float xTargetPos = Random.Range(0.25f, 1.25f).RandomizeSing() + initialPos.x;

            transform.DOScale(0, data.duration).SetEase(data.sizeCurve);
            transform.DOMoveY(yTargetPos, data.duration).SetEase(data.yMovement);
            transform.DOMoveX(xTargetPos, data.duration);
            text.DOColor(Color.clear, data.duration).SetEase(data.colorCurve).OnComplete(() => Destroy(gameObject));
        }
    }
}
