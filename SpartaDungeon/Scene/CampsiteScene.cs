using SpartaDungeon.Managers;

namespace SpartaDungeon.Scene
{
    public enum ERestResult
    {
        SUCCESS,
        NOT_ENOUGH_GOLD,
        FULL_HP
    }

    class CampsiteScene : IDisplayable
    {
        enum Campsite { CAMPSITE_PRICE = 500 }

        public void Display()
        {
            bool stayCamp = true;

            while(stayCamp)
            {
                Console.Clear();

                ShowCampsiteScreen();

                ProcessInput(ref stayCamp);
            }            
        }

        private void ShowCampsiteScreen()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("휴식하기");
            Console.ResetColor();

            var player = Manager.Instance.Game.PlayerController;
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {player.Gold} G)\n");

            Console.WriteLine("0. 나가기");
            Console.WriteLine("1. 휴식하기\n");

            Console.Write("원하는 행동을 입력해주세요. \n>> ");
        }

        private void ProcessInput(ref bool stayCamp)
        {
            string input = Console.ReadLine();

            // int.TryParse가 성공했을 때(숫자 입력)만 switch문 실행
            if (int.TryParse(input, out int choice))
            {
                switch (choice)
                {
                    case 0:
                        Manager.Instance.Scene.ChangeScene(ESceneType.TOWN_SCENE);
                        stayCamp = false;
                        return;

                    case 1:
                        var player = Manager.Instance.Game.PlayerController;
                        ERestResult result = player.RestAtCampsite((int)Campsite.CAMPSITE_PRICE);

                        switch(result)
                        {
                            case ERestResult.NOT_ENOUGH_GOLD:
                                Console.WriteLine("Gold가 부족합니다.");
                                break;

                            case ERestResult.FULL_HP:
                                Console.WriteLine("체력이 이미 가득 찼습니다.");
                                break;

                            case ERestResult.SUCCESS:
                                Console.WriteLine("체력이 모두 회복되었습니다.");
                                break;
                        }

                        Thread.Sleep(1000);
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
