using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
namespace ReplayableExtension
{
    public class ReplayableUnit : MonoBehaviour
    {
        [HideInInspector]
        public DataPair<string, Object> objs = new DataPair<string, Object>();

        [SerializeField]
        string m_id = null;

        [HideInInspector]
        public bool isAdvance = true;

        //public UnityEvent<Component, string, List<string>> CustomCommand;

        bool InitialActive;
        Transform InitialParent;
        Vector3 InitialLocalPositon;
        Vector3 InitialLocalRotation;
        Vector3 InitialLocalScale;
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(m_id))
                    m_id = System.Guid.NewGuid().ToString();
                return m_id;
            }
            set
            {
                m_id = value;
            }
        }
        private void Start()
        {

            if (isAdvance)
            {
                RecordAndReplayBase.Register(this);
                InitialActive = gameObject.activeSelf;
                InitialParent = transform.parent;
                InitialLocalPositon = transform.localPosition;
                InitialLocalRotation = transform.localEulerAngles;
                InitialLocalScale = transform.localScale;

                RecordAndReplayBase.OnStart += Initialize;
            }
        }
        void Initialize()
        {
            gameObject.ReActive(InitialActive);
            transform.parent = InitialParent;
            transform.localPosition = InitialLocalPositon;
            transform.localEulerAngles = InitialLocalRotation;
            transform.localScale = InitialLocalScale;
        }
        private void OnValidate()
        {
            ID = ID;
        }
        public void RefreshReference()
        {
            Component[] com = GetComponentsInChildren<Component>(true);
            List<Object> objects = new List<Object>();
            foreach (var item in com)
            {
                objects.Add(item);
                if (!objects.Contains(item.gameObject))
                    objects.Add(item.gameObject);
            }
            foreach (var item in objects)
            {
                if (!objs.Contains(item))
                {
                    Debug.Log("添加引用：" + item.GetType().ToString());
                    objs.Add(System.Guid.NewGuid().ToString(), item);
                }
            }
            objs.Optimize(objects);
            EditorUtility.SetDirty(this);
        }
    }

    [CustomEditor(typeof(ReplayableUnit))]
    public class ReplayableUnitEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ReplayableUnit instance = (ReplayableUnit)target;
            if (GUILayout.Button("刷新引用"))
            {
                instance.RefreshReference();
            }
        }
    }
}
