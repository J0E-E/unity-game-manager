using System;
using UnityEngine;

public abstract class Manager: MonoBehaviour, IManager
{
    public virtual void Loaded()
    {
        Debug.Log($"Loaded: {GetType().Name}");
    }
}
