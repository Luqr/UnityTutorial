

using Unity.Entities;
using Unity.Mathematics;

// Entity生命周期
public struct LifeTime : IComponentData
{
    public float Value;
}


// MoveSpeed
public struct Speed : IComponentData
{
    public float Value;
}

public struct PreviousTransloation : IComponentData
{
    public float3 Value;
}

public struct RotationOnlyTag : IComponentData { }