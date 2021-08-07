using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KineTypoSystem
{
    [Serializable]
    public class CloneTextMesh : MonoBehaviour
    {


        [SerializeField] private Rect _rect;
      
        public Rect rect
        {
            get => _rect;
            set => _rect = value;
        }
        // [SerializeField] private float _width;
        // [SerializeField] private float _height;
        // [SerializeField] private Vector2 _center;
        // public float width
        // {
        //     get { return _width; }
        //     set 
        //     {
        //        
        //         _width = value;
        //     }
        // }
        //
        // public float height
        // {
        //     get { return _height; }
        //     set 
        //     {
        //        
        //         _height = value;
        //     }
        // }
        //
        //
        // public Vector2 center
        // {
        //     get { return _center; }
        //     set 
        //     {
        //        
        //         _center = value;
        //     }
        // }
        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update() { }
    }
}