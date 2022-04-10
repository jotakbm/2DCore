using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// Torna a classe um Singleton.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance;
        public static T Instance { get => _instance; }
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            _instance = (T)this;
        }
    }
}