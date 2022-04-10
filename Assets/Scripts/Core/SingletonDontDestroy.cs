using UnityEngine;

namespace GameCore
{
    /// <summary>
    /// Torna a classe um Singleton e aplica o metodo DontDestroyOnLoad em si.
    /// </summary>
    [DisallowMultipleComponent]
    public abstract class SingletonDontDestroy<T> : Singleton<T> where T : Singleton<T>
    {
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(this);
        }
    }
}