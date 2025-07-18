﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SpartaDungeon.Controller;
using SpartaDungeon.Managers;

namespace SpartaDungeon.Data
{
    // 아이템 정보(아이디, 장착 여부)
    public class UserItemData
    {
        public int ID;
        public bool HasEquipped;

        public UserItemData(int iD, bool hasEquipped)
        {
            ID = iD;
            HasEquipped = hasEquipped;
        }
    }

    public class UserData
    {
        public int SlotID;
        [JsonConverter(typeof(StringEnumConverter))] // enum 값을 저장할 때 문자열 형태로 저장하기 위한 Attribute
        public EClassType ClassType;
        public int Level;
        public string Name;
        public int HP;
        public float Attack;
        public float Defense;
        public int Gold;
        public List<UserItemData> Items; // 아이템 정보 배열
    }

    public class UserDataLoader : ILoader<int, UserData>
    {
        public List<UserData> UserData = new List<UserData>();

        public Dictionary<int, UserData> MakeData()
        {
            Dictionary<int, UserData> dict = new Dictionary<int, UserData>();

            foreach (UserData user in UserData)
            {
                dict.Add(user.SlotID, user);
            }

            return dict;
        }
    }
}
