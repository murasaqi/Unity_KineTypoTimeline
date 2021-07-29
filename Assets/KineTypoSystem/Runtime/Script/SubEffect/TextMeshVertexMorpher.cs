using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


// [Serializable]
// public class CharacterMeshVertexInfo
// {
//     public Vector3 pos00;
//     public Vector3 pos01;
//     public Vector3 pos02;
//     public Vector3 pos03;
//     
//     [SerializeField,HideInInspector] private List<Vector3> vertices = new List<Vector3>();
//     [SerializeField,HideInInspector] private List<Vector3> initialVertices = new List<Vector3>();
// }
[ExecuteAlways]
public class TextMeshVertexMorpher : MonoBehaviour
{
    [SerializeField,ReadOnly] private Mesh mesh;
    [SerializeField] private bool isUpdateVertices = false;
    [SerializeField] private Vector3 pos00;
    [SerializeField] private Vector3 pos01;
    [SerializeField] private Vector3 pos02;
    [SerializeField] private Vector3 pos03;
    // private List<Vector3> _offsetVertices = new List<Vector3>();
    [SerializeField,HideInInspector] private List<Vector3> vertices = new List<Vector3>();
    [SerializeField,HideInInspector] private List<Vector3> initialVertices = new List<Vector3>();

    void Start()
    {
        Init();
    }

    public void Init()
    {
        mesh = GetComponent<MeshFilter>().sharedMesh;
        
        
        // var count = 0;
        vertices.Clear();
        initialVertices.Clear();
        foreach (var v in mesh.vertices)
        {
            initialVertices.Add(new Vector3(v.x,v.y,v.z));
            // _offsetVertices.Add(Vector3.zero);
            vertices.Add(Vector3.zero);
        }
    }

    public void Reset()
    {
        if(mesh != null)mesh.SetVertices(initialVertices);
    }

    private void OnDestroy()
    {
        Reset();
    }

    private void OnDisable()
    {
        Reset();
    }
    
    // Update is called once per frame
    void Update()
    {

       
        if (isUpdateVertices)
        {
            
            if (mesh == null || mesh.vertexCount != initialVertices.Count || mesh.vertexCount != vertices.Count)
            {
                Init();
            }
            for (int i = 0; i < vertices.Count; i++)
            {
                var initial = initialVertices[i];
                Vector3 offset = Vector3.zero;

                if (i == 0) offset = pos00;
                if (i == 1) offset = pos01;
                if (i == 2) offset = pos02;
                if (i == 3) offset = pos03;
                vertices[i] = new Vector3(
                    initial.x + offset.x,
                    initial.y + offset.y,
                    initial.z + offset.z
                );
            }
            mesh.SetVertices(vertices);
        }
    }
}
