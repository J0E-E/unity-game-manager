using System;
using System.Reflection;
using UnityEngine;

public abstract class GameManager : MonoBehaviour
{
    // GameManager Instance
    public static GameManager Instance;

    // Establish the singleton pattern and DDOL.
    protected virtual void Awake()
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
        RegisterSelf();

        // Register all the other managers. 
        RegisterManagers();
    }

    /// <summary>
    /// Abstract method intended to register the current manager instance with the application.
    /// Concrete implementations of this method should handle the specific registration logic for an inheriting
    /// manager class. This is typically used to ensure the manager is properly accessible via a manager locator
    /// or similar service management structure.
    ///
    /// <para>Register via: <c>ManagerLocator.Register&lt;GameManager&gt;(this);</c></para>
    /// </summary>
    protected abstract void RegisterSelf();

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
