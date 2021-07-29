using System.Collections;
using System.Collections.Generic;
using UMotionGraphicUtilities;
using UnityEngine;

namespace KineTypoSystem
{

    [RequireComponent(typeof(AnimationClipTransfer))]
    public class KineTypoDirector : MonoBehaviour
    {

        [SerializeField] private string text;
        public string Text
        {
            get => text;
            set => text = value;
        }
        
        
        // Start is called before the first frame update
        void Start() { }
    
        // Update is called once per frame
        void Update() { }
    }
}   