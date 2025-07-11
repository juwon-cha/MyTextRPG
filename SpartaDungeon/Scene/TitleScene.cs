using SpartaDungeon.Managers;

namespace SpartaDungeon.Scene
{
    enum ETitleInput
    {
        EXIT,
        NEW_GAME,
        LOAD_GAME,
    }

    class TitleScene : IDisplayable
    {
        public void Display()
        {
            bool inTitle = true;
            while(inTitle)
            {
                ShowTitleScreen();
                ProcessInput(ref inTitle);
            }

        }

        private void ShowTitleScreen()
        {
            Console.Clear(); // 이전 내용 클리어

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@" ____                   _                 ");
            Console.WriteLine(@"/ ___| _ __   __ _ _ __| |_ __ _          ");
            Console.WriteLine(@"\___ \| '_ \ / _` | '__| __/ _` |         ");
            Console.WriteLine(@" ___) | |_) | (_| | |  | || (_| |         ");
            Console.WriteLine(@"|____/| .__/ \__,_|_|   \__\__,_|         ");
            Console.WriteLine(@"|  _ \|_|  _ _ __   __ _  ___  ___  _ __  ");
            Console.WriteLine(@"| | | | | | | '_ \ / _` |/ _ \/ _ \| '_ \ ");
            Console.WriteLine(@"| |_| | |_| | | | | (_| |  __/ (_) | | | |");
            Console.WriteLine(@"|____/ \__,_|_| |_|\__, |\___|\___/|_| |_|");
            Console.WriteLine(@"                   |___/                  ");
            Console.ResetColor();

            Console.WriteLine("\n1. New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("\n0. Exit\n");
            Console.Write("원하는 행동을 입력해주세요. \n>> ");
        }

        private void ProcessInput(ref bool inTitle)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                switch (choice)
                {
                    case (int)ETitleInput.NEW_GAME: // 새 게임 시작
                        Manager.Instance.Scene.ChangeScene(ESceneType.CHARACTER_SETTING_SCENE);
                        inTitle = false; // 루프 종료
                        break;

                    case (int)ETitleInput.LOAD_GAME: // 불러오기
                        bool loadSuccess = Manager.Instance.Game.LoadGame();
                        if (loadSuccess)
                        {
                            Console.WriteLine("게임 데이터를 성공적으로 불러왔습니다.");
                            Console.WriteLine("마을로 이동합니다...");
                            Thread.Sleep(1500);
                            Manager.Instance.Scene.ChangeScene(ESceneType.TOWN_SCENE);
                            inTitle = false; // 루프 종료
                        }
                        else
                        {
                            Console.WriteLine("저장된 게임 데이터가 없습니다.");
                            Thread.Sleep(1500);
                        }
                        break;

                    case (int)ETitleInput.EXIT: // 게임 종료
                        Manager.Instance.Scene.ExitGame();
                        inTitle = false; // 루프 종료
                        break;

                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Thread.Sleep(1000);
                        break;
                }
            }
            else
            {
                Console.WriteLine("숫자를 입력해주세요.");
                Thread.Sleep(1000);
            }
        }
    }
}
