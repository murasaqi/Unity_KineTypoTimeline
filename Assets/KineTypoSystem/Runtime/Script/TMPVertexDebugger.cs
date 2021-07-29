using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[ExecuteAlways]
public class TMPVertexDebugger : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private List<Vector3> vertices;
    [SerializeField] private List<Vector2> uvs;
    [SerializeField] private List<int> index;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (Input.GetKeyDown("d"))
        // {
            var mesh = GetComponent<MeshFilter>().sharedMesh;

            vertices = mesh.vertices.ToList();
            index = mesh.GetIndices(0).ToList();
            mesh.GetUVs(0,uvs);
            // }
    }
}
