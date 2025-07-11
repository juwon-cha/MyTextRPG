using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon.Managers
{
    class Manager
    {
        private static Manager mInstance;

        public static Manager Instance
        {
            get
            {
                if (mInstance == null)
                {
                    mInstance = new Manager();
                    mInstance.Init();
                }

                return mInstance;
            }
        }

        public SceneManager Scene { get; private set; }
        public GameManager Game { get; private set; }
        public ItemManager Item { get; private set; }
        public DungeonManager Dungeon { get; private set; }

        private void Init()
        {
            // 게임 데이터(json) 로드
            DataManager.LoadGameData();

            Scene = new SceneManager();
            Game = new GameManager();
            Item = new ItemManager();
            Dungeon = new DungeonManager();

            Scene.Init();
            Item.Init();
        }
    }
}
