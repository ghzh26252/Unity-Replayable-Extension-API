using System.Collections.Generic;
using UnityEngine;

namespace ReplayableExtension
{
    public abstract class RecordAndReplayBase : MonoBehaviour
    {
        public static System.Action OnStart;

        public static System.Action CustomEvent;
        public class ReplayableData
        {
            public List<Data> Datas = new List<Data>();
        }
        public struct Data
        {
            public int time;
            public string id;
            public string type;
            public List<string> command;
        }
        protected static ReplayableData currentData = new ReplayableData();

        public enum State
        {
            None,
            Recording,
            Replaying,
        }
        public static State currentState = State.None;

        public static DataPair<string, UnityEngine.Object> advanceObject = new DataPair<string, UnityEngine.Object>();
        public static DataPair<string, UnityEngine.Object> runtimeObject = new DataPair<string, UnityEngine.Object>();

        //public static DataPair<string, object> fields = new DataPair<string, object>();
        //public static void GetReplayableAttributes()
        //{
        //    Type type = typeof(MonoBehaviour);
        //    foreach (var item in type.GetFields())
        //    {
        //        foreach (var i in item.GetCustomAttributes(true))
        //        {
        //            if (!(i is ReplayableAttribute)) return;
        //            ReplayableAttribute replayableAttribute = i as ReplayableAttribute;
        //            fields.Add(replayableAttribute.id,item.GetValue(i));
        //        }
        //    }
        //}

        protected int startTime = -1;
        protected int currentTime => Mathf.Max((int)(Time.time * 1000) - startTime, 0);

        public static void Register(ReplayableUnit unit)
        {
            Debug.Log("注册物体：" + unit.name + "/" + unit.ID);
            foreach (var item in unit.objs)
            {
                if (unit.isAdvance)
                    advanceObject.Add(unit.ID + item.Key, item.Value);
                else
                    runtimeObject.Add(unit.ID + item.Key, item.Value);
            }
        }
        public void Begin()
        {
            OnStart.Invoke();
            foreach (var item in runtimeObject.List2)
            {
                if (!(item is Transform))
                    Destroy(item);
            }
            runtimeObject.Clear();
            currentData = new ReplayableData();
            startTime = (int)(Time.time * 1000);
        }

        protected void Exit()
        {
            currentState = State.None;
            currentData = null;
            startTime = -1;
        }

        public static Object GetObject(string id)
        {
            return advanceObject.Get(id) ?? runtimeObject.Get(id);
        }
        public static string GetID(Object obj)
        {
            return advanceObject.Get(obj) ?? runtimeObject.Get(obj);
        }

        public static Object Parse(string id, string type, List<string> command)
        {
            Object obj = GetObject(id);
            if (obj == null)
            {
                Debug.LogError("未找到组件：" + id);
                return null;
            }
            //unit.CustomCommand.Invoke(component, type, command);
            switch (type)
            {
                case ReplayableType.RE_INSTANTIATE:
                    GameObject ins = Instantiate((GameObject)obj);
                    if (ins.TryGetComponent(out ReplayableUnit unit))
                    {
                        unit.ID = XMLHelper.XMLToObject<string>(command[0]);
                        unit.isAdvance = false;
                        Register(unit);
                    }
                    else
                        runtimeObject.Add(XMLHelper.XMLToObject<string>(command[0]), ins);
                    return ins;
                case ReplayableType.RE_DESTORY:
                    Debug.Log("销毁物体：" + obj.name + "/" + id);
                    GameObject.Destroy(obj);
                    break;
                case ReplayableType.RE_ACTIVE:
                    ((GameObject)obj).SetActive(XMLHelper.XMLToObject<bool>(command[0]));
                    break;
                case ReplayableType.RE_PARENT:
                    Component transformParent;
                    if (command.Count == 0)
                        transformParent = null;
                    else
                        transformParent = (Transform)GetObject(XMLHelper.XMLToObject<string>(command[0]));
                    ((Transform)obj).parent = (Transform)transformParent;
                    break;
                case ReplayableType.RE_POSITION:
                    ((Transform)obj).position = XMLHelper.XMLToObject<Vector3>(command[0]);
                    break;
                case ReplayableType.RE_LOCAL_POSITION:
                    ((Transform)obj).localPosition = XMLHelper.XMLToObject<Vector3>(command[0]);
                    break;
                case ReplayableType.RE_EULER_ANGLES:
                    ((Transform)obj).eulerAngles = XMLHelper.XMLToObject<Vector3>(command[0]);
                    break;
                case ReplayableType.RE_LOCAL_EULER_ANGLES:
                    ((Transform)obj).localEulerAngles = XMLHelper.XMLToObject<Vector3>(command[0]);
                    break;
                case ReplayableType.RE_SCALE:
                    ((Transform)obj).localScale = XMLHelper.XMLToObject<Vector3>(command[0]);
                    break;
                case ReplayableType.RE_MATERIAL_FLOAT:
                    ((Renderer)obj).materials[XMLHelper.XMLToObject<int>(command[2])].SetFloat(XMLHelper.XMLToObject<string>(command[0]), XMLHelper.XMLToObject<float>(command[1]));
                    break;
                case ReplayableType.RE_MATERIAL_COLOR:
                    ((Renderer)obj).materials[XMLHelper.XMLToObject<int>(command[2])].SetColor(XMLHelper.XMLToObject<string>(command[0]), XMLHelper.XMLToObject<Color>(command[1]));
                    break;
                case ReplayableType.RE_MATERIAL_VECTOR:
                    ((Renderer)obj).materials[XMLHelper.XMLToObject<int>(command[2])].SetVector(XMLHelper.XMLToObject<string>(command[0]), XMLHelper.XMLToObject<Vector4>(command[1]));
                    break;
                case ReplayableType.RE_MATERIAL_TEXTURE:
                    Texture tex;
                    if (command.Count == 0)
                        tex = null;
                    else
                        tex = (Texture)GetObject(XMLHelper.XMLToObject<string>(command[1]));
                    ((Renderer)obj).materials[XMLHelper.XMLToObject<int>(command[2])].SetTexture(XMLHelper.XMLToObject<string>(command[0]), tex);
                    break;
                case ReplayableType.RE_ANIMATOR_INTEGER:
                    ((Animator)obj).SetInteger(XMLHelper.XMLToObject<string>(command[0]), XMLHelper.XMLToObject<int>(command[1]));
                    break;
                case ReplayableType.RE_ANIMATOR_FLOAT:
                    ((Animator)obj).SetFloat(XMLHelper.XMLToObject<string>(command[0]), XMLHelper.XMLToObject<float>(command[1]));
                    break;
                case ReplayableType.RE_ANIMATOR_BOOL:
                    ((Animator)obj).SetBool(XMLHelper.XMLToObject<string>(command[0]), XMLHelper.XMLToObject<bool>(command[1]));
                    break;
                case ReplayableType.RE_ANIMATOR_TRIGGER:
                    ((Animator)obj).SetTrigger(XMLHelper.XMLToObject<string>(command[0]));
                    break;
                case ReplayableType.RE_ANIMATOR_RESET_TRIGGER:
                    ((Animator)obj).ResetTrigger(XMLHelper.XMLToObject<string>(command[0]));
                    break;
                case ReplayableType.RE_ANIMATOR_PLAY:
                    ((Animator)obj).Play(XMLHelper.XMLToObject<string>(command[0]), XMLHelper.XMLToObject<int>(command[1]), XMLHelper.XMLToObject<float>(command[2]));
                    break;
                case ReplayableType.RE_ANIMATOR_SPEED:
                    ((Animator)obj).speed = XMLHelper.XMLToObject<float>(command[0]);
                    break;
                case ReplayableType.RE_ANIMATOR_CROSS_FADE:
                    ((Animator)obj).CrossFade(XMLHelper.XMLToObject<string>(command[0]), XMLHelper.XMLToObject<float>(command[1]), XMLHelper.XMLToObject<int>(command[2]), XMLHelper.XMLToObject<float>(command[3]), XMLHelper.XMLToObject<float>(command[4]));
                    break;
                case ReplayableType.RE_ANIMATION_PLAY:
                    if (string.IsNullOrEmpty(XMLHelper.XMLToObject<string>(command[0])))
                        ((Animation)obj).Play(XMLHelper.XMLToObject<PlayMode>(command[1]));
                    else
                        ((Animation)obj).Play(XMLHelper.XMLToObject<string>(command[0]), XMLHelper.XMLToObject<PlayMode>(command[1]));
                    break;
                case ReplayableType.RE_ANIMATION_STOP:
                    if (string.IsNullOrEmpty(XMLHelper.XMLToObject<string>(command[0])))
                        ((Animation)obj).Stop();
                    else
                        ((Animation)obj).Stop(XMLHelper.XMLToObject<string>(command[0]));
                    break;
                case ReplayableType.RE_ANIMATION_REWIND:
                    if (string.IsNullOrEmpty(XMLHelper.XMLToObject<string>(command[0])))
                        ((Animation)obj).Rewind();
                    else
                        ((Animation)obj).Rewind(XMLHelper.XMLToObject<string>(command[0]));
                    break;
            }
            return null;
        }
    }
}
