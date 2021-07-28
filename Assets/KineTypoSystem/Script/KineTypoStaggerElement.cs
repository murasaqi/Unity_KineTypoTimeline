using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UMotionGraphicUtilities;
using UnityEngine;

namespace KineTypoSystem
{
    public class KineTypoStaggerElement : MonoBehaviour
    {

        [HideInInspector] [SerializeField] private string text;
        [HideInInspector] [SerializeField] private int fontSize = 12;
        [HideInInspector] [SerializeField] private TMP_FontAsset tmpFontAsset;
        [SerializeField] private FontStyles fontStyle;
        [HideInInspector] [SerializeField] private AnimationClip animationClip;
        [HideInInspector] [SerializeField] private TextMaterialType textMaterialType = TextMaterialType.SDFTexture;
        [HideInInspector] [SerializeField] private AnimationClipTransfer animationClipTransfer;
        [SerializeField] private List<TextMeshPro> textMeshPros;
        [SerializeField] private List<GameObject> cloneTextMesh;
        
        // Start is called before the first frame update
        void Start() { }
        private void InitAnimationComponents(AnimationClip animationClip)
        {
            var transfer = GetComponent<AnimationClipTransfer>();
            if (transfer == null)
            {
                animationClipTransfer = gameObject.AddComponent<AnimationClipTransfer>();
            }

            this.animationClip = animationClip;

            animationClipTransfer.TargetObject = gameObject;
            animationClipTransfer.AnimationClip = this.animationClip;
            
        }
        public void Init( string text, TMP_FontAsset tmpFontAsset, AnimationClip animationClip)
        {
            cloneTextMesh = new List<GameObject>();
            textMeshPros = new List<TextMeshPro>();
            this.tmpFontAsset = tmpFontAsset;
            this.text = text;
            
            if (textMaterialType == TextMaterialType.SDFTexture)
            { 
                cloneTextMesh = KineTypoCreator.CreateCloneTextMesh(text, tmpFontAsset, fontSize, fontStyle);

                foreach (var clone in cloneTextMesh)
                {
                    clone.transform.SetParent(transform,false);
                }

            }
            if (textMaterialType == TextMaterialType.TextMeshProOriginal)
            {
                foreach (var textMeshPro in textMeshPros)
                {
                    Debug.Log(textMeshPro);
                    textMeshPro.transform.SetParent(transform,false);
                }
            }
            
            InitAnimationComponents(animationClip);
            
            animationClipTransfer.Init();
        }

        public void UpdateFontAsset()
        {
            foreach (var textMeshPro in textMeshPros)
            {
                textMeshPro.font = tmpFontAsset;
            }
        }

        public void ApplySettings()
        {
            animationClipTransfer.AnimationClip = animationClip;
            UpdateFontAsset();
            KineTypoCreator.AdjustCharacterPosition(textMeshPros);
        }

        public void Regenerate()
        {
            if(text.Length == 0 || animationClip == null || tmpFontAsset == null) return;
            DestroyImmediate(animationClipTransfer);
            foreach (var t in textMeshPros)
            {
                DestroyImmediate(t.gameObject);
            }
            
            foreach (var t in cloneTextMesh)
            {
                DestroyImmediate(t);
            }
            textMeshPros.Clear();
            cloneTextMesh.Clear();
            gameObject.name = text;
            var profile = new KineTypoProfile();
            profile.word = text;
            profile.animationClip = animationClip;
            profile.fontAsset = tmpFontAsset;
            profile.fontSize = 12;
            
            
            Init(text, tmpFontAsset,animationClip);


        }
        // Update is called once per frame
        void Update() { }
    }
}