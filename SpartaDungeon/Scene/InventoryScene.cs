using SpartaDungeon.Controller;
using SpartaDungeon.Items;
using SpartaDungeon.Managers;
using SpartaDungeon.Utils;

namespace SpartaDungeon.Scene
{
    enum EInventoryState
    {
        EXIT,
        EQUIPMENT,
        MAINMENU
    }

    class InventoryScene : IDisplayable
    {
        private EInventoryState mCurrentState = EInventoryState.MAINMENU;

        public void Display()
        {
            bool inInventory = true;
            while(inInventory)
            {
                Console.Clear();

                switch (mCurrentState)
                {
                    case EInventoryState.MAINMENU:
                        ShowInventoryHeader(mCurrentState);
                        DisplayMainMenu(ref inInventory);
                        break;

                    case EInventoryState.EQUIPMENT:
                        ShowInventoryHeader(mCurrentState);
                        DisplayEquipment();
                        break;

                    default:
                        break;
                }
            }
        }

        private void ShowInventoryHeader(EInventoryState storeState)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            switch (storeState)
            {
                case EInventoryState.MAINMENU:
                    Console.WriteLine("인벤토리");
                    Console.ResetColor();
                    Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.\n");
                    break;

                case EInventoryState.EQUIPMENT:
                    Console.WriteLine("인벤토리 - 장착 관리");
                    Console.ResetColor();
                    Console.WriteLine("보유 중인 아이템을 장착할 수 있습니다.\n");
                    break;

                default:
                    break;
            }
        }

        private void DisplayMainMenu(ref bool inStore)
        {
            // 인벤토리 아이템 리스트 출력
            PrintInventory();

            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("1. 장착 관리\n");
            Console.Write("원하는 행동을 입력해주세요. \n>> ");

            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                switch (choice)
                {
                    case (int)EInventoryState.EXIT: // 나가기
                        Manager.Instance.Scene.ChangeScene(ESceneType.TOWN_SCENE);
                        inStore = false; // 루프 종료
                        return;

                    case (int)EInventoryState.EQUIPMENT: // 아이템 구매
                        mCurrentState = EInventoryState.EQUIPMENT;
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

        private void DisplayEquipment()
        {
            PrintInventory();

            Console.WriteLine("\n0. 나가기\n");
            Console.Write("장착할 아이템의 번호를 입력해주세요. \n>> ");

            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                if (choice == (int)EInventoryState.EXIT)
                {
                    mCurrentState = EInventoryState.MAINMENU; // 메인 메뉴로 상태 전환
                    return;
                }

                // 아이템 장착
                var playerController = Manager.Instance.Game.PlayerController;
                Item item = playerController.InventoryController.GetItemFromInventory(choice); // 아이템 찾기

                if(item == null)
                {
                    Console.WriteLine("잘못된 번호입니다.");
                }
                else
                {
                    // 아이템 장착
                    playerController.EquipItem(item);

                    if (item.HasEquipped)
                    {
                        Console.WriteLine($"{item.Name} 장착을 완료했습니다.");
                    }
                    else
                    {
                        Console.WriteLine($"{item.Name} 장착을 해제했습니다.");
                    }
                }

                Thread.Sleep(1000);
            }
            else
            {
                Console.WriteLine("숫자를 입력해주세요.");
                Thread.Sleep(1000);
            }
        }

        private void PrintInventory()
        {
            Console.WriteLine("[아이템 목록]\n");

            // 문자열 포맷 폭 설정
            const int nameWidth = 15;
            const int statWidth = 10;
            const int descWidth = 55;

            // 헤더를 포맷팅 유틸 함수 적용하여 정렬
            Console.WriteLine(
                StringFormatting.PadRightForMixedText("-", 4) +
                StringFormatting.PadRightForMixedText("아이템 이름", nameWidth) + " | " +
                StringFormatting.PadRightForMixedText("능력치", statWidth) + " | " +
                StringFormatting.PadRightForMixedText("설명", descWidth)
            );
            Console.WriteLine(new string('-', 110)); // 구분선

            var inventory = Manager.Instance.Game.PlayerController.InventoryController.Inventory;
            int count = 1;
            foreach (KeyValuePair<int, Item> item in inventory)
            {
                // 아이템 타입에 따라 능력치 구분
                string statDisplay = "";

                if (item.Value.ItemType == EItemType.ITEM_WEAPON)
                {
                    Weapon weapon = (Weapon)item.Value;
                    statDisplay = $"공격력 +{weapon.Damage}";
                }
                else if (item.Value.ItemType == EItemType.ITEM_ARMOR)
                {
                    Armor armor = (Armor)item.Value;
                    statDisplay = $"방어력 +{armor.Defense}";
                }

                // 각 부분을 도우미 함수를 이용해 정렬된 문자열로 만든다.
                string countStr = StringFormatting.PadRightForMixedText($"- {count}", 4);
                string nameStr = StringFormatting.PadRightForMixedText(item.Value.Name, nameWidth);
                string statStr = StringFormatting.PadRightForMixedText(statDisplay, statWidth);
                string descStr = StringFormatting.PadRightForMixedText(item.Value.Description, descWidth);

                if(item.Value.HasEquipped)
                {
                    nameStr = "[E]" + nameStr;
                }

                // 정렬된 문자열들을 조합하여 최종 출력
                Console.WriteLine($"{countStr} {nameStr} | {statStr} | {descStr}");

                ++count;
            }
        }
    }
}
