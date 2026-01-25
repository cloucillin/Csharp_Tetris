using System;
using System.Collections.Generic;
using System.Text;

namespace C_进阶__俄罗斯方块
{
    internal class GameScene : ISceneUpdate
    {
        Map map;

        BlockWorker blockWorker;

        //构造函数中new（实例化）一个map对象
        public GameScene()
        {
            map  = new Map();
            blockWorker = new BlockWorker();
        }

        public void Update()
        {
            //地图绘制
            map.Draw();
            blockWorker.Draw();

        }
    }
}
