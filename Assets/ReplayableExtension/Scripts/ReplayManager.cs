using System;
using System.Collections;
using UnityEngine;

namespace ReplayableExtension
{
    public class ReplayManager : RecordAndReplayBase
    {
        public static ReplayManager instance;

        Coroutine currentReplay = null;

        Action<float> OnReplayProgress;



        void Awake()
        {
            instance = this;
        }
        //public void Create(string unitID, string command)
        //{
        //    if (currentState != State.Replaying) return;
        //    if (runtimeObject.Contains(unitID)) return;
        //    ReplayableUnit unit = (ReplayableUnit)advanceObject.Get(command);
        //    if (unit == null)
        //    {
        //        Debug.LogError("未找到预制体");
        //        return;
        //    }
        //    ReplayableUnit m_unit = ReplayableAPI.ReInstantiate(unit, false);
        //    m_unit.ID = unitID;
        //    Register(m_unit);
        //}


        public void StartReplay(string filePath, Action<float> onReplayProgress = null)
        {
            if (currentState != State.None)
            {
                Debug.LogError("当前状态无法开始回放");
                return;
            }

            base.Begin();
            currentState = State.Replaying;
            OnReplayProgress = onReplayProgress;
            currentData = XMLHelper.LoadFromFile<ReplayableData>(filePath, true, true);
            currentReplay = StartCoroutine(Play());
        }
        IEnumerator Play()
        {
            foreach (Data data in currentData.Datas)
            {
                while (currentTime < data.time)
                {
                    yield return new WaitForFixedUpdate();
                    OnReplayProgress?.Invoke(currentTime);
                }
                switch (data.type)
                {
                    default:
                        Parse(data.id, data.type, data.command);
                        break;
                    case ReplayableType.RE_CUSTOMEVENT:
                        CustomEvent.Invoke();
                        break;
                    case ReplayableType.RE_END:
                        break;
                }
            }
            BreakReplay();
        }
        void BreakReplay()
        {
            if (currentState == State.None) return;
            //foreach (var item in recordReplayableUnit.Values)
            //{
            //    Destroy(item.gameObject);
            //}
            StopCoroutine(currentReplay);
            OnReplayProgress?.Invoke(-1);
            Exit();
        }
        public float ReplaySpeed
        {
            set
            {
                if (Application.isEditor)
                    value = Mathf.Clamp(value, 0, 100);
                Time.timeScale = value;
            }
            get
            {
                return Time.timeScale;
            }
        }
        bool jumping = false;
        public enum JumpEndState
        {
            Keep,
            Play,
            Pause
        }

        public void Jump(float time, bool isProgress = true, JumpEndState endState = JumpEndState.Keep)
        {
            if (currentState != State.Replaying) return;
            if (currentData == null) return;
            if (jumping) return;
            if (isProgress)
                time = Mathf.Lerp(0, currentData.Datas[currentData.Datas.Count - 1].time, time);
            if (currentTime > time)
                ReStart();
            StartCoroutine(Boost(time, endState));
        }
        IEnumerator Boost(float time, JumpEndState endState)
        {
            jumping = true;

            float originalSpeed = ReplaySpeed;
            while (time > currentTime)
            {
                ReplaySpeed = Math.Min(Math.Max(1, (time - currentTime) / 1000 / Time.fixedDeltaTime / 20), 50);
                yield return new WaitForFixedUpdate();
            }
            switch (endState)
            {
                case JumpEndState.Keep:
                    ReplaySpeed = originalSpeed;
                    break;
                case JumpEndState.Play:
                    ReplaySpeed = 1;
                    break;
                case JumpEndState.Pause:
                    ReplaySpeed = 0;
                    break;
            }
            jumping = false;
        }
        public void ReStart()
        {
            BreakReplay();
            currentReplay = StartCoroutine(Play());
        }
    }
}
