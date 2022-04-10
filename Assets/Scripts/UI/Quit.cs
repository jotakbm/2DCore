using UnityEngine;

namespace GameCore
{
    public class Quit : MonoBehaviour
    {
        /// <summary>
        /// Fecha o aplicativo.
        /// </summary>
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}