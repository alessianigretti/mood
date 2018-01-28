using UnityEngine;
using System.Collections;

namespace TeamTheDream
{
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        static bool _destroyed = false;

        static T _instance = null;

        /// <summary>
        /// Get Singleton Instance
        /// </summary>
        public static T Instance
        {
            get
            {
                CreateInstance();

                return _instance;
            }
        }

        /// <summary>
        /// False if instance does not exists or has been destroyed
        /// </summary>
        public static bool HasInstance
        {
            get { return _instance != null; }
        }

        static void CreateInstance()
        {
            if (!_destroyed && _instance == null)
            {
                _instance = FindObjectOfType<T>();

                if (_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    _instance = go.AddComponent<T>();
                }
            }
        }

        /// <summary>
        /// Override Awake if you use it in children classes and call to base.Awake();
        /// </summary>
        virtual protected void Awake() {
            CreateInstance();
        }

        void OnApplicationQuit()
        {
            _instance = null;
            _destroyed = true;
        }
    }
}