using System;
using System.Collections.Generic;
using System.Text;

namespace C_进阶__俄罗斯方块
{
    internal class BeginScene : BeginOrEndBeginScene
    {
        public BeginScene()
        {
            strTitle = "俄罗斯方块";
            strOne = "开始游戏";
           
        }
        public override void EnterJDoSomething()
        {
            //按J键做什么的逻辑
            if(nowSelIndex == 0)
            {
                Game.ChangeScene(E_SceneType.Game);
            }
            else
            {
                Environment.Exit(0);
            }
        }
    }
}
