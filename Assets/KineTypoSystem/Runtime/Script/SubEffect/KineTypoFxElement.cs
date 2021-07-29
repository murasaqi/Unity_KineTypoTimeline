using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    public interface IKineTypoFxElement
    {
        void Init();
        void ResetProps();
        void OnProcessFrame(float progress);
    }

}
