using SpartaDungeon.Controller;
using SpartaDungeon.Managers;
using SpartaDungeon.Utils;

namespace SpartaDungeon.Scene
{
    class CharacterSettingScene : IDisplayable
    {
        public void Display()
        {
            // 이름 입력
            string name = SetPlayerName();

            // 직업 선택
            EClassType classType = SetPlayerClass();

            // 게임 시작
            Console.WriteLine($"\n{name}({StringConverter.ClassTypeToString(classType)})님, 스파르타 마을로 여정을 시작합니다!");
            Thread.Sleep(2000);

            Manager.Instance.Game.SetPlayerData(name, classType);

            // 마을로 씬 전환
            Manager.Instance.Scene.ChangeScene(ESceneType.TOWN_SCENE);
        }

        private string SetPlayerName()
        {
            string name;
            while (true) // 이름이 확정될 때까지 반복
            {
                Console.Clear();
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");
                Console.Write("원하는 이름을 설정해주세요.\n");
                Console.Write(">> ");
                name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("이름은 공백일 수 없습니다. 잠시 후 다시 시도해주세요.");
                    Thread.Sleep(1000);
                    continue; // 이름 입력 다시 받기
                }

                // 이름 확인 루프
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine($"입력하신 이름은 \"{name}\"입니다.\n");

                    Console.WriteLine("1. 저장");
                    Console.WriteLine("2. 취소\n");
                    Console.Write(">> ");

                    string choiceInput = Console.ReadLine();
                    if (int.TryParse(choiceInput, out int choice))
                    {
                        if (choice == 1)
                        {
                            return name; // 이름이 확정되었으므로 반환하고 함수 종료
                        }
                        if (choice == 2)
                        {
                            break; // 이름 확인 루프를 빠져나가 이름 입력부터 다시 시작
                        }
                    }
                    Console.WriteLine("잘못된 입력입니다.");
                    Thread.Sleep(1000);
                }
            }
        }

        private EClassType SetPlayerClass()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 던전에 오신 여러분 환영합니다.");

                Console.WriteLine("원하는 직업을 선택해주세요.\n");
                Console.WriteLine("1. 전사");
                Console.WriteLine("2. 마법사");
                Console.WriteLine("3. 궁수");
                Console.WriteLine("4. 도적");
                Console.Write(">> ");

                string input = Console.ReadLine();
                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 4)
                {
                    EClassType selectedClass = (EClassType)(choice - 1);

                    // 직업 확인 루프
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"선택하신 직업은 \"{StringConverter.ClassTypeToString(selectedClass)}\"입니다.\n");
                        
                        Console.WriteLine("1. 저장");
                        Console.WriteLine("2. 취소");
                        Console.Write(">> ");

                        string confirmInput = Console.ReadLine();
                        if (int.TryParse(confirmInput, out int confirmChoice))
                        {
                            if (confirmChoice == 1)
                            {
                                return selectedClass; // 직업 확정
                            }
                            if (confirmChoice == 2)
                            {
                                break; // 직업 확인 루프를 빠져나가 직업 선택부터 다시 시작
                            }
                        }
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
        }
    }
}
