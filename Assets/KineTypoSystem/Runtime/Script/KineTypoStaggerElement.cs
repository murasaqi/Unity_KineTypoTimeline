using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UMotionGraphicUtilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace KineTypoSystem
{
    [RequireComponent(typeof(AnimationClipTransfer))]
    public class KineTypoStaggerElement : MonoBehaviour
    {

        [HideInInspector] [SerializeField] [TextArea(1,3)]private string text;
        [HideInInspector] [SerializeField] private int fontSize = 12;
        [HideInInspector] [SerializeField] private TMP_FontAsset tmpFontAsset;
        [SerializeField] private FontStyles fontStyle;
        [SerializeField] private float characterSpacing = 0;
        [SerializeField] private float lineSpacing = 0;
        [HideInInspector] [SerializeField] private AnimationClip animationClip;
        [HideInInspector] [SerializeField] private TextMaterialType textMaterialType = TextMaterialType.SDFTexture;
        [HideInInspector] [SerializeField] private AnimationClipTransfer animationClipTransfer;
        [SerializeField] private List<TextMeshPro> textMeshPros;
        [SerializeField] private List<CloneTextMesh> cloneTextMesh;
        [SerializeField] private TextAlignmentOptions textAlignmentOptions;
        [SerializeField] private Rect _rect = new Rect();
        private Animation _animation;

        
        // public AnimationClipTransfer 

        public void ProcessFrame(float progress)
        {
            animationClipTransfer.ProcessFrame(progress);
        }
        public Rect rect => _rect;
        public string Text
        {
            get => text;
            set => text = value;
        }

        public void SetAnimationClip(AnimationClip animationClip)
        {
            this.animationClip = animationClip;
            animationClipTransfer.AnimationClip = animationClip;
        }
        

        // Start is called before the first frame update
        void Start() { }
        private void InitAnimationComponents(AnimationClip animationClip)
        {
            // var transfer = GetComponent<AnimationClipTransfer>();
            if (animationClipTransfer == null)
            {
                animationClipTransfer = gameObject.AddComponent<AnimationClipTransfer>();
            }

            this.animationClip = animationClip;

            animationClipTransfer.TargetObject = gameObject;
            animationClipTransfer.AnimationClip = this.animationClip;
            
        }
        public void Init( string text, TMP_FontAsset tmpFontAsset, AnimationClip animationClip)
        {
            cloneTextMesh = new List<CloneTextMesh>();
            textMeshPros = new List<TextMeshPro>();
            this.tmpFontAsset = tmpFontAsset;
            this.text = text;
            
            if (textMaterialType == TextMaterialType.SDFTexture)
            { 
                cloneTextMesh = KineTypoCreator.CreateCloneTextMesh(transform,text, tmpFontAsset, fontSize, textAlignmentOptions, fontStyle, characterSpacing,lineSpacing);
                var startX = cloneTextMesh.First().rect.position.x - cloneTextMesh.First().rect.width / 2f;
                var startY = cloneTextMesh.First().rect.position.y + cloneTextMesh.First().rect.height / 2f;
                var lastX = 0f;
                var lastY = 0f;
                foreach (var clone in cloneTextMesh)
                {
                    clone.transform.SetParent(transform,false);
                    clone.gameObject.layer = gameObject.layer;

                    var lastVertexX = clone.rect.position.x + clone.rect.width / 2f;
                    var lastVertexY = clone.rect.position.y - clone.rect.height / 2f;
                    if (lastX < lastVertexX)
                    {
                        lastX = lastVertexX;
                    }

                    if (lastY > lastVertexY)
                    {
                        lastY = lastVertexY;
                    }

                }
                
                _rect.Set(
                    (startX + lastX)/2f,
                    (startY + lastY)/2f,
                    lastX - startX,
                    lastY - startY
                );

            }
            if (textMaterialType == TextMaterialType.TextMeshProOriginal)
            {
                foreach (var textMeshPro in textMeshPros)
                {
                    Debug.Log(textMeshPro);
                    textMeshPro.transform.SetParent(transform,false);
                    textMeshPro.gameObject.layer = gameObject.layer;
                }
            }

            foreach (var clone in cloneTextMesh)
            {
                clone.transform.SetParent(transform,false);
                var textVertexMorpher = clone.gameObject.AddComponent<TextMeshVertexMorpher>();
                textVertexMorpher.Init();
                // animationClipTransfer.OnInitHandler += textVertexMorpher.Init;
                animationClipTransfer.OnResetChildTransformHandler += textVertexMorpher.Reset;

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

        public void GenerateText(string newText)
        {
            this.text = newText;
            Regenerate();
        }

        public void Play(string newText, AnimationClip animationClip)
        {
            SetAnimationClip(animationClip);
            GenerateText(newText);
            Play();
        }
        
        public void Play(string newText)
        {
            SetAnimationClip(animationClip);
            GenerateText(newText);
            Play();
        }
        
        public void Play(AnimationClip animationClip)
        {
            SetAnimationClip(animationClip);
            Play();
        }
        
        public void Play()
        {
            if(_animation == null)_animation = GetComponent<Animation>();
            if (_animation != null)
            {
                _animation.Stop();
                _animation.Play();
            }
        }

        public void Regenerate()
        {
            if(text.Length == 0 || animationClip == null || tmpFontAsset == null) return;
            // DestroyImmediate(animationClipTransfer);
            foreach (var t in textMeshPros)
            {
                if(t != null)DestroyImmediate(t.gameObject);
            }
            
            foreach (var t in cloneTextMesh)
            {
                if(t != null)DestroyImmediate(t.gameObject);
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
        
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            // Draw a yellow sphere at the transform's position
            // var count = 0;
            // var drawRect = new Rect();
            // // var pos = transform.TransformPoint(new Vector3(
            // //     cloneTextMesh.First().rect.position.x,
            // // cloneTextMesh.First().rect.position.y,
            // //     transform.localPosition.z
            // //     ));
            // drawRect.position = new Vector2(
            //     cloneTextMesh.First().rect.position.x,
            //     cloneTextMesh.First().rect.position.y
            //     );
            //
            // drawRect.width = _rect.width * transform.lossyScale.x;
            // drawRect.height = _rect.height * transform.lossyScale.y;
            //
            // UnityEditor.Handles.DrawSolidRectangleWithOutline(drawRect,Color.blue, Color.white);
            // foreach(var v in _rect)
            // {
            //     UnityEditor.Handles.color = Color.white;
            //     UnityEditor.Handles.Label( v+transform.position, count.ToString() );
            //     count++;
            //
            // }
        }
#endif
    }
}