using System;
using System.Collections.Generic;
using System.Text;

namespace C_进阶__俄罗斯方块
{
    /// <summary>
    /// 变形左右枚举 决定顺时针还是逆时针
    /// </summary>
    enum E_Change_Type
    {
        Left,
        Right,
    }
    internal class BlockWorker:IDraw
    {
        //方块们
        private List<DrawObject> blocks;
        //心中要默默的知道 各个形状的方块信息是什么
        //选择一个容器来记录各个方块的形态信息

        //用list和Dictionary都可以用来存储方块的几种信息
        //选择DIctionary的主要目的是号召 并且起到练习作用
        private Dictionary<E_DrawType, BlockInfo> blockInfoDic;

        //记录随机创建出来的方块的 具体形态信息
        private BlockInfo nowBlockInfo;

        //当前的形态索引
        private int nowInfoIndex;


        public BlockWorker()
        {
            //初始化 装块信息
            blockInfoDic = new Dictionary<E_DrawType, BlockInfo>()
            {
                {E_DrawType.Cube,new BlockInfo(E_DrawType.Cube)},
                {E_DrawType.Line,new BlockInfo(E_DrawType.Line)},
                {E_DrawType.Tank,new BlockInfo(E_DrawType.Tank)},
                {E_DrawType.Left_Ladder,new BlockInfo(E_DrawType.Left_Ladder)},
                {E_DrawType.Right_Ladder,new BlockInfo(E_DrawType.Right_Ladder)},
                {E_DrawType.Left_Long_Ladder,new BlockInfo(E_DrawType.Left_Long_Ladder)},
                {E_DrawType.Right_Long_Ladder,new BlockInfo(E_DrawType.Right_Long_Ladder)},
            };

            RandomCreateBlock();
        }

        public void Draw()
        {
            for(int i = 0; i < blocks.Count; i++)
            {
                blocks[i].Draw();
            }
        }

        /// <summary>
        /// 随机创建一个方块
        /// </summary>
        public void RandomCreateBlock()
        {
            //随机方块类型
            Random r = new Random();

            //用整形强转成枚举枚举整形默认是从0开始，Wall不用随机
            E_DrawType type = (E_DrawType)r.Next(1, 8);

            //每次新建一个 砖块 其实就是创建4个小方形
            blocks = new List<DrawObject>()
            {
                new DrawObject(type),
                new DrawObject(type),
                new DrawObject(type),
                new DrawObject(type),
            };

            //需要初始化方块位置
            //原点位置 我们随机 自己定义 方块List中第0个就是我们的原点方块
            blocks[0].pos = new Position(24,5);


            //其它三个方块的位置 都是根据相对偏移值算出来的
            //取出方块的形态信息 来进行具体的随机
            //应该把取出来的 方块具体的形态信息 存起来 用于之后变形
            nowBlockInfo = blockInfoDic[type];
            //随机几种形态中的一种来设置方块的信息
            nowInfoIndex = r.Next(0,nowBlockInfo.Count);
            //取出其中一种形态的坐标信息
            Position[] pos = nowBlockInfo[nowInfoIndex];
            for(int i = 0; i < pos.Length; i++)
            {
                //取出来的pos是相对原点方块的坐标 所以需要进行计算
                blocks[i + 1].pos =pos[i] + blocks[0].pos;
            }



        }


        public void Change(E_Change_Type type)
        {
            //自动补全switch：写完switch双击Tab，然后在()填枚举然后enter回车
            switch (type)
            {
                case E_Change_Type.Left:
                    break;
                case E_Change_Type.Right:
                    break;
            }
        }

    }
}
