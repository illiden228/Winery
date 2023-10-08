using Core;
using UnityEngine;
using System.Collections.Generic;

public class FactoryView : BaseMonobehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public T GetPrefabInstanceWithComponent<T>(string name) where T : BaseMonobehaviour
    {
        return new GameObject(name).AddComponent<T>();     
    }
}
