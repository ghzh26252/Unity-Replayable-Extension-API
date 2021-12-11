using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ReplayableExtension
{
    public class CreateManager : MonoBehaviour
    {
        public static CreateManager instance;

        public List<ReplayableUnit> replayablePrefabs = new List<ReplayableUnit>();

        [HideInInspector]
        public DataPair<ReplayableUnit, string> replayableUnitID = new DataPair<ReplayableUnit, string>();

        private void Awake()
        {
            foreach (var item in replayablePrefabs)
            {
                replayableUnitID.Add(item, item.GetInstanceID().ToString());
            }
            instance = this;
        }
    }
}
