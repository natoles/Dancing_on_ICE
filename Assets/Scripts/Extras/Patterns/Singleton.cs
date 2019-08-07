/// Code inspired from http://wiki.unity3d.com/index.php/Singleton and http://www.bivis.com.br/2016/06/07/unity-creating-singleton-from-prefab/

using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    // Check to see if we're about to be destroyed.
    private static bool _shuttingDown = false;
    private static object _lock = new object();
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_shuttingDown)
            {
                Debug.LogWarning("[Singleton] Instance '" + typeof(T) +
                    "' already destroyed. Returning null.");
                return null;
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    // Search for existing instance.
                    _instance = (T)FindObjectOfType(typeof(T));

                    // Create new instance if one doesn't already exist.
                    if (_instance == null)
                    {
                        GameObject singletonPrefab = null;
                        GameObject singleton = null;

                        // Check if exists a singleton prefab on Resources Folder.
                        // -- Prefab must have the same name as the Singleton SubClass
                        singletonPrefab = Resources.Load<GameObject>(typeof(T).Name);

                        // Create singleton as new or from prefab
                        if (singletonPrefab != null)
                        {
                            singleton = Instantiate(singletonPrefab);
                            _instance = singleton.GetComponent<T>();
                        }
                        else
                        {
                            // Need to create a new GameObject to attach the singleton to.
                            singleton = new GameObject();
                            _instance = singleton.AddComponent<T>();
                        }

                        singleton.name = typeof(T).ToString() + " (Singleton)";

                        // Make instance persistent.
                        DontDestroyOnLoad(singleton);
                    }
                }

                return _instance;
            }
        }
    }


    private void OnApplicationQuit()
    {
        _shuttingDown = true;
    }


    private void OnDestroy()
    {
        _shuttingDown = true;
    }
}