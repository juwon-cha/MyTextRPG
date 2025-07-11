using SpartaDungeon.Scene;

namespace SpartaDungeon.Managers
{
    enum ESceneType
    {
        TITLE_SCENE,
        CHARACTER_SETTING_SCENE,
        TOWN_SCENE,
        STATUS_SCENE,
        INVENTORY_SCENE,
        STORE_SCENE,
        DUNGEON_SCENE,
        CAMPSITE_SCENE,
    }

    class SceneManager
    {
        private IDisplayable mCurrentScene;
        private bool mbIsGameRunning = true;

        // 씬들을 미리 생성해서 담아 놓을 딕셔너리
        private Dictionary<ESceneType, IDisplayable> mSceneDict = new Dictionary<ESceneType, IDisplayable>();

        public void Init()
        {
            // 각 씬들 미리 생성
            mSceneDict.Add(ESceneType.TITLE_SCENE, new TitleScene());
            mSceneDict.Add(ESceneType.CHARACTER_SETTING_SCENE, new CharacterSettingScene());
            mSceneDict.Add(ESceneType.TOWN_SCENE, new TownScene());
            mSceneDict.Add(ESceneType.STATUS_SCENE, new StatusScene());
            mSceneDict.Add(ESceneType.INVENTORY_SCENE, new InventoryScene());
            mSceneDict.Add(ESceneType.STORE_SCENE, new StoreScene());
            mSceneDict.Add(ESceneType.DUNGEON_SCENE, new DungeonScene());
            mSceneDict.Add(ESceneType.CAMPSITE_SCENE, new CampsiteScene());

            // 타이틀 씬으로 초기화
            mCurrentScene = mSceneDict[ESceneType.TITLE_SCENE];
        }

        public void DisplayScene()
        {
            if (mCurrentScene != null)
            {
                mCurrentScene.Display();
            }
        }

        // 씬 타입에 따라 씬 변경
        public void ChangeScene(ESceneType scene)
        {
            if(mSceneDict.ContainsKey(scene))
            {
                mCurrentScene = mSceneDict[scene];
            }

            // 씬이 전환될 때마다 유저 데이터 저장
            Manager.Instance.Game.SaveGame();

            // 화면 리프레쉬
            Console.Clear();
        }

        public void ExitGame()
        {
            mbIsGameRunning = false;

            Console.WriteLine("게임 종료");
        }

        public bool IsGameRunning()
        {
            return mbIsGameRunning;
        }
    }
}
