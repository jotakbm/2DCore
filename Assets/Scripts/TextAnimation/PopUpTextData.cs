using UnityEngine;

namespace GameCore
{
    [CreateAssetMenu(fileName = "PopText_SO", menuName = "Animations/PopUpText")]
    public class PopUpTextData : ScriptableObject
    {
        public float yMove;
        public float duration;
        public AnimationCurve sizeCurve = AnimationCurve.Constant(0, 1, 1);
        public AnimationCurve colorCurve = AnimationCurve.Constant(0, 1, 1);
        public AnimationCurve yMovement = AnimationCurve.Constant(0, 1, 1);

        public Color color = Color.white;
    }
}
