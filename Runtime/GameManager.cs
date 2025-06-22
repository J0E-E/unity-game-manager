using System;
using System.Reflection;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // GameManager Instance
    public static GameManager Instance;

    // Establish the singleton pattern and DDOL.
    private void Awake()
    {
        // If the instance is set, this is a duplicate, BAIL!
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // Set the instance.`
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Register THIS manager. Duh!
        ManagerLocator.Register<GameManager>(this);

        // Register all the other managers. 
        RegisterManagers();
    }

    private void RegisterManagers()
    {
        var scripts = GetComponents<Manager>();

        foreach (var script in scripts)
        {
            var type = script.GetType();
            var methodInfo = typeof(ManagerLocator)
                .GetMethod("Register", BindingFlags.Public | BindingFlags.Static);
            
            if (methodInfo == null)
            {
                Debug.LogError("Register method not found on ManagerLocator.");
                return;
            }

            var registerMethod = methodInfo.MakeGenericMethod(type);
            registerMethod.Invoke(null, new object[] { script });
            Debug.Log($"Registered manager: {type.Name}");

            script.Loaded();
        }
    }
}
