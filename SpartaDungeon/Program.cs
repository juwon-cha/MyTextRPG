using SpartaDungeon.Managers;

namespace SpartaDungeon
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (Manager.Instance.Scene.IsGameRunning())
            {
                Manager.Instance.Scene.DisplayScene();
            }
        }
    }
}
