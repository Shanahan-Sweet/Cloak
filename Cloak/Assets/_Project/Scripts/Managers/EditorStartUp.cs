using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
[ExecuteInEditMode]
class EditorStartUp : MonoBehaviour
{

    [SerializeField] ShaderManager shaderManager;
    void Awake()
    {
        Debug.Log("Editor causes this Awake");
        shaderManager.SetColours();
    }

}
#endif