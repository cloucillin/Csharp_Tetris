using System;
using System.Collections.Generic;
using System.Text;

namespace C_进阶__俄罗斯方块 
{ 
    internal class EndScene : BeginOrEndBeginScene
    {
        public EndScene()
        {
            strTitle = "游戏结束";
            strOne = "回到开始界面";
        }

        public override void EnterJDoSomething()
        {
            //按J做什么逻辑
            if(nowSelIndex == 0)
            {
                Game.ChangeScene(E_SceneType.Begin);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
