using SpartaDungeon.Items;
using SpartaDungeon.Managers;
using SpartaDungeon.Utils;

namespace SpartaDungeon.Scene
{
    class StatusScene : IDisplayable
    {
        public void Display()
        {
            ShowStatusScreen();
            ProcessInput();
        }

        private void ShowStatusScreen()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("상태 보기");
            Console.ResetColor();

            Console.WriteLine("캐릭터의 정보가 표시됩니다.\n");

            var player = Manager.Instance.Game.PlayerController;

            Console.WriteLine($"Lv. {player.Level:D2}");
            Console.WriteLine($"{player.Name} ( {StringConverter.ClassTypeToString(player.ClassType)} )");

            // 공격력 출력 부분
            string attackStat = $"공격력 : {player.Attack}";
            if (player.BonusAttack > 0)
            {
                attackStat += $" (+{player.BonusAttack})";
            }
            Console.WriteLine(attackStat);

            // 방어력 출력 부분
            string defenseStat = $"방어력 : {player.Defense}";
            if (player.BonusDefense > 0)
            {
                defenseStat += $" (+{player.BonusDefense})";
            }
            Console.WriteLine(defenseStat);

            Console.WriteLine($"체 력 : {player.HP}");
            Console.WriteLine($"Gold : {player.Gold} G\n");

            Console.WriteLine("0. 나가기\n");

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
                    case 0:
                        Manager.Instance.Scene.ChangeScene(ESceneType.TOWN_SCENE);
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

            // 계속 입력 받을 수 있게 게임 루프 진행
            if (Manager.Instance.Scene.IsGameRunning())
            {
                Console.WriteLine("\n계속하려면 아무 키나 누르세요...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private bool HasEquippedItem()
        {
            // 장착한 아이템이 하나라도 있으면 true 반환
            var inventory = Manager.Instance.Game.PlayerController.InventoryController.Inventory;
            foreach (KeyValuePair<int, Item> item in inventory)
            {
                if (item.Value.HasEquipped)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
