using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Net;

namespace Socket1
{
    class CheckPort
    {
        //检查端口是否被占用
        public static bool check(int port)
        {
            bool isUse = false;
            Process p = new Process();//提供对本地和远程进程的访问并使您能够启动和停止本地系统进程。
            //Process.StartInfo 属性,获取或设置要传递给 Process 的 Start 方法的属性。
            p.StartInfo = new ProcessStartInfo("netstat", "-a");//指定启动进程时使用的一组值。
            p.StartInfo.CreateNoWindow = true;//获取或设置指示是否在新窗口中启动该进程的值。
            //如果UseShellExecute属性是true或UserName和Password属性不是null、 CreateNoWindow属性值将被忽略，并创建一个新窗口。
            p.StartInfo.UseShellExecute = false;//获取或设置一个值，该值指示是否使用操作系统 shell 启动进程。
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//获取或设置启动进程时使用的窗口状态。
            p.StartInfo.RedirectStandardOutput = true;//获取或设置一个值，该值指示是否将应用程序的输出写入 Process.StandardOutput 流中。
            p.Start();
            string result = p.StandardOutput.ReadToEnd().ToLower();//最后都转换成小写字母
            //Environment.MachineName获取此本地计算机的 NetBIOS 名称。
            if (result.IndexOf("tcp    " + Environment.MachineName.ToLower() + ":" + port) >= 0)
            {
                //TCP端口被占用
                isUse = true;
            }
            if (result.IndexOf("udp    " + Environment.MachineName.ToLower() + ":" + port) >= 0)
            {
                //udp端口被占用
                isUse = true;
            }
            p.Close();
            return isUse;
        }

        /// <summary>
        /// 检测TCP端口是否被占用
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static bool PortInUse(int port)
        {
            bool inUse = false;

            IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
            IPEndPoint[] tcpEndPoints = ipProperties.GetActiveTcpListeners();//获取TCP端口
            //IPEndPoint[] udpEndPoints = ipProperties.GetActiveUdpListeners();//获取UDP端口
            foreach (IPEndPoint endPoint in tcpEndPoints)
            {
                if (endPoint.Port == port)
                {
                    inUse = true;
                    break;
                }
            }
            return inUse;
        }

    }
}
