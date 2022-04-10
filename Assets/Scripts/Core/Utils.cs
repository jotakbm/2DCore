using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore
{
    public static class Utils
    {
        /// <summary>
        /// Torna a classe um Singleton.
        /// </summary>
        public static void SetSingleton<T>(GameObject go, ref T Instance, T classRef, bool ddol) where T : class
        {
            if (Instance != null && Instance != classRef)
            {
                Object.Destroy(go);
                return;
            }

            Instance = classRef;

            if (ddol)
                Object.DontDestroyOnLoad(go);
        }

        /// <summary>
        /// Retorna um vetor3 com adiçao de offsets.
        /// </summary>
        public static Vector3 Get3DOffset(Vector3 pos, float xOff, float yOff, float zOff)
        {
            Vector3 offset = pos;
            offset.x += xOff;
            offset.y += yOff;
            offset.z += zOff;
            return offset;
        }

        /// <summary>
        /// Retorna um vetor3 com adiçao de offsets baseado no tamanho y do render.
        /// </summary>
        public static Vector3 Get3DOffset(Vector3 pos, Renderer renderer, float xOff, float yOff, float zOff)
        {
            Vector3 offset = pos;
            offset.x += xOff;
            offset.y += yOff;
            offset.z += renderer.bounds.size.z / 2 + zOff;
            return offset;
        }

        /// <summary>
        /// Retorna um vetor2 com adiçao de offsets.
        /// </summary>
        public static Vector2 Get2DOffset(Vector2 pos, float xOff, float yOff)
        {
            Vector2 offset = pos;
            offset.x += xOff;
            offset.y += yOff;
            return offset;
        }

        /// <summary>
        /// Retorna um vetor2 com adiçao de offsets baseado no tamanho y do render.
        /// </summary>
        public static Vector2 Get2DOffset(Vector2 pos, SpriteRenderer renderer, float xOff, float yOff)
        {
            Vector2 offset = pos;
            offset.x += xOff;
            offset.y += renderer.sprite.bounds.size.y / 2 + yOff;
            return offset;
        }

        /// <summary>
        /// Retorna a distancia entre dois vetores. (Mais otimizado que o padrao da unity)
        /// </summary>
        public static float GetDistance(Vector2 a, Vector2 b)
        {
            return Vector2.SqrMagnitude(b - a);
        }

        /// <summary>
        /// Retorna uma direçao randomica.
        /// </summary>
        public static Vector2 GetRandomDir()
        {
            return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        /// <summary>
        /// Retorna a direçao normalizada da posiçao ao alvo.
        /// </summary>
        public static Vector3 GetDirection(Vector3 position, Vector3 target)
        {
            return (target - position).normalized;
        }

        /// <summary>
        /// Retorna a direçao normalizada da posiçao ao alvo multiplicado por um número.
        /// </summary>
        public static Vector3 GetDirection(Vector3 position, Vector3 target, float power)
        {
            return (target - position).normalized * power;
        }

        /// <summary>
        /// Retorna uma booleana com valor aleatório.
        /// </summary>
        public static bool GetRandomBoolean()
        {
            return Random.value > 0.5f;
        }


        /// <summary>
        /// Retorna um valor igual a 1 ou -1.
        /// </summary>
        public static int GetRandomSing()
        {
            return Random.value < .5 ? 1 : -1;
        }

        /// <summary>
        /// Retorna True caso o valor passado seja maior ou igual a um valor randomico entre 0 a 1.
        /// </summary>
        public static bool GetChance(float value)
        {
            return value >= Random.value;
        }

        /// <summary>
        /// Transforma radiano em Vector2.
        /// </summary>
        public static Vector2 RadianToVector2(float radian)
        {
            return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
        }

        /// <summary>
        /// Transforma graus em Vector2.
        /// </summary>
        public static Vector2 DegreeToVector2(float degree)
        {
            return RadianToVector2(degree * Mathf.Deg2Rad);
        }

        /// <summary>
        /// Incrementa x graus a rotaçao do vetor.
        /// </summary>
        public static Vector2 IncreaseAngleToVector2(Vector2 iniVec, float degree)
        {
            return Quaternion.Euler(0, 0, degree) * iniVec;
        }

        /// <summary>
        /// Retorna o angulo em float de uma direçao entre 0 e 360.
        /// </summary>
        public static float GetVectorFloatAngle(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        /// <summary>
        /// Retorna o angulo em int de uma direçao entre 0 e 360.
        /// </summary>
        public static int GetVectorIntAngle(Vector3 dir)
        {
            dir = dir.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        /// <summary>
        /// Retorna um vetor3 com adiçao de offsets.
        /// </summary>
        public static int ClampIntToZero(int value)
        {
            return (int)Mathf.Clamp(value, 0, float.MaxValue);
        }


        /// <summary>
        /// Remove o objeto do "DontDestroyOnLoad", voltando a ser destruído ao transitar entre cenas.
        /// </summary>
        public static void DestroyOnLoad(GameObject targetGo)
        {
            SceneManager.MoveGameObjectToScene(targetGo, SceneManager.GetActiveScene());
        }

        /// <summary>
        /// Retorna uma soma de booleanas.
        /// </summary>
        public static int CountTrue(params bool[] args)
        {
            return args.Count(t => t);
        }

        #region UnityEditor
#if UNITY_EDITOR
        /// <summary>
        /// Confere se é possivel, e Renomeia um ScriptableObject.
        /// </summary>
        public static void RenameScriptableObject(string originalName, string desiredName, ScriptableObject obj)
        {
            if (!EditorApplication.isPlaying && !originalName.Contains("Clone") && desiredName != null && desiredName.Length > 0 &&
                originalName != $"SO_{desiredName}")
            {
                string assetPath = AssetDatabase.GetAssetPath(obj.GetInstanceID());
                if (System.IO.File.Exists(RemoveFinalPath(assetPath) + $"/SO_{desiredName}.asset"))
                {
                    Debug.LogWarning($"{desiredName} - already exists!", obj);
                }
                else
                {
                    Debug.Log($"The name of {obj} - {originalName} has changed to {desiredName}", obj);

                    AssetDatabase.RenameAsset(assetPath, $"SO_{desiredName}");
                    AssetDatabase.SaveAssets();
                    AssetDatabase.Refresh();
                }
            }
        }

        /// <summary>
        /// Confere se é necessário, e renomeia um GameObject.
        /// </summary>
        public static void RenameSceneObject(GameObject go, string newName)
        {
            if (go.name != newName && PrefabUtility.GetPrefabInstanceStatus(go) != PrefabInstanceStatus.NotAPrefab)
                go.name = newName;
        }

        /// <summary>
        /// Remove os ultimos caracteres de um Path até o ultimo "/". [Exemplo: Exemplo/Pasta/Nome.txt = Exemplo/Pasta/].
        /// </summary>
        private static string RemoveFinalPath(string path)
        {
            int index = path.LastIndexOf("/");
            return (index > 0) ? path.Substring(0, index + 1) : path;
        }
#endif
        #endregion

        #region LayerMask
        /// <summary>
        /// Retorna True se a layer está contida na LayerMask.
        /// </summary>
        public static bool IsInLayerMask(LayerMask mask, LayerMask layer)
        {
            return (mask & 1 << layer) == 1 << layer;
        }
        #endregion

        #region Extensions

        #region int
        public static float RandomizeSing(this int value)
        {
            return value *= GetRandomSing();
        }
        #endregion

        #region float
        public static float RandomizeSing(this float value)
        {
            return value *= GetRandomSing();
        }
        #endregion

        #region Transform

        /// <summary>
        /// Resseta a posiçao, rotaçao e escala para o valor padrao.
        /// </summary>
        public static void ResetTransformation(this Transform trans)
        {
            trans.position = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = new Vector3(1, 1, 1);
        }

        /// <summary>
        /// Destroi todos os objetos filhos.
        /// </summary>
        public static void DestroyAllChildrens(this Transform parent)
        {
            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                Object.Destroy(parent.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Ativa ou desativa todos os objetos filhos.
        /// </summary>
        public static void SetAllChildrenEnabled(this Transform parent, bool enabled)
        {
            for (int i = 0; i < parent.childCount; i++)
            {
                GameObject child = parent.GetChild(i).gameObject;
                child.SetActive(enabled);
            }
        }
        #endregion

        #region GameObject
        /// <summary>
        /// Checa se o GameObject tem um componente específico.
        /// </summary>
        public static bool HasComponent<T>(this GameObject go) where T : Component
        {
            return go.GetComponent<T>();
        }

        /// <summary>
        /// Ativa ou desativa todos os componentes do GameObject.
        /// </summary>
        public static void SetAllComponentsEnabled(this GameObject go, bool acive)
        {
            MonoBehaviour[] itemz = go.GetComponents<MonoBehaviour>();
            for (int i = 0; i < itemz.Length; i++)
            {
                MonoBehaviour c = itemz[i];
                c.enabled = acive;
            }
        }
        #endregion

        #region Collider2D
        /// <summary>
        /// Checa se o gameobject do collider2D está na layer.
        /// </summary>
        public static bool IsOnLayer(this Collider2D collider, LayerMask mask)
        {
            return collider.gameObject.layer == mask;
        }

        /// <summary>
        /// Checa se o gameobject do collider2D está na layer.
        /// </summary>
        public static bool IsOnLayer(this Collider2D collider, string maskName)
        {
            return collider.gameObject.layer == LayerMask.NameToLayer(maskName);
        }
        #endregion

        #region RaycastHit2D
        /// <summary>
        /// Checa se o gameobject atingido está na layer.
        /// </summary>
        public static bool IsOnLayer(this RaycastHit2D hit, LayerMask mask)
        {
            return hit.collider.gameObject.layer == mask;
        }

        /// <summary>
        /// Checa se o gameobject atingido está na layer.
        /// </summary>
        public static bool IsOnLayer(this RaycastHit2D hit, string maskName)
        {
            return hit.collider.gameObject.layer == LayerMask.NameToLayer(maskName);
        }

        /// <summary>
        /// Retorna a Layer do objeto atingido.
        /// </summary>
        public static LayerMask GetLayer(this RaycastHit2D hit)
        {
            return hit.collider.gameObject.layer;
        }
        #endregion

        #region Vector3
        /// <summary>
        /// Retorna a direçao normalizada até um alvo.
        /// </summary>
        public static Vector3 GetDirTo(this Vector3 vector, Vector3 target)
        {
            return (target - vector).normalized;
        }

        /// <summary>
        /// Retorna o angulo em float entre 0 e 360.
        /// </summary>
        public static float GetFloatAngle(this Vector3 vector)
        {
            vector = vector.normalized;
            float n = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        /// <summary>
        /// Retorna o angulo em int entre 0 e 360.
        /// </summary>
        public static int GetIntAngle(this Vector3 vector)
        {
            vector = vector.normalized;
            float n = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        /// <summary>
        /// Rotaciona o vetor em angulos, e retorna um novo Vector3.
        /// </summary>
        public static Vector3 ApplyRotationToVector(this Vector3 vector, float angle)
        {
            return Quaternion.Euler(0, 0, angle) * vector;
        }

        /// <summary>
        /// Retorna um novo Vector3 com X alterado e demais valores identicos.
        /// </summary>
        public static Vector3 SetVectorXValue(this Vector3 vector, float newXvalue = 0)
        {
            return new Vector3(newXvalue, vector.y, vector.z);
        }

        /// <summary>
        /// Retorna um novo Vector3 com Y alterado e demais valores identicos.
        /// </summary>
        public static Vector3 SetVectorYValue(this Vector3 vector, float newYvalue = 0)
        {
            return new Vector3(vector.x, newYvalue, vector.z);
        }

        /// <summary>
        /// Retorna um novo Vector3 com Z alterado e demais valores identicos.
        /// </summary>
        public static Vector3 SetVectorZValue(this Vector3 vector, float newZvalue = 0)
        {
            return new Vector3(vector.x, vector.y, newZvalue);
        }
        #endregion

        #region Vector2
        /// <summary>
        /// Retorna o angulo em float entre 0 e 360.
        /// </summary>
        public static float GetFloatAngle(this Vector2 vector)
        {
            vector = vector.normalized;
            float n = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;

            return n;
        }

        /// <summary>
        /// Retorna o angulo em int entre 0 e 360.
        /// </summary>
        public static int GetIntAngle(this Vector2 vector)
        {
            vector = vector.normalized;
            float n = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
            if (n < 0) n += 360;
            int angle = Mathf.RoundToInt(n);

            return angle;
        }

        /// <summary>
        /// Retorna a direçao normalizada até um alvo.
        /// </summary>
        public static Vector2 GetDirTo(this Vector2 vector, Vector2 target)
        {
            return (target - vector).normalized;
        }
        #endregion

        #region Color
        public static string GetTMPColor(this Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>";
        }
        #endregion

        /// <summary>
        /// Retorna um objeto aleatório da lista caso haja algum.
        /// </summary>
        public static T GetRandom<T>(this T[] sequence)
        {
            return sequence.Length > 0 ? sequence[Random.Range(0, sequence.Length)] : default;
        }

        #region List
        /// <summary>
        /// Retorna um objeto aleatório da lista caso haja algum.
        /// </summary>
        public static T GetRandom<T>(this List<T> sequence)
        {
            return sequence.Count > 0 ? sequence[Random.Range(0, sequence.Count)] : default;
        }

        /// <summary>
        /// Retorna a quantidade de objetos de uma lista menos 1. O valor nunca retorna menor do que 0.
        /// </summary>
        public static int CountLessOne<T>(this List<T> sequence)
        {
            return (int)Mathf.Clamp(sequence.Count - 1, 0, float.MaxValue);
        }

        /// <summary>
        /// Retorna verdadeir se a quantidade de itens da lista for igual sua capacidade.
        /// </summary>
        public static bool IsFull<T>(this List<T> sequence)
        {
            return sequence.Count == sequence.Capacity;
        }

        /// <summary>
        /// Retorna a quantidade de espaços restantes em uma lista, limitado por sua capacidade.
        /// </summary>
        public static int RemainingSpace<T>(this List<T> sequence)
        {
            return sequence.Capacity - sequence.Count;
        }

        public static List<T> ShuffledOrder<T>(this List<T> pile)
        {
            System.Random random = new System.Random();
            return pile.OrderBy(x => random.Next()).ToList();
        }
        #endregion

        #endregion
    }
}