using UnityEngine;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine.Jobs;

public class PerlinScrollerJobs : MonoBehaviour
{
    private int _cubeCount;
    public int Width = 500;
    public int Height = 500;
    public int Layers = 3;

    private GameObject[] _cubes;

    private Transform[] _cubeTransforms;
    private TransformAccessArray _cubeTransformAccessArray;
    private PositionUpdateJob _cubeJob;
    private JobHandle _cubePositionJobHandle;

    void Awake()
    {
        _cubeCount = (int) (Width * Height * Layers);
        _cubes = new GameObject[_cubeCount];
        _cubeTransforms = new Transform[_cubeCount];
    }

    void Start()
    {
        _cubes = CreateCubes(_cubeCount);

        for (int i = 0; i < _cubeCount; i++)
        {
            _cubeTransforms[i] = _cubes[i].transform;
        }

        _cubeTransformAccessArray = new TransformAccessArray(_cubeTransforms);
    }

    public GameObject[] CreateCubes(int count)
    {
        var cubes = new GameObject[count];
        var cubeToCopy = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var renderer = cubeToCopy.GetComponent<MeshRenderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        var collider = cubeToCopy.GetComponent<Collider>();
        collider.enabled = false;

        for (int i = 0; i < count; i++)
        {
            var cube = GameObject.Instantiate(cubeToCopy);
            int x = i / (Width * Layers);
            cube.transform.position = new Vector3(x, 0, (i - x * Height * Layers) / Layers);
            cubes[i] = cube;
        }

        GameObject.Destroy(cubeToCopy);

        return cubes;
    }

    struct  PositionUpdateJob : IJobParallelForTransform
    {
        public int Height;
        public int Width;
        public int Layers;
        public int OffsetX;
        public int OffsetZ;

        public void Execute(int i, TransformAccess transform)
        {
            int x = i / (Width * Layers);
            int z = (i - x * Height * Layers) / Layers;
            int yOffset = i - x * Width * Layers - z * Layers;

            transform.position = new Vector3(x, GeneratePerlinHeight(x + OffsetX, z + OffsetZ) + yOffset, z + OffsetZ);
        }
    }

    private int xOffset = 0;
    public void Update()
    {
        _cubeJob = new PositionUpdateJob();
        {
            _cubeJob.OffsetX = xOffset++;
            _cubeJob.OffsetZ = (int) (transform.position.z - Height / 2.0f);
            _cubeJob.Height = Height;
            _cubeJob.Width = Width;
            _cubeJob.Layers = Layers;
        }

        _cubePositionJobHandle = _cubeJob.Schedule(_cubeTransformAccessArray);
    }

    public void LateUpdate()
    {
        _cubePositionJobHandle.Complete();
    }

    private void OnDestroy()
    {
        _cubeTransformAccessArray.Dispose();
    }

    static float GeneratePerlinHeight(float posX, float posZ)
    {
        float smooth = 0.03f;
        float heightMult = 5;
        float height = (Mathf.PerlinNoise(posX * smooth, posZ * smooth * 2) * heightMult +
                        Mathf.PerlinNoise(posX * smooth, posZ * smooth * 2) * heightMult) / 2.0f;

        return height * 10;
    }
}

