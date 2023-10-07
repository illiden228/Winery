using System;
using UnityEngine;

namespace Tools
{
    public interface IResourceLoader : IDisposable
    {
        bool CheckExistance(string bundleName);
        IDisposable LoadPrefab(string bundleName, string prefabName, Action<GameObject> onComplete);
    }
}