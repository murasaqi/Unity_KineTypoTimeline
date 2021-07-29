using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace KineTypoSystem
{ 
    [Serializable]
    public struct KineTypoProfile
    {
        public string word;
        public TMP_FontAsset fontAsset;
        public int fontSize;
        public AnimationClip animationClip;
    }
}