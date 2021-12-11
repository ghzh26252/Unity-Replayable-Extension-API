using System.Collections.Generic;
using UnityEngine;

namespace ReplayableExtension
{
    public static class ReplayableAPI
    {
        /// <summary>
        /// Instantiate
        /// </summary>
        public static ReplayableUnit ReInstantiate(ReplayableUnit unit, bool record = true)
        {
            string id = CreateManager.instance.replayableUnitID.Get(unit);
            if (string.IsNullOrEmpty(id)) return null;
            ReplayableUnit m_unit = GameObject.Instantiate(unit);
            m_unit.ID = null;
            m_unit.isAdvance = false;
            if (record) RecordManager.instance.Create(m_unit, id);
            return m_unit;
        }
        /// <summary>
        /// Destory
        /// </summary>
        public static void ReDestroy(Object obj)
        {
            string id = RecordAndReplayBase.GetID(obj);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_DESTORY, commands);
        }
        /// <summary>
        /// GameObject
        /// </summary>
        public static void ReActive(this GameObject gameObject, bool value)
        {
            string id = RecordAndReplayBase.GetID(gameObject.transform);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(value));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ACTIVE, commands);
        }
        /// <summary>
        /// Transform
        /// </summary>
        public static void ReParent(this Transform component, Transform parent)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();

            string idParent = RecordAndReplayBase.GetID(parent);
            if (idParent != null)
            {
                commands.Add(XMLHelper.ObjectToXML(idParent));
            }
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_PARENT, commands);
        }

        public static void RePosition(this Transform component, Vector3 value)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(value));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_POSITION, commands);
        }

        public static void ReLoaclPosition(this Transform component, Vector3 value)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(value));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_LOCAL_POSITION, commands);
        }

        public static void ReEulerAngles(this Transform component, Vector3 value)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(value));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_EULER_ANGLES, commands);
        }

        public static void ReLoaclEulerAngles(this Transform component, Vector3 value)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(value));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_LOCAL_EULER_ANGLES, commands);
        }

        public static void ReScale(this Transform component, Vector3 value)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(value));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_SCALE, commands);
        }
        /// <summary>
        /// Renderer
        /// </summary>
        public static void ReFloat(this Renderer component, string name, float value, int index = 0)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(name));
            commands.Add(XMLHelper.ObjectToXML(value));
            commands.Add(XMLHelper.ObjectToXML(index));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_MATERIAL_FLOAT, commands);
        }
        public static void ReColor(this Renderer component, string name, Color value, int index = 0)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(name));
            commands.Add(XMLHelper.ObjectToXML(value));
            commands.Add(XMLHelper.ObjectToXML(index));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_MATERIAL_COLOR, commands);
        }
        public static void ReVector(this Renderer component, string name, Vector4 value, int index = 0)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(name));
            commands.Add(XMLHelper.ObjectToXML(value));
            commands.Add(XMLHelper.ObjectToXML(index));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_MATERIAL_VECTOR, commands);
        }
        /// <summary>
        /// Animator
        /// </summary>
        public static void ReInteger(this Animator component, int id, int value)
        {
            component.ReInteger(component.GetParameter(id).name, value);
        }
        public static void ReInteger(this Animator component, string name, int value)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(name));
            commands.Add(XMLHelper.ObjectToXML(value));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ANIMATOR_INTEGER, commands);
        }

        public static void ReFloat(this Animator animator, int id, float value)
        {
            animator.ReFloat(animator.GetParameter(id).name, value);
        }
        public static void ReFloat(this Animator component, string name, float value)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(name));
            commands.Add(XMLHelper.ObjectToXML(value));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ANIMATOR_FLOAT, commands);
        }

        public static void ReBool(this Animator component, int id, bool value)
        {
            component.ReBool(component.GetParameter(id).name, value);
        }
        public static void ReBool(this Animator component, string name, bool value)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(name));
            commands.Add(XMLHelper.ObjectToXML(value));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ANIMATOR_BOOL, commands);
        }

        public static void ReTrigger(this Animator animator, int id)
        {
            animator.ReTrigger(animator.GetParameter(id).name);
        }
        public static void ReTrigger(this Animator component, string name)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(name));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ANIMATOR_TRIGGER, commands);
        }

        public static void ReResetTrigger(this Animator component, int id)
        {
            component.ReResetTrigger(component.GetParameter(id).name);
        }
        public static void ReResetTrigger(this Animator component, string name)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(name));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ANIMATOR_RESET_TRIGGER, commands);
        }

        public static void RePlay(this Animator component, string stateName, int layer = -1, float normalizedTime = float.NegativeInfinity)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(stateName));
            commands.Add(XMLHelper.ObjectToXML(layer));
            commands.Add(XMLHelper.ObjectToXML(normalizedTime));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ANIMATOR_PLAY, commands);
        }
        public static void ReSpeed(this Animator component, float value)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(value));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ANIMATOR_SPEED, commands);
        }

        public static void ReAnimatorCrossFade(this Animator component, string stateName, float normalizedTransitionDuration, int layer = -1, float normalizedTimeOffset = float.NegativeInfinity, float normalizedTransitionTime = 0f)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(stateName));
            commands.Add(XMLHelper.ObjectToXML(normalizedTransitionDuration));
            commands.Add(XMLHelper.ObjectToXML(layer));
            commands.Add(XMLHelper.ObjectToXML(normalizedTimeOffset));
            commands.Add(XMLHelper.ObjectToXML(normalizedTransitionTime));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ANIMATOR_SPEED, commands);
        }
        /// <summary>
        /// Animation
        /// </summary>
        public static void RePlay(this Animation component, string animation = null, PlayMode mode = PlayMode.StopSameLayer)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(animation));
            commands.Add(XMLHelper.ObjectToXML(mode));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ANIMATION_PLAY, commands);
        }
        public static void ReStop(this Animation component, string animation = null)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(animation));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ANIMATION_STOP, commands);
        }
        public static void ReRewind(this Animation component, string animation = null)
        {
            string id = RecordAndReplayBase.GetID(component);
            if (string.IsNullOrEmpty(id)) return;

            List<string> commands = new List<string>();
            commands.Add(XMLHelper.ObjectToXML(animation));
            RecordManager.instance.RecordCommand(id, ReplayableType.RE_ANIMATION_REWIND, commands);
        }
        /// <summary>
        /// CustomEvent
        /// </summary>
        public static void ReCustomEvent()
        {
            List<string> commands = new List<string>();
            RecordManager.instance.RecordCommand(null, ReplayableType.RE_CUSTOMEVENT, commands);
        }
    }
}
