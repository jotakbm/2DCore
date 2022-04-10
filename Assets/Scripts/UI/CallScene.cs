using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore
{
    public class CallScene : MonoBehaviour
    {
        /// <summary>
        /// Inicia uma nova cena com tela de load.
        /// </summary>
        public void StartNewSceneWithLoadScreen(string sceneName)
        {
            SceneManager.LoadScene("LoadScreen", LoadSceneMode.Additive);
            StartCoroutine(LoadNewScene(sceneName));
        }

        /// <summary>
        /// Inicia uma nova cena sem tela de load.
        /// </summary>
        public void StartNewScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// Processos relacionados a tela de load.
        /// </summary>
        IEnumerator LoadNewScene(string sceneName)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitWhile(() => !asyncLoad.isDone);
        }
    }
}