using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ReplayableExtension
{
    public class RecordManager : RecordAndReplayBase
    {
        public static RecordManager instance;
        void Awake()
        {
            instance = this;
        }
        public void Create(ReplayableUnit unit, string instanceID)
        {
            if (currentState == State.Recording)
            {
                List<string> command = new List<string>();
                command.Add(instanceID);
                currentData.Datas.Add(new Data
                {
                    time = currentTime,
                    id = unit.ID,
                    type = ReplayableType.RE_INSTANTIATE,
                    command = command
                });
            }
            Register(unit);
        }

        public void RecordCommand(string id, string type, List<string> command)
        {
            Data data = new Data
            {
                time = currentTime,
                id = id,
                type = type,
                command = command
            };

            if (currentState == State.Recording)
                currentData.Datas.Add(data);

            Parse(data.id, data.type, data.command);
        }

        public void StartRecord()
        {
            if (currentState != State.None)
            {
                Debug.LogError("当前状态无法开始录制");
                return;
            }
            base.Begin();
            currentState = State.Recording;
        }
        public void EndRecord(string filePath = null, string fileName = null, int interval = 0)
        {
            if (currentState != State.Recording) return;
            if (currentData == null) return;

            currentData.Datas.Add(new Data
            {
                time = currentTime,
                type = ReplayableType.RE_END
            }
            );

            if (interval > 0)
                currentData = Compress(currentData, interval);

            if (!string.IsNullOrEmpty(filePath) && !string.IsNullOrEmpty(fileName))
            {
                if (!fileName.EndsWith(".replay"))
                    fileName += ".replay";
                XMLHelper.SaveToFile(currentData, Path.Combine(filePath, fileName), true, true);
            }
            Exit();
        }
        ReplayableData Compress(ReplayableData recordData, int Interval)
        {
            List<Data> original = recordData.Datas;
            List<Data> result = new List<Data>();
            int k = 1;
            for (int i = 0; i < original.Count; i++)
            {
                if (
                    i != 0
                    && i != original.Count
                    && ReplayableType.AllowCompress(original[i].type) //该命令类型需要压缩
                    && original[i].id == original[i - k].id  //与前一个命令GUID相同
                    && original[i].id == original[i + 1].id  // 与后一个命令GUID相同
                    && original[i].type == original[i - k].type  //与前一个命令类型相同
                    && original[i].type == original[i + 1].type  //与后一个命令类型相同
                    && original[i].time - original[i - k].time < Interval //与前一个命令时间间隔小于压缩间隔
                    && original[i].id == original[i - k].id//与前一个命令引用相同
                    && original[i].id == original[i + 1].id //与后一个命令引用相同
                )
                {
                    k++;
                    continue; //丢弃
                }
                else
                {
                    k = 1;
                    result.Add(original[i]); //写入
                }
            }
            recordData.Datas = result;
            return recordData;
        }
    }
}
