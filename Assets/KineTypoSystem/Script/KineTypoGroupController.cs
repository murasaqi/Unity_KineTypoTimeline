using System.Collections;
using System.Collections.Generic;
using KineTypoSystem;
using UMotionGraphicUtilities;
using UnityEngine;
[ExecuteAlways]
public class KineTypoGroupController : MonoBehaviour
{
    [SerializeField] private AnimationCurve animationCurve =new AnimationCurve();
    [SerializeField] [Range(0,1)]private float progress;
    [SerializeField] private List<AnimationClipTransfer> kineTypoStaggerElements;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var kineTypoStaggerElement in kineTypoStaggerElements)
        {
            kineTypoStaggerElement.ProcessFrame(animationCurve.Evaluate(progress));
        }
    }
}
