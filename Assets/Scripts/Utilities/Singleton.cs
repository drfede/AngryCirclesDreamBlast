using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngryCirclesDreamBlast.Utilities
{
    public abstract class Singleton<T> : MonoBehaviour
       where T : Component
    {

        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var objs = FindObjectsOfType(typeof(T)) as T[];
                    if (objs.Length == 1)
                        _instance = objs[0];
                    if (objs.Length > 1)
                        Debug.LogError("More than one " + typeof(T).Name + " in the scene.");
                    if (_instance == null)
                    {
                        GameObject obj = new GameObject();
                        _instance = obj.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        public virtual void Awake() { }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }

    }


    public abstract class SingletonPersistent<T> : MonoBehaviour
        where T : Component
    {
        public static T Instance { get; private set; }

        public virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }
    }

}