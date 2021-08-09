using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UMotionGraphicUtilities;
using UnityEditor;

namespace KineTypoSystem
{
    public class KineTypoCreator : MonoBehaviour
    {
#if UNITY_EDITOR
        [MenuItem("GameObject/KineTypoSystem/KineTypoElement", false, 0)]
        public static void CreateScreenObject()
        {
            GameObject activeGameObject = Selection.activeGameObject;

            var profile = new KineTypoProfile();
            profile.word = "KineTypoElement";
            profile.animationClip = Resources.Load<AnimationClip>("ExampleTransformAnim");
            profile.fontAsset = Resources.Load<TMP_FontAsset>("LiberationSans");
            profile.fontSize = 12;

            var newObj = CreateKineTypeElement(profile);
            
            if(activeGameObject != null)newObj.transform.SetParent(activeGameObject.transform, false);

        }

#endif
        public static KineTypoStaggerElement CreateKineTypeElement(KineTypoProfile profile)
        {
            
            var parent = new GameObject(profile.word);
            
            var kineTypoElement = parent.GetComponent<KineTypoStaggerElement>();
            if (kineTypoElement == null) kineTypoElement = parent.AddComponent<KineTypoStaggerElement>();
            kineTypoElement.Init(
                profile.word,
                profile.fontAsset,
                profile.animationClip
            );

            return kineTypoElement;
        }

        public static void AdjustCharacterPosition(List<TextMeshPro> textMeshPros, float margin = 0)
        {
            Vector3 position = new Vector3(0,0,0);
            var totalWidth = 0f;
            var height = 0f;
            foreach (var textMesh in textMeshPros)
            {
                // if(textMeshPros.Count > 0 )position += new Vector3(textMeshPros.Last().preferredWidth / 2f, 0f, 0f);
                textMesh.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                if(textMeshPros.Count != 0 ) position += new Vector3(textMesh.preferredWidth / 2f, 0f, 0f);
                textMesh.transform.localPosition = position;
                textMesh.rectTransform.sizeDelta = new Vector2(textMesh.preferredWidth, textMesh.preferredHeight);
                var renderer = textMesh.GetComponent<MeshRenderer>();
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.allowOcclusionWhenDynamic = false;
                totalWidth += textMesh.preferredWidth;
                totalWidth += margin;
                if (height < textMesh.preferredHeight)
                {
                    height = textMesh.preferredHeight;
                }
                if(textMeshPros.Count != 0 ) position += new Vector3(textMesh.preferredWidth / 2f+margin, 0f, 0f);
            }

            var diff = new Vector3(totalWidth/2f, 0f, 0f);
            
            foreach (var t in textMeshPros)
            {
                t.transform.localPosition -= diff;
            }
        }
        // public static List<TextMeshPro> CreateTextMeshList(string word, TMP_FontAsset font, int fontSize, TextAlignmentOptions textAlignmentOptions)
        // {
        //
        //     List<TextMeshPro> texts = new List<TextMeshPro>();
        //
        //     var originalMesh = CreateTextMesh(word, font, fontSize, textAlignmentOptions).meshFilter.sharedMesh;
        //     // var count = 0;
        //     foreach (var ch in word)
        //     {
        //         var textMesh = CreateTextMesh(ch.ToString(),font, fontSize,textAlignmentOptions);
        //         textMesh.name = "text: " + ch;
        //         textMesh.fontSize = fontSize;
        //         textMesh.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        //         var renderer = textMesh.GetComponent<MeshRenderer>();
        //         renderer.shadowCastingMode = ShadowCastingMode.Off;
        //         renderer.allowOcclusionWhenDynamic = false;
        //         texts.Add(textMesh);
        //     }
        //
        //     AdjustCharacterPosition(texts);
        //     return texts;
        // }


        public static List<CloneTextMesh> CreateCloneTextMesh(
            Transform parent,
            string text, 
            TMP_FontAsset fontAsset, 
            int fontSize, 
            TextAlignmentOptions textAlignmentOptions, 
            FontStyles fontStyle = FontStyles.Normal, 
            float characterSpacing = 0f,
            float lineSpacing = 0)
        {


            var textMeshPro = CreateTextMesh(parent,text,fontAsset,fontSize,textAlignmentOptions, fontStyle,characterSpacing,lineSpacing);
            var clones = new List<CloneTextMesh>();
            
            Debug.Log(textMeshPro.text);
            var characterCount = 0;
            // var count = 0;
            var originalMesh = textMeshPro.meshFilter.sharedMesh;
            for (int verticesCount = 0; verticesCount < originalMesh.vertexCount; verticesCount+=4)
            {
                
                if (text.Length <= characterCount) break;
                // Debug.Log(textMeshPro.text[characterCount]);
                var initialVertices = new List<Vector3>();
                var initialIndices = new List<int>{0,1,2,2,3,0};
                var initialUvs00 = new List<Vector2>();
                var initialUvs01 = new List<Vector2>();

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
                
                var child = new GameObject();
                var cloneTextMesh = child.AddComponent<CloneTextMesh>();
                child.name = text[characterCount].ToString();
                var childMeshRenderer = child.AddComponent<MeshRenderer>();
                var childMesh = new Mesh();
                childMesh.SetVertices(initialVertices);
                
                childMesh.SetIndices(initialIndices,originalMesh.GetTopology(0),0,true);
                childMesh.SetUVs(0,initialUvs00);
                childMesh.SetUVs(1,initialUvs01);
                childMesh.RecalculateNormals();

                var meshFilter = child.AddComponent<MeshFilter>();
                meshFilter.sharedMesh = childMesh;
                // Debug.Log(childMeshRenderer);
                childMeshRenderer.sharedMaterial = new Material(textMeshPro.fontSharedMaterial);
                // childMeshRenderer.sharedMaterial.CopyPropertiesFromMaterial(textMeshPro.fontSharedMaterial);


                // cloneTextMesh.width = initialVertices.Last().x-initialVertices.First().x;
                // cloneTextMesh.height = initialVertices[1].y-initialVertices.First().y;

                // Debug.Log( cloneTextMesh.width );
                cloneTextMesh.rect = new Rect(
                    (initialVertices.First().x + initialVertices.Last().x)/2,
                    (initialVertices[1].y + initialVertices.First().y)/2,
                    initialVertices.Last().x-initialVertices.First().x,
                    initialVertices[1].y-initialVertices.First().y
                );
                clones.Add(cloneTextMesh);
                characterCount++;

            }

            DestroyImmediate(textMeshPro.gameObject);
            return clones;

        }
        public static TextMeshPro CreateTextMesh(
            Transform parent,
            string character, 
            TMP_FontAsset font, 
            float fontSize,TextAlignmentOptions textAlignmentOptions, 
            FontStyles fontStyle = FontStyles.Normal, 
            float characterSpacing = 0f,
            float lineSpacing = 0f
            )
        {
            var tmPro = new GameObject().AddComponent<TextMeshPro>();
            tmPro.font = font;
            tmPro.alignment = textAlignmentOptions;
            tmPro.text = character;
            tmPro.fontSize = fontSize;
            tmPro.fontStyle = fontStyle;
            tmPro.enableWordWrapping = false;
            tmPro.name = character;
            tmPro.characterSpacing = characterSpacing;
            tmPro.lineSpacing = lineSpacing;
            tmPro.transform.localEulerAngles = Vector3.zero;
            tmPro.transform.localPosition = Vector3.zero;
            tmPro.transform.localScale = Vector3.one;
            
            tmPro.UpdateFontAsset();
            tmPro.ForceMeshUpdate();
            tmPro.transform.position = parent.position;
            tmPro.transform.localScale = parent.localScale;
            tmPro.transform.eulerAngles = parent.eulerAngles;

            // Debug.Log(tmPro.meshFilter.sharedMesh.vertices.Length);
            return tmPro;
        }

        
        // Update is called once per frame
        void Update()
        {
            
        }
    }

}

