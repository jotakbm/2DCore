using UnityEngine;

namespace GameCore
{
    public class Main : Singleton<Main>
    {
        /// <summary>
        /// Instancia o prefab "Main" da pasta Rescources.
        /// <para>Neste objeto deverao ficar todas as configura�oes e valores que sempre devem ser chamados ao iniciar o jogo.
        /// Desta forma, � dispensavel uma cena inicial para configura�oes e evita possiveis bugs por iniciar o aplicativo por uma cena sem tais configura�oes.</para>
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void LoadMain()
        {
            GameObject main = Instantiate(Resources.Load("Main")) as GameObject;
            DontDestroyOnLoad(main);
        }
    }
}