using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
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
    class BlockWorker:IDraw
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
            blocks[0].pos = new Position(24,-5);


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

        public void ClearDraw()
        {
            for(int i =0; i < blocks.Count;i++)
            {
                blocks[i].clearDraw();
            }
        }

        /// <summary>
        /// 变形
        /// </summary>
        /// <param name="type">左变形还是右变</param>
        public void Change(E_Change_Type type)
        {
            //变之前把之前的位置擦除
            ClearDraw();


            //自动补全switch：写完switch双击Tab，然后在()填枚举然后enter回车
            switch (type)
            {
                case E_Change_Type.Left:
                    --nowInfoIndex;
                    if (nowInfoIndex < 0)
                        nowInfoIndex = nowBlockInfo.Count-1;
                    break;
                case E_Change_Type.Right:
                    ++nowInfoIndex;
                    if (nowInfoIndex >= nowBlockInfo.Count)
                        nowInfoIndex = 0;
                    break;
            }
            //得到索引的目的 是得到对应形态的 位置偏移信息
            //用于设置另外的三个小方块
            Position[] pos = nowBlockInfo[nowInfoIndex];
            //将另外的三个小方块进行设置 计算
            for (int i = 0; i < pos.Length; i++)
            {
                //取出来的pos是相对原点方块的坐标 所以需要进行计算
                blocks[i + 1].pos = pos[i] + blocks[0].pos;
            }


            //变之后再来绘制
            Draw();
        }

        /// <summary>
        /// 判断是否能变，能变才执行Change
        /// 本质是先让这个变一下，看看是否符合规则，如果符合则执行这个变
        /// </summary>
        /// <param name="type"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public bool CanChange(E_Change_Type type,Map map)
        {
            //用一个临时变量记录 当前索引 不变化当前索引
            //不变化当前索引而是变化这个临时变量
            int nowIndex = nowInfoIndex;
            switch (type)
            {
                case E_Change_Type.Left:
                    --nowIndex;
                    if (nowInfoIndex < 0)
                        nowInfoIndex = nowBlockInfo.Count - 1;
                    break;
                case E_Change_Type.Right:
                    ++nowIndex;
                    if(nowInfoIndex >= nowBlockInfo.Count)
                        nowInfoIndex = 0;
                    break;
            }

            //通过临时索引 取出形态信息 用于重合判断
            Position[] nowPos = nowBlockInfo[nowIndex];
            //判断是否超出地图边界
            Position tempPos;
            for(int i = 0; i < nowPos.Length; i++)
            {
                tempPos = blocks[0].pos + nowPos[i];
                //判断左右边界 和 下边界
                if(tempPos.x < 2 ||
                    tempPos.x >= Game.w -4 ||
                    tempPos.y >= map.h)
                {
                    return false;
                }
            }

            //判断是否和地图上的动态方块重合
            for(int i =0; i < nowPos.Length; i++)
            {
                tempPos = blocks[0].pos + nowPos[i];
                for(int j = 0;j< map.dynamicWalls.Count; j++)
                {
                    if( tempPos == map.dynamicWalls[j].pos)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 方块左右移动的函数
        /// </summary>
        /// <param name="type">左或者右</param>
        public void MoveRL(E_Change_Type type)
        {
            //在动之前要得到原来的坐标 进行擦除
            ClearDraw();

            //根据传入参数的类型 决定是左动还是右动
            //左动 x-2,y0   右动 x 2 y 0
            //得到我们的便宜位置
            Position movePos = new Position(type == E_Change_Type.Left ? -2 : 2, 0);
            // 遍历我的所有小方块
            for (int i = 0; i < blocks.Count; i++)
            {
                blocks[i].pos += movePos;
            }

            //动之后 再画上去
            Draw();
        }

        /// <summary>
        /// 移动之前判断 判断可以进行左右移动
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool CanMoveRL(E_Change_Type type, Map map)
        {
            //根据传入参数的类型 决定是左动还是右动
            //左动 x-2,y0   右动 x 2 y 0
            //得到我们的便宜位置
            Position movePos = new Position(type == E_Change_Type.Left ? -2 : 2, 0);

            //要不和左右边界重合
            //动过后的结果 不能直接改小方块的位置 改了就覆水难收
            //这知识想预判断 所以 得到一个临时变量用于判断即可
            Position pos;
            for (int i = 0; i < blocks.Count; i++)
            {
                pos = blocks[i].pos + movePos;
                if (pos.x < 2 || pos.x > Game.w - 4)
                    return false;
            }

            //要不和动态方块重合
            for (int i = 0; i < blocks.Count; i++)
            {
                pos = blocks[i].pos + movePos;
                for (int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if (pos == map.dynamicWalls[j].pos)
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 自动向下移动
        /// </summary>
        public void AutoMove()
        {
            //变位置之前擦除
            ClearDraw();

            //首先要得到移动的多少
            //Position downMove = new Position(0, 1);
            //得到所有的方块 让其向下移动
            for(int i =0;i<blocks.Count; i++)
            {
                blocks[i].pos.y += 1;
            }



            //变了位置再画
            Draw();

        }

        public bool CanAutoMove(Map map)
        {

            //用临时变量存储下一次移动的位置 然后用于进行重合判断
            Position movePos = new Position(0, 1);

            Position pos;

            //1.边界
            for(int i = 0; i < blocks.Count; i++)
            {
                pos = blocks[i].pos + movePos;
                if(pos.y >= map.h)
                {
                    //停下来 给予地图动态方块
                    map.AddWalls(blocks);
                    //随机创建新的方块
                    RandomCreateBlock();

                    return false;
                }
            }

            //2.动态方块
            for(int i = 0; i < blocks.Count; i++)
            {
                pos = blocks[i].pos + movePos;
                for(int j = 0; j < map.dynamicWalls.Count; j++)
                {
                    if( pos == map.dynamicWalls[j].pos)
                    {
                        //停下来给予动态方块
                        map.AddWalls(blocks);
                        //随机创建新的方块
                        RandomCreateBlock();

                        return false;
                    }  
                }
            }

            //先写： 默认返回可以移动
            return true;
        }


    }
}
