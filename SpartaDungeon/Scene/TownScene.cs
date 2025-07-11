using SpartaDungeon.Managers;

namespace SpartaDungeon.Scene
{
    enum ETownInput
    {
        EXIT,
        STATUS,
        INVENTORY,
        STORE,
        DUNGEON,
        CAMPSITE,
    }

    class TownScene : IDisplayable
    {
        public void Display()
        {
            ShowTownScreen();
            ProcessInput();
        }

        private void ShowTownScreen()
        {
            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기전 활동을 할 수 있습니다.");
            
            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전 입장");
            Console.WriteLine("5. 휴식");
            Console.WriteLine();

            Console.Write("원하는 행동을 입력해주세요. \n>> ");
        }

        private void ProcessInput()
        {
            string input = Console.ReadLine();

            // int.TryParse가 성공했을 때(숫자 입력)만 switch문 실행
            if (int.TryParse(input, out int choice))
            {
                switch (choice)
                {
                    case (int)ETownInput.EXIT:
                        Manager.Instance.Scene.ExitGame();
                        return;

                    case (int)ETownInput.STATUS:
                        Manager.Instance.Scene.ChangeScene(ESceneType.STATUS_SCENE);
                        return;

                    case (int)ETownInput.INVENTORY:
                        Manager.Instance.Scene.ChangeScene(ESceneType.INVENTORY_SCENE);
                        return;

                    case (int)ETownInput.STORE:
                        Manager.Instance.Scene.ChangeScene(ESceneType.STORE_SCENE);
                        return;

                    case (int)ETownInput.DUNGEON:
                        Manager.Instance.Scene.ChangeScene(ESceneType.DUNGEON_SCENE);
                        return;

                    case (int)ETownInput.CAMPSITE:
                        Manager.Instance.Scene.ChangeScene(ESceneType.CAMPSITE_SCENE);
                        return;

                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("숫자를 입력해주세요.");
            }

            // 잘못된 입력 후 계속 입력 받을 수 있게 게임 루프 진행
            if (Manager.Instance.Scene.IsGameRunning())
            {
                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
