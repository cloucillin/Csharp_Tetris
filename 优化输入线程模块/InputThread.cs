using System;
using System.Collections.Generic;
using System.Text;

namespace C_进阶__俄罗斯方块
{
    //使用线程单例模式好处：
    //这个线程在生命周期中始终只有一个；避免内存垃圾的产生
    //而且拓展性很强
    internal class InputThread
    {
        //线程成员变量
        Thread inputThread;


        public event Action inputEvent;

        //单例模式 
        private static InputThread instance = new InputThread();

        public static InputThread Instance
        {
            get { return instance; } 
        }

        //不允许在外new
        private InputThread()
        {
            inputThread = new Thread(InputCheck);
            inputThread.IsBackground = true;
            inputThread.Start();
        }

        //线程成员变量
        private void InputCheck()
        {
            while (true)
            {
                //实践不为空就会执行
                inputEvent?.Invoke();
            }
        }

    }
}
