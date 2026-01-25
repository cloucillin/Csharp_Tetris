using System;
using System.Collections.Generic;
using System.Text;

namespace C_进阶__俄罗斯方块
{
    //基类供子类继承
    internal abstract class BeginOrEndBeginScene : ISceneUpdate
    {
        protected int nowSelIndex = 0;

        protected string strTitle;

        protected string strOne;

        public abstract void EnterJDoSomething();

        public void Update()
        {
            //开始和结束场景的 游戏逻辑
            //选择当前的选项 然后 监听 键盘输入 wsj键输入
            Console.ForegroundColor = ConsoleColor.White;

            //显示标题:窗口宽高在Game类里面
            Console.SetCursorPosition(Game.w/2 - strTitle.Length, 5);
            Console.Write(strTitle);

            //显示下方的选项
            Console.SetCursorPosition(Game.w / 2 - strOne.Length, 8);
            Console.ForegroundColor = nowSelIndex == 0 ? ConsoleColor.Red : ConsoleColor.White;
            Console.WriteLine(strOne);
            Console.SetCursorPosition(Game.w / 2 - strOne.Length, 10);
            Console.ForegroundColor = nowSelIndex == 1 ? ConsoleColor.Red : ConsoleColor.White;
            Console.Write("结束游戏");

            //检测输入
            switch (Console.ReadKey(true).Key)
            {
                case ConsoleKey.W:
                    --nowSelIndex;
                    if( nowSelIndex < 0)
                    {
                        nowSelIndex = 0;
                    }
                    break;
                case ConsoleKey.S:
                    ++nowSelIndex;
                    if(nowSelIndex > 1)
                    {
                        nowSelIndex = 1;
                    }
                    break;
                case ConsoleKey.J:
                    //在J键里面做什么要在子类中实现
                    EnterJDoSomething();
                    break;  

            }
        }
    }
}
