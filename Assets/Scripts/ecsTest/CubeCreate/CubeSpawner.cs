using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Unity.Rendering;
using Random = Unity.Mathematics.Random;
using UnityEngine.UI;


public class CubeSpawner : MonoBehaviour
{
    #region ECS 变量

    private EntityManager entityManager;
    private EntityArchetype cubeArchetype;

    #endregion

    #region Cube 创建时参数

    // 几种创建方式
    [SerializeField] public SpawnType spawnType = SpawnType.None;
    
    // 间隔多少秒创建Cube
    [SerializeField] public float cubeSpawnRate = 3;

    // 决定多少cubes要创建
    [SerializeField] public int NumCubesToSpawnAtOnce = 1;

    // 决定cubes是否旋转
    [SerializeField] public bool bRotationCube = false;

    // 生成mesh
    [SerializeField] private Mesh inMesh;

    // 第一种给mesh使用的material颜色 - 需要实例化
    [SerializeField]
    Material inMaterial_Primary;

    // 同上 第二种
    [SerializeField]
    Material inMaterial_Secondary;

    private float timeSinceLastSpawn = 0;

    #endregion

    #region 随机数相关

    private bool bPrimaryColor = true;
    private Random r;
    private Quaternion rQuaternion;
    private float3 rPos, rScale;
    private float rLife, rSpeed;

    #endregion

    void Start()
    {
        entityManager = World.Active.EntityManager;

        // 创建entity archetype
        cubeArchetype = entityManager.CreateArchetype(ComponentType.ReadWrite<Translation>(),
            ComponentType.ReadWrite<Rotation>(), ComponentType.ReadWrite<NonUniformScale>(),
            ComponentType.ReadWrite<LifeTime>(), ComponentType.ReadWrite<Speed>(),
            ComponentType.ReadOnly<LocalToWorld>());
    }

    void Update()
    {
        if (inMesh == null || inMaterial_Primary == null || inMaterial_Secondary == null)
        {
            Debug.LogError("Mesh or materials are null - needs to be fixed");
            return;
        }

        // Determine spawn settings based on inspector values
        switch (spawnType)
        {
            case SpawnType.None:
                return;
            case SpawnType.Input:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    for (int i = 0; i < NumCubesToSpawnAtOnce; i++)
                    {
                        SpawnCube();
                    }
                }
                break;
            case SpawnType.Timer:
                timeSinceLastSpawn -= Time.deltaTime;
                if (timeSinceLastSpawn <= 0)
                {
                    for (int i = 0; i < NumCubesToSpawnAtOnce; i++)
                    {
                        SpawnCube();
                    }

                    // Set the 'timer' back to the start
                    timeSinceLastSpawn = cubeSpawnRate;
                }
                break;
        }

    }

    void SpawnCube()
    {
        #region 随机数值设定

        r = new Random((uint)UnityEngine.Random.Range(0, 1000000));

        rPos = r.NextFloat3();
        rScale = r.NextFloat3(0.5f, 2.0f);
        rLife = r.NextFloat(2, 10);
        rSpeed = r.NextFloat(1, 20);
        rQuaternion = r.NextQuaternionRotation();

        #endregion

        Entity cubeEntity = entityManager.CreateEntity(cubeArchetype);

        entityManager.SetComponentData(cubeEntity, new Translation { Value = rPos });
        entityManager.SetComponentData(cubeEntity, new Rotation{ Value = rQuaternion });
        entityManager.SetComponentData(cubeEntity, new NonUniformScale { Value = rScale });
        entityManager.SetComponentData(cubeEntity, new LifeTime { Value = rLife });
        entityManager.SetComponentData(cubeEntity, new Speed { Value = rSpeed });


        if (bPrimaryColor)
        {
            entityManager.AddSharedComponentData(cubeEntity, new RenderMesh{ mesh=inMesh, material = inMaterial_Primary });
        }
        else
        {
            entityManager.AddSharedComponentData(cubeEntity, new RenderMesh { mesh = inMesh, material = inMaterial_Secondary });
        }
        bPrimaryColor = !bPrimaryColor;

        if(bRotationCube)
            entityManager.AddComponentData(cubeEntity, new RotationOnlyTag());
    }
}


public enum SpawnType
{
    None,
    Input,
    Timer
}