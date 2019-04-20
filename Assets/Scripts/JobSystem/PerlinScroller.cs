using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PerlinScroller : MonoBehaviour
{
    private int _cubeCount;
    public int Width = 500;
    public int Height = 500;
    public int Layers = 3;

    private GameObject[] _cubes;

    void Awake()
    {
        _cubeCount = (int) (Width * Height * Layers);
        _cubes = new GameObject[_cubeCount];
    }

    void Start()
    {
        _cubes = CreateCubes(_cubeCount);
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

    private int xOffset = 0;
    public void Update()
    {
        //int xOffset = (int) (this.transform.position.x - Width / 2.0f);
        xOffset++;
        int zOffset = (int) (this.transform.position.z - Height / 2.0f);

        for (int i = 0; i < _cubeCount; i++)
        {
            int x = i / (Width * Layers);
            int z = (i - x * Height * Layers) / Layers;
            int yOffset = i - x * Width * Layers - z * Layers;

            _cubes[i].transform.position = new Vector3(x, GeneratePerlinHeight(x + xOffset, z + zOffset) + yOffset, z + zOffset);
        }

        if(Input.GetKey("up"))
            transform.Translate(0,0,2);
        else if(Input.GetKey("down"))
            transform.Translate(0,0,-2);
        else if(Input.GetKey("left"))
            transform.Translate(-2,0,0);
        else if(Input.GetKey("right"))
            transform.Translate(2,0,0);
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

