using System;
using System.Collections.Generic;

/// <summary>
/// Interface to help identify Manager component scripts.
/// </summary>
public interface IManager { };

/// <summary>
///  A class to facilitate registering instances of managers that will be used across the application. 
/// </summary>
public class ManagerLocator
{
    // The dictionary to store the managers. Managers are expected to be their own class objects.
    private static Dictionary<Type, object> _managers = new();

    // Register Manager
    public static void Register<T>(T manager) where T: class
    {
        // Get the type, to use as the key for the dictionary.
        var type = typeof(T);
        
        // Manager cannot be null. 
        if (manager == null)
        {
            throw new ArgumentNullException(nameof(manager), $"Can't register null for type {type}");
        }

        // Using TryAdd to catch duplicate types from being added. 
        if (!_managers.TryAdd(type, manager))
        {
            throw new InvalidOperationException($"Service of type {type} is already registered.");
        }
    }
    
    // Get Manager
    public static T Get<T>() where T: class
    {
        var type = typeof(T);
        if (_managers.TryGetValue(type, out var manager))
            return manager as T;

        throw new InvalidOperationException($"Service of type {type} is not registered.");
    }
    
    // Deregister Manager
    public static bool Deregister<T>() where T: class
    {
        return _managers.Remove(typeof(T));
    }
    
    // Clear Managers
    public static void ClearAll()
    {
        _managers.Clear();
    } 
}