using System;
using System.Collections.Generic;
using System.Text;

namespace C_进阶__俄罗斯方块
{

    /// <summary>
    /// 场景类型枚举
    /// </summary>
    enum E_SceneType
    {
        /// <summary>
        /// 开始场景
        /// </summary>
        Begin,
        /// <summary>
        /// 游戏场景
        /// </summary>
        Game,
        /// <summary>
        /// 结束场景
        /// </summary>
        End,

    }


    internal class Game
    {
        //经常用且不变申明成静态成员static：可被修改或const：无法被修改
        //都是类名点出使用
        public const int w = 50;
        public const int h = 35;

        /// <summary>
        /// 当前选中的场景
        /// </summary>
        /// 接口不可被初始化但可被申明
        /// 把这个也设为static
        public static ISceneUpdate nowScene;

        public Game()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(w, h);
            Console.SetBufferSize(w, h);

            ChangeScene(E_SceneType.Begin);
        }

        //游戏开始的方法
        public  void Start()
        {
            //游戏主循环  主要负责  游戏场景逻辑的更新
            while (true)
            {
                //判断当前游戏场景不为空就更新
                if( nowScene != null )
                {
                    nowScene.Update();
                }
            }
        }

        //场景切换的方法
        //要使一个类中的方法供别的类使用最快的方法就是设为静态方法
        //静态方法中不能使用非静态成员变量
        public  static void ChangeScene(E_SceneType type)
        {
            //切换场景之前 应该把上一个场景的绘制内容擦掉
            Console.Clear();

            switch(type)
            {
                case E_SceneType.Begin:
                    nowScene = new BeginScene();
                    break;
                case E_SceneType.Game:
                    nowScene = new GameScene();
                    break;
                case E_SceneType.End:
                    nowScene = new EndScene();
                    break;
            }

        }

    }
}
