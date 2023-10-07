using UnityEngine;

public interface IGrowable
{
    void UpdateGrowth(float deltaTime);

    bool Grown { get; }
}
