using UnityEngine;

public interface IGrowable
{
    void UpdateGrowth();

    bool Grown { get; }
}
