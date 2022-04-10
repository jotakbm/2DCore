using Cinemachine;
using UnityEngine;
using System;

namespace GameCore
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraBehaviors : MonoBehaviour
    {
        /// <summary>
        /// Params: Intesity, Time
        /// </summary>
        public static Action<float, float> OnCameraShake;

        [SerializeField] CinemachineVirtualCamera virtualCamera;
        private CinemachineBasicMultiChannelPerlin perlinNoise;
        float shakeTimer;
        float totalTime;
        float intensity;

        #region UnityCallback
        private void OnValidate()
        {
            if (virtualCamera == null)
                virtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        private void Awake()
        {
            perlinNoise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        private void OnEnable()
        {
            OnCameraShake += ShakeCamera;
        }

        private void OnDisable()
        {
            OnCameraShake -= ShakeCamera;
        }

        private void Update()
        {
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;
                perlinNoise.m_AmplitudeGain = Mathf.Lerp(intensity, 0f, 1 - (shakeTimer / totalTime));
            }
        }
        #endregion

        /// <summary>
        /// Agita a camera por um tempo e intensidade definida.
        /// </summary>
        public void ShakeCamera(float intensity, float time)
        {
            this.totalTime = time;
            this.intensity = intensity;
            shakeTimer = time;
        }
    }
}
