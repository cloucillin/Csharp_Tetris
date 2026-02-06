using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace C_进阶__俄罗斯方块
{
    class GameScene : ISceneUpdate
    {
        Map map;

        BlockWorker blockWorker;



        //构造函数中new（实例化）一个map对象
        public GameScene()
        {
            map  = new Map(this);
            blockWorker = new BlockWorker();

            InputThread.Instance.inputEvent += CheckInputThread;

            ////设置输入线程
            //inputThread = new Thread(CheckInputThread);

            ////设置为后台线程 生命周期随主线程决定，主线程结束，所有后台线程跟着结束
            //inputThread.IsBackground = true;
            ////开启线程
            //inputThread.Start();
        }

        private void CheckInputThread()
        {
            //检测需要一直进行用while(true)设置为常驻线程
            //两个线程访问内存中同一位置的数据(同一个数据内存)由于原子操作顺序的不确定性
            //会出现内存安全问题

            while (true)
            {
                //为了避免影响主线程 在输入后加锁
                //lock()内填的只要是引用类型的即可，使之成为一个互斥信号量
                //只要执行就会该对象访问是否上锁,如果没上锁就锁上这个对象并执行lock{}内的语句块
                //如果上锁则会等待这个对象解锁
                //可以是共享资源本身，也可以是自定义设计出的信号量
                lock (blockWorker)
                {
                    if (Console.KeyAvailable)
                    {
                        switch (Console.ReadKey(true).Key)
                        {
                            case ConsoleKey.LeftArrow:
                                //判断能不能变形
                                if (blockWorker.CanChange(E_Change_Type.Left, map))
                                    blockWorker.Change(E_Change_Type.Left);
                                break;
                            case ConsoleKey.RightArrow:
                                //判断能不能变形
                                if (blockWorker.CanChange(E_Change_Type.Right, map))
                                    blockWorker.Change(E_Change_Type.Right);
                                break;

                            case ConsoleKey.A:
                                if (blockWorker.CanMoveRL(E_Change_Type.Left, map))
                                    blockWorker.MoveRL(E_Change_Type.Left);
                                break;
                            case ConsoleKey.D:
                                if (blockWorker.CanMoveRL(E_Change_Type.Right, map))
                                    blockWorker.MoveRL(E_Change_Type.Right);
                                break;

                            case ConsoleKey.S:
                                if (blockWorker.CanAutoMove(map))
                                    blockWorker.AutoMove();
                                break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 停止线程
        /// </summary>
        public void StopThread()
        {
            //inputThreadIsRunning = false;
            //inputThread = null;
            InputThread.Instance.inputEvent -= CheckInputThread;
        }

        public void Update()
        {
            //锁里面不要包含 休眠 否则 仍会导致卡顿
            lock (blockWorker)
            {
                //地图绘制
                map.Draw();
                //搬运工绘制
                blockWorker.Draw();

                if (blockWorker.CanAutoMove(map))
                    blockWorker.AutoMove();
            }

            //用线程休眠的形式,在检测输入时休眠的是整个主线程，会影响整个线程导致卡顿
            Thread.Sleep(200);


        }
    }
}
