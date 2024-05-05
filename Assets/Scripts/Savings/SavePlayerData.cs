using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveData
{
    [System.Serializable]
    public class SavePlayerData
    {
        public int coinsAmount;

        public SavePlayerData()
        {
            coinsAmount = 45;
        }
    }
}