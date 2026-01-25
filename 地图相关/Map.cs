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
        private List<DrawObject>dynamicWalls = new List<DrawObject>();

        //重载一次无参构造 去初始化我们的固定墙壁
        public Map()
        {
            for(int i = 0; i < Game.w; i+=2)
            {
                walls.Add(new DrawObject(E_DrawType.Wall, i, Game.h - 6));
            }

            for(int i = 0; i < Game.h-6; i++)
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
            }
        }

    }
}
