using UnityEngine;

public class SingletoneBase<T> : MonoBehaviour where T : MonoBehaviour
{
    private static volatile T _instance = null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)) as T;
            }
            return _instance;
        }
    }
}
