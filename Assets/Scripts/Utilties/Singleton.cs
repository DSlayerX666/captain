using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCells.Utilities
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<T>(); //new GameObject(typeof(T).ToString()).AddComponent<T>();
                return instance;
            }
        }
        
        private void OnApplicationQuit()
        {
            RemoveInstance();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RemoveInstance()
        {
            instance = null;
        }

        protected void SetDontDestroyOnLoad()
        {
            DontDestroyOnLoad(Instance.gameObject);
        }
    }
}