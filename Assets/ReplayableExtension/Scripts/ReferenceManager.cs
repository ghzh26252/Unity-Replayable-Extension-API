using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace ReplayableExtension
{
    public class ReferenceManager : MonoBehaviour
    {
        public static ReferenceManager instance;

        public List<Object> references = new List<Object>();

        [HideInInspector]
        public DataPair<string, Object> objs = new DataPair<string, Object>();


        private void OnValidate()
        {
            RefreshReference();
        }

        private void Start()
        {
            RecordAndReplayBase.advanceObject.AddRange(objs);
        }

        public void RefreshReference()
        {
            foreach (var item in references)
            {
                if (!objs.Contains(item))
                {
                    Debug.Log("添加引用：" + item.GetType().ToString());
                    objs.Add(System.Guid.NewGuid().ToString(), item);
                }
            }
            objs.Optimize(references);
            EditorUtility.SetDirty(this);
        }
    }

    [CustomEditor(typeof(ReferenceManager))]
    public class ReferenceManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ReferenceManager instance = (ReferenceManager)target;
            if (GUILayout.Button("刷新引用"))
            {
                instance.RefreshReference();
            }
        }
    }
}
