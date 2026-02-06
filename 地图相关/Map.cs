using System;
using System.Collections.Generic;
using System.Text;

namespace C_进阶__俄罗斯方块
{
    internal class Map : IDraw
    {
        //记录固定墙壁
        private List<DrawObject>walls = new List<DrawObject>();

        //动态墙壁
        public List<DrawObject>dynamicWalls = new List<DrawObject>();

        //为了外部能快速得到地图边界

        //动态墙壁的宽容量小方块个数
        public int w;
        public int h;


        //记录每一行有多少个小方块的容器
        //索引对应的是行号
        private int[] recordInfo;

        private GameScene nowGameScene;


        //重载一次无参构造 去初始化我们的固定墙壁
        public Map(GameScene scene)
        {

            this.nowGameScene = scene;

            //设置宽高
            h = Game.h - 6;
            w = 0;
            //这个就代表对应每一行的计数初始化 默认都为0
            recordInfo = new int[h];

            for (int i = 0; i < Game.w; i+=2)
            {
                walls.Add(new DrawObject(E_DrawType.Wall, i, h));
                ++w;
            }

            w -= 2;

            for (int i = 0; i < h ; i++)
            {
                walls.Add(new DrawObject(E_DrawType.Wall, 0, i));
                walls.Add(new DrawObject(E_DrawType.Wall, Game.w - 2, i));

            }

        }

        public void Draw()
        {
            //绘制固定墙壁
            for(int i = 0; i < walls.Count; i++)
            {
                walls[i].Draw();
            }

            //绘制动态墙壁 有才绘制
            for(int i=0;i<dynamicWalls.Count;i++)
            {
                dynamicWalls[i].Draw();
            }

        }

        public void ClearDraw()
        {
            //绘制动态墙壁才有绘制
            for (int i = 0; i < dynamicWalls.Count; i++)
            {
                dynamicWalls[i].clearDraw();
            }

        }

        /// <summary>
        /// 提供给外部添加动态方块的函数
        /// </summary>
        /// <param name="walls"></param>
        public void AddWalls(List<DrawObject> walls)
        {
            for(int i =0; i < walls.Count;i++)
            {
                //传递方块进来时 把类型改成 墙壁类型
                walls[i].ChangeType(E_DrawType.Wall);
                dynamicWalls.Add(walls[i]);

                //在动态墙壁添加处 发现 位置顶满了 就结束
                if (walls[i].pos.y <= 0)
                {
                    //关闭输入线程
                    this.nowGameScene.StopThread();
                    //场景切换 切换到结束界面
                    Game.ChangeScene(E_SceneType.End);
                    
                    return;
                }


                //进行添加动态墙壁的计数
                //根据索引来得到行
                //h 时 Game.h - 6
                //y最大为 Game.h - 7
                recordInfo[h - walls[i].pos.y - 1] += 1;

            }
            //先把之前的动态小方块擦掉
            ClearDraw();
            //检测移除
            CheckClear();  
            //再绘制动态小方块
            Draw();
        }


        /// <summary>
        /// 检测是否垮层
        /// </summary>
        public void CheckClear()
        {
            List<DrawObject> delList = new List<DrawObject>();

            //流程：
            //要选择记录行中有多少个方块的容器
            //数组
            //判断这一行是否满(方块)
            //遍历数组 检测数组里面存的数
            //是不是w-2
            for(int i = 0; i < recordInfo.Length; i++)
            {
                //必须满足条件 才证明满了
                //小方块计数 == w 
                if (recordInfo[i] == w )
                {
                    //如果是证明这行满了
                    //需要去移除
                    //1.这一行的所有小方块移除
                    for(int j = 0; j < dynamicWalls.Count; j++)
                    {
                        //当前通过动态方块的y计算它在哪一行 如果行号
                        //和当前记录索引一致 就证明 应该移除
                        if(i == h - dynamicWalls[j].pos.y - 1)
                        {
                            //移除这个方块 为了安全移除 添加一个纪录列表
                            delList.Add(dynamicWalls[j]);
                        }
                        //2.要这一行之上的所有小方块下移一个单位
                        //如果当前的这个位置 是该行以上 那就该小方块 下移一格
                        else if (h - 1 - dynamicWalls[j].pos.y > i)
                        {
                            ++dynamicWalls[j].pos.y;
                        }
                    }

                    //移除待删除的小方块
                    for(int j = 0; j < delList.Count; j++)
                    {
                        dynamicWalls.Remove(delList[j]);
                    }


                    //3.记录小方块数量的数组从上到下迁移
                    for(int j = i; j< recordInfo.Length - 1; j++)
                    {
                        recordInfo[j] = recordInfo[j + 1];
                    }
                    // 置空最顶的计数
                    recordInfo[recordInfo.Length - 1] = 0;

                    //如果有多层要垮 ，在垮掉一行后再去检测是否需要跨层
                    CheckClear();
                    break;
                }
            }


        }
    }
}
