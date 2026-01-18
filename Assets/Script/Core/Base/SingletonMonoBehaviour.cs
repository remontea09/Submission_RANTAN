using UnityEngine;

namespace Base {
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour {
        private static T _instance;

        public static T Instance {
            get {
                if (_instance == null) {
                    Debug.LogError($"{typeof(T)} がシーン内に見つかりません。");
                }
                return _instance;
            }
        }

        protected virtual void Awake() {
            if (_instance == null) {
                _instance = this as T;
                DontDestroyOnLoad(this.gameObject);
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}
