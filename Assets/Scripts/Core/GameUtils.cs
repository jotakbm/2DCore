using UnityEngine;

namespace GameCore
{
    public class GameUtils : SingletonDontDestroy<GameUtils>
    {
        public Camera MainCamera { get; private set; }

        // Refazer pelo novo sistema de input!
        public Vector2 MousePos { get => MainCamera.ScreenToWorldPoint(Input.mousePosition); }

        #region UnityCallback
        protected override void Awake()
        {
            base.Awake();
        }
        #endregion

    }
}