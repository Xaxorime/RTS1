using UnityEngine;
//Al usar este Singleton en otras clases se puede añadir la siguiente línea en el código
//protected override bool Persistent => true;
//para hacer que el singleton sea persistente entre escenas
//protected override bool Persistent => false;
//para hacer que se destruya entre cambios de escenas, poe defecto el Persistent será 'false'
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual bool Persistent => false;

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;

        if (Persistent)
            DontDestroyOnLoad(gameObject);
    }
}