using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace KineTypoSystem
{ 
    [CustomEditor(typeof(KineTypoStaggerElement), true)]
    public class KineTypoElementEditor : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = new VisualElement();
            var serializedTargetObject = serializedObject.targetObject as KineTypoStaggerElement;
            root.Bind(serializedObject);
            
            var visualTree = Resources.Load<VisualTreeAsset>("KineTypoElement");
            visualTree.CloneTree(root);


            root.Query<Button>("ReGenerateButton").First().clickable.clicked += serializedTargetObject.Regenerate;

            // root.Query<ObjectField>("TmpFontAssetField").First().objectType = typeof(TMP_FontAsset);
            var container = new IMGUIContainer(OnInspectorGUI);
            root.Add(container);

            return root;
        }
    }
}