using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JslibCaller : MonoBehaviour
{
#if  !UNITY_EDITOR && UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void Call(string name, int type, string argument);

        [DllImport("__Internal")]
        private static extern void Call(string name, int type, float argument);
#endif
}
