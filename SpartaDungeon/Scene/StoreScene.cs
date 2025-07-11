using SpartaDungeon.Utils;
using SpartaDungeon.Items;
using SpartaDungeon.Managers;

namespace SpartaDungeon.Scene
{
    enum EStoreState
    {
        EXIT,
        BUYING,
        SELLING,
        MAINMENU
    }

    public enum EPurchaseResult
    {
        SUCCESS,
        NOT_ENOUGH_GOLD,
        ALREADY_PURCHASED,
        NONE,
    }

    public enum ESellResult
    {
        SUCCESS,
        NOT_IN_INVENTORY
    }

    class StoreScene : IDisplayable
    {
        private EStoreState mCurrentState = EStoreState.MAINMENU;

        public void Display()
        {
            bool inStore = true;
            while (inStore)
            {
                Console.Clear();

                switch (mCurrentState)
                {
                    case EStoreState.MAINMENU:
                        ShowStoreHeader(mCurrentState);
                        DisplayMainMenu(ref inStore);
                        break;

                    case EStoreState.BUYING:
                        ShowStoreHeader(mCurrentState);
                        DisplayBuyingMode();
                        break;

                    case EStoreState.SELLING:
                        ShowStoreHeader(mCurrentState);
                        DisplaySellingMode();
                        break;

                    default:
                        break;
                }
            }
        }

        private void ShowStoreHeader(EStoreState storeState)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            switch (storeState)
            {
                case EStoreState.MAINMENU:
                    Console.WriteLine("상점");
                    Console.ResetColor();
                    Console.WriteLine("필요한 아이템을 얻을 수 있는 상점입니다.\n");
                    break;

                case EStoreState.BUYING:
                    Console.WriteLine("상점 - 아이템 구매");
                    Console.ResetColor();
                    Console.WriteLine("필요한 아이템을 구매할 수 있습니다.\n");
                    break;

                case EStoreState.SELLING:
                    Console.WriteLine("상점 - 아이템 판매");
                    Console.ResetColor();
                    Console.WriteLine("가지고 있는 아이템을 판매할 수 있습니다.\n");
                    break;

                default:
                    break;
            }

            Console.WriteLine("[보유 골드]");
            Console.WriteLine($"{Manager.Instance.Game.PlayerController.Gold} G\n");
        }

        private void DisplayMainMenu(ref bool inStore)
        {
            // 상점 아이템 리스트 출력
            PrintItemList(Manager.Instance.Item.Items);

            Console.WriteLine("\n0. 나가기");
            Console.WriteLine("1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매\n");
            Console.Write("원하는 행동을 입력해주세요. \n>> ");

            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                switch (choice)
                {
                    case (int)EStoreState.EXIT: // 나가기
                        Manager.Instance.Scene.ChangeScene(ESceneType.TOWN_SCENE);
                        inStore = false; // 루프 종료
                        return;
                    case (int)EStoreState.BUYING: // 아이템 구매
                        mCurrentState = EStoreState.BUYING;
                        break;
                    case (int)EStoreState.SELLING: // 아이템 판매
                        mCurrentState = EStoreState.SELLING;
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

        private void DisplayBuyingMode()
        {
            // 상점 아이템 리스트 출력
            PrintItemList(Manager.Instance.Item.Items);

            Console.WriteLine("\n0. 나가기\n");
            Console.Write("구매할 아이템의 번호를 입력해주세요. \n>> ");

            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                if (choice == (int)EStoreState.EXIT)
                {
                    mCurrentState = EStoreState.MAINMENU; // 메인 메뉴로 상태 전환
                    return;
                }

                // 아이템 구매
                Item item = Manager.Instance.Item.GetItemFromStoreList(choice);
                if (item == null)
                {
                    Console.WriteLine("잘못된 번호입니다.");
                }
                else
                {
                    var player = Manager.Instance.Game.PlayerController;
                    EPurchaseResult result = player.BuyItem(item);

                    switch(result)
                    {
                        case EPurchaseResult.NONE:
                            Console.WriteLine("잘못된 번호입니다.");
                            break;

                        case EPurchaseResult.NOT_ENOUGH_GOLD:
                            int currentGold = player.Gold;
                            Console.WriteLine($"{item.Price - currentGold} Gold가 부족합니다.");
                            break;

                        case EPurchaseResult.ALREADY_PURCHASED:
                            Console.WriteLine("이미 구매한 아이템입니다.");
                            break;

                        case EPurchaseResult.SUCCESS:
                            Console.WriteLine($"{item.Name} 구매를 완료했습니다.");
                            break;

                        default:
                            Console.WriteLine("잘못된 번호입니다.");
                            break;
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

        private void DisplaySellingMode()
        {
            // 플레이어가 소지한 아이템 목록 출력
            var inventory = Manager.Instance.Game.PlayerController.InventoryController;
            PrintItemList(inventory.Inventory, true);

            Console.WriteLine("\n0. 나가기\n");
            Console.Write("판매할 아이템의 번호를 입력해주세요. \n>> ");

            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                if (choice == (int)EStoreState.EXIT)
                {
                    mCurrentState = EStoreState.MAINMENU; // 메인 메뉴로 상태 전환
                    return;
                }

                // 아이템 판매
                Item item = inventory.GetItemFromInventory(choice);

                if (item == null)
                {
                    Console.WriteLine("잘못된 번호입니다.");
                }
                else if (item.HasSold)
                {
                    Console.WriteLine("이미 판매한 아이템입니다.");
                }
                else
                {
                    ESellResult result = Manager.Instance.Game.PlayerController.SellItem(item);

                    switch(result)
                    {
                        case ESellResult.NOT_IN_INVENTORY:
                            Console.WriteLine($"오류: 해당 아이템을 소지하고 있지 않습니다.");
                            break;

                        case ESellResult.SUCCESS:
                            Console.WriteLine($"{item.Name} 판매를 완료했습니다.");
                            break;

                        default:
                            Console.WriteLine($"판매를 실패했습니다.");
                            break;
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

        // 아이템 리스트 출력
        protected void PrintItemList(Dictionary<int, Item> itemDict, bool isSellingList = false)
        {
            Console.WriteLine("[아이템 목록]\n");

            // 문자열 포맷 폭 설정
            const int nameWidth = 15;
            const int statWidth = 10;
            const int descWidth = 55;
            const int priceWidth = 10;

            // 헤더를 포맷팅 함수를 적용하여 정렬
            Console.WriteLine(
                StringFormatting.PadRightForMixedText("-", 4) +
                StringFormatting.PadRightForMixedText("아이템 이름", nameWidth) + " | " +
                StringFormatting.PadRightForMixedText("능력치", statWidth) + " | " +
                StringFormatting.PadRightForMixedText("설명", descWidth) + " | " +
                "가격"
            );
            Console.WriteLine(new string('-', 110)); // 구분선

            int count = 1;
            foreach (KeyValuePair<int, Item> item in itemDict)
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
                
                string priceDisplay = "";
                // 판매 가격 표시
                if(isSellingList)
                {
                    priceDisplay = $"{item.Value.SellingPrice} G";
                }
                // 구매 여부에 따라 가격 표시 변경
                else if (item.Value.HasPurchased)
                {
                    priceDisplay = "구매완료";
                }
                else
                {
                    priceDisplay = $"{item.Value.Price} G";
                }

                // 각 부분을 유틸 함수를 이용해 정렬된 문자열로 만든다.
                string countStr = StringFormatting.PadRightForMixedText($"- {count}", 4);
                string nameStr = StringFormatting.PadRightForMixedText(item.Value.Name, nameWidth);
                string statStr = StringFormatting.PadRightForMixedText(statDisplay, statWidth);
                string descStr = StringFormatting.PadRightForMixedText(item.Value.Description, descWidth);
                string priceStr = StringFormatting.PadRightForMixedText(priceDisplay, priceWidth);

                // 정렬된 문자열들을 조합하여 최종 출력
                Console.WriteLine($"{countStr} {nameStr} | {statStr} | {descStr} | {priceStr}");

                ++count;
            }
        }
    }
}
