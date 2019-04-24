using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Burst;
using Unity.Collections;

[UpdateAfter(typeof(MovementSystem))]
public class LifeTimeSystem : JobComponentSystem
{
    [BurstCompile]
    public struct LifeTimeJob : IJobForEach<LifeTime>
    {
        [ReadOnly] public float deltaTime; // 渲染最后一帧的时间

        public void Execute(ref LifeTime lifeTime)
        {
            lifeTime.Value = lifeTime.Value - deltaTime;
        }
    }


    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new LifeTimeJob()
        {
            deltaTime = Time.deltaTime
        };

        return job.Schedule(this, inputDeps);
    }
}
