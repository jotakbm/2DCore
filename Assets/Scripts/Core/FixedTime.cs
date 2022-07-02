using System.Collections;
using UnityEngine;

namespace GameCore
{
    public static class FixedTime
    {
        private static float _timeScale = 1;
        private static float _lastTimeScale = 1;
        private static bool isFreezeTime;

        /// <summary>
        /// Retorna a ultima escala de tempo do jogo. Utilizado para despausar.
        /// </summary>
        private static float LastTimeScale
        {
            get => _lastTimeScale;
            set => _lastTimeScale = (value <= 0) ? 1 : value;
        }

        /// <summary>
        /// Retorna o valor atual da escala de tempo do jogo.
        /// </summary>
        public static float LocalTimeScale
        {
            get => _timeScale;
            set
            {
                if (value < 0) { value = 0; }
                else if (value > 60) { value = 60; }
                _timeScale = value;
            }
        }

        /// <summary>
        /// Retorna verdadeiro se o jogo estiver pausado e não congelado.
        /// </summary>
        public static bool IsPaused => LocalTimeScale == 0f && !isFreezeTime;

        /// <summary>
        /// Retorna verdadeiro se o jogo estiver congelado e não pausado.
        /// </summary>
        public static bool IsFreezed => LocalTimeScale == 0f && isFreezeTime;
        /// <summary>
        /// Retorna Time.deltaTime * a escala de tempo do jogo.
        /// </summary>
        public static float DeltaTime => Time.deltaTime * LocalTimeScale;

        /// <summary>
        /// Retorna Time.timeScale * a escala de tempo do jogo.
        /// </summary>
        public static float TimeScale => Time.timeScale * LocalTimeScale;

        /// <summary>
        /// Pausa o jogo, tornando sua escala de tempo igual a 0. Apenas objetos que utilizam o FixedTime sao afetados.
        /// </summary>
        public static void PauseGame() => PauseGame(false);

        /// <summary>
        /// Despausa o jogo e retorna a escala de tempo normal.
        /// </summary>
        public static void UnPauseGame() => UnPauseGame(false);

        /// <summary>
        /// Pausa o jogo, tornando sua escala de tempo igual a 0. Apenas objetos que utilizam o FixedTime sao afetados.
        /// </summary>
        private static void PauseGame(bool isFreez)
        {
            isFreezeTime = isFreez;

            if (!IsPaused)
            {
                LastTimeScale = LocalTimeScale;
                LocalTimeScale = 0;
            }
        }

        /// <summary>
        /// Despausa o jogo e retorna a escala de tempo normal.
        /// </summary>
        private static void UnPauseGame(bool isFreeze)
        {
            if (isFreezeTime || !isFreeze)
            {
                isFreezeTime = false;
                LocalTimeScale = LastTimeScale;
            }
        }

        ///// <summary>
        ///// Congela o jogo pelo tempo determinado.
        ///// </summary>
        public static void FreezeGame(float time)
        {
            if (IsFreezed)
                return;

            Main.Instance.StartCoroutine(FreezeTime(time));
        }

        private static IEnumerator FreezeTime(float time)
        {
            isFreezeTime = true;
            Time.timeScale = 0;
            yield return new WaitForSecondsRealtime(time);
            Time.timeScale = 1;
            isFreezeTime = false;
        }
    }
}