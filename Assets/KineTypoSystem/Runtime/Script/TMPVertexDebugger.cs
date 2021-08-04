using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[CustomEditor(typeof(TMPVertexDebugger))] //拡張するクラスを指定
public class CircleAlignmentEditor : Editor
{

    public override void OnInspectorGUI()
    {
        //元のInspector部分を表示
        base.OnInspectorGUI();

        var target = serializedObject.targetObject as TMPVertexDebugger;
        //ボタンを表示
        if (GUILayout.Button("CreateClone"))
        {
            target.CreateClone();
        }
        
    }
}

#endif


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
        uvs = new List<Vector2>();
        // if (Input.GetKeyDown("d"))
        // {
            var mesh = GetComponent<MeshFilter>().sharedMesh;


            vertices = mesh.vertices.ToList();
            index = mesh.GetIndices(0).ToList();
            mesh.GetUVs(0,uvs);
            // }
    }

    public void CreateClone()
    {
        var textMeshPro = GetComponent<TextMeshPro>();
        var text = textMeshPro.text;
            Debug.Log(textMeshPro.text);
            var characterCount = 0;
            // var count = 0;
            var originalMesh = textMeshPro.meshFilter.sharedMesh;



            var uv2s = new List<Vector2>();
            originalMesh.GetUVs(1, uv2s);
            Debug.Log(uv2s.Count);
          
            
            for (int verticesCount = 0; verticesCount < originalMesh.vertexCount; verticesCount+=4)
            {
                if (text.Length <= characterCount) break;
                // Debug.Log(textMeshPro.text[characterCount]);
                var initialVertices = new List<Vector3>();
                var initialIndices = new List<int>{0,1,2,2,3,0};
                var initialUvs00 = new List<Vector2>();
                var initialUvs01 = new List<Vector2>();
                // var initialColor = new List<Color>();
                // var centerPos = Vector3.zero;
            
                var originalUvs00 = new List<Vector2>();
                var originalUvs01 = new List<Vector2>();
                originalMesh.GetUVs(0, originalUvs00);
                originalMesh.GetUVs(1, originalUvs01);
                for (int vCount = 0; vCount < 4; vCount++)
                {
                    var num = characterCount * 4 + vCount;
                    var v = originalMesh.vertices[num];
                    initialVertices.Add(new Vector3(v.x,v.y,v.z));
                    initialUvs00.Add(originalUvs00[num]);
                    initialUvs01.Add(originalUvs01[num]);
                }
                
                // originalMesh.GetColors(initialColor);
                
                var child = new GameObject();
                child.name = text[characterCount].ToString();
                var childMeshRenderer = child.AddComponent<MeshRenderer>();
                var childMesh = new Mesh();
                childMesh.SetVertices(initialVertices);
                
                childMesh.SetTriangles(initialIndices,0);
                childMesh.SetUVs(0,initialUvs00);
                childMesh.SetUVs(1,initialUvs01);
                // childMesh.SetColors(initialColor);
                // childMesh.attrib
                childMesh.RecalculateNormals();
                
                
                var meshFilter = child.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = childMesh;
                // Debug.Log(childMeshRenderer);
                childMeshRenderer.sharedMaterial = textMeshPro.fontSharedMaterial;
                // childMeshRenderer.sharedMaterial.CopyPropertiesFromMaterial(textMeshPro.fontSharedMaterial);
            
                child.transform.SetParent(transform,false);
                characterCount++;
            
            }

    }
#if UNITY_EDITOR
     void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        var count = 0;
        foreach(var v in vertices)
        {
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label( v+transform.position, count.ToString() );
            count++;
            
        }
    }
#endif
    
}
