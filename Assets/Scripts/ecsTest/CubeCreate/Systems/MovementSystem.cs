using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class MovementSystem : JobComponentSystem
{
    [BurstCompile]

    public struct MovementJob : IJobForEach<Translation, Rotation, Speed>
    {
        [ReadOnly] public float deltaTime;

        public void Execute(ref Translation position, ref Rotation rotation, [ReadOnly]ref Speed speed)
        {
            float3 newPos = position.Value + (deltaTime * speed.Value) * math.forward(rotation.Value);

            position.Value = newPos;
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new MovementJob()
        {
            deltaTime = Time.deltaTime
        };

        return job.Schedule(this, inputDeps); ;
    }
}
