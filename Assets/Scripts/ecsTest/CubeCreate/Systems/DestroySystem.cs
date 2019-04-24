using Unity.Entities;
using Unity.Jobs;
using Unity.Collections;
using Unity.Burst;

[UpdateAfter(typeof(LifeTimeSystem))]
public class DestroySystem : JobComponentSystem
{
    EndSimulationEntityCommandBufferSystem entityCommandBuffer;

    [BurstCompile]
    public struct DestoryJob : IJobForEachWithEntity<LifeTime>
    {
        [WriteOnly] public EntityCommandBuffer.Concurrent CommandBuffer;

        public void Execute(Entity entity, int index, ref LifeTime lifeTime)
        {
            if (lifeTime.Value < 0)
            {
                CommandBuffer.DestroyEntity(index, entity);
            }
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new DestoryJob()
        {
            CommandBuffer = entityCommandBuffer.CreateCommandBuffer().ToConcurrent()
        }.Schedule(this, inputDeps);

        // 确保EntityCommandBufferSystem在回放命令缓冲区之前等待任务完成
        entityCommandBuffer.AddJobHandleForProducer(job);
        return job;
    }

    protected override void OnCreateManager()
    {
        // 阻止我们在每一帧创建这个
        entityCommandBuffer = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }
}
