using SpartaDungeon.Managers;

namespace SpartaDungeon.Scene
{
    public enum EDungeonLevel
    {
        EASY = 1,
        NORMAL,
        HARD
    }

    enum EDungeonState
    {
        EXIT,
        MAINMENU,
        CLEAR,
        FAIL
    }

    class DungeonScene : IDisplayable
    {
        private EDungeonState mCurrentState = EDungeonState.MAINMENU;
        private DungeonResult mLastResult;

        public void Display()
        {
            bool inDungeon = true;

            while(inDungeon)
            {
                Console.Clear();

                switch(mCurrentState)
                {
                    case EDungeonState.MAINMENU:
                        ShowDungeonHeader();
                        ProcessMainMenuInput(ref inDungeon);
                        break;

                    case EDungeonState.CLEAR:
                    case EDungeonState.FAIL:
                        ShowResultUI();
                        ProcessResultInput(ref inDungeon);
                        break;

                    default:
                        break;
                }
            }
        }

        private void ShowDungeonHeader()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("던전 입장");
            Console.ResetColor();
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.\n");
            Console.WriteLine("1. 쉬운 던전\t | 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전\t | 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전\t | 방어력 17 이상 권장\n");
            Console.WriteLine("0. 나가기\n");
            Console.Write("원하는 행동을 입력해주세요. \n>> ");
        }

        private void ShowResultUI()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(mCurrentState == EDungeonState.CLEAR ? "던전 클리어" : "던전 공략 실패");
            Console.ResetColor();

            var player = Manager.Instance.Game.PlayerController;
            if (mCurrentState == EDungeonState.CLEAR)
            {
                Console.WriteLine("축하합니다!");
                Console.WriteLine("던전을 클리어 하였습니다.\n");
                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {mLastResult.InitialHP} -> {player.HP}");
                Console.WriteLine($"Gold {mLastResult.RewardGold} G 획득 -> : {player.Gold} G\n");
            }
            else // FAIL
            {
                Console.WriteLine("던전 공략에 실패했습니다.\n");
                Console.WriteLine("[탐험 결과]");
                Console.WriteLine($"체력 {mLastResult.InitialHP} -> {player.HP}\n");
            }

            Console.WriteLine("0. 나가기\n");
            Console.Write("원하는 행동을 입력하세요.\n>> ");
        }

        private void ProcessMainMenuInput(ref bool inDungeon)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                if (choice == 0)
                {
                    Manager.Instance.Scene.ChangeScene(ESceneType.TOWN_SCENE);
                    inDungeon = false;
                    return;
                }

                if (choice >= 1 && choice <= 3)
                {
                    // 플레이어 체력이 0이면 던전 입장 불가
                    if (Manager.Instance.Game.PlayerController.HP <= 0)
                    {
                        Console.WriteLine("던전에 입장할 힘이 남아 있지 않습니다.");
                        Thread.Sleep(1000);
                        return;
                    }

                    // DungeonManager에게 던전 입장을 요청
                    mLastResult = Manager.Instance.Dungeon.TryEnterDungeon((EDungeonLevel)choice);
                    // 결과에 따라 씬 변경
                    mCurrentState = mLastResult.IsClear ? EDungeonState.CLEAR : EDungeonState.FAIL;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                }
            }
            else
            {
                Console.WriteLine("숫자를 입력해주세요.");
                Thread.Sleep(1000);
            }
        }

        private void ProcessResultInput(ref bool inDungeon)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                if (choice == (int)EDungeonState.EXIT)
                {
                    mCurrentState = EDungeonState.MAINMENU; // 메인 메뉴로 상태 전환
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }

                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("숫자를 입력해주세요.");
                Thread.Sleep(1000);
            }
        }
    }
}
