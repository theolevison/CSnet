using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSnet.UDP
{
    public class UDPController : IUDPListener
    {
        private ISocket server;
        private Form1 form;

        public UDPController(ISocket server, Form1 form)
        {
            this.server = server;
            this.form = form;
        }
        public void Start()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(6);

                    byte[] data1 = new byte[258];

                    server.ReceiveFrom(data1);
                    if (data1.Length >= 1)
                    {
                        string cc = $"{data1[0]:X2}";//盒子编号
                        string dd = $"{data1[1]:X2}";//功能码   00 开 01关 02前8个设置 03后8个设置                                             
                        //string ff = ee.Substring(0, 96);
                        switch (cc) 
                        {
                            case "00":
                                form.CloseDevices();
                                break;
                            case "01":
                                form.OpenDevices(); //第一个盒子关闭
                                break;
                            case "02":
                                //关掉计时器
                                break;
                            default: break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //throw ex;
                    Console.WriteLine($"发送失败：{ex.Message}");
                }
            }
        }
    }
}
