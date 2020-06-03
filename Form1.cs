using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)
            {
                if (c.GetType().Name == "Label")
                {
                    c.Text = "aa";
                }
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            Assembly assem = Assembly.LoadFrom("ClassLibrary1.dll");  //加载DLL

            Type[] tys = assem.GetTypes();

            Type type = tys[0];
            object classLibrary1 = System.Activator.CreateInstance(type);       // 创建实例 = new

            var m1 = type.GetMethod("Run");
            object[] p = new object[] {"P1" };
            m1.Invoke(classLibrary1, p);

            var m2 = type.GetMethod("Stop");
            m2.Invoke(classLibrary1, p);
        }
        UdpClient _client;
        private void button1_Click(object sender, EventArgs e)
        {
            _client = new UdpClient(int.Parse(this.textBox1.Text));
          //  StartReceive();
        }
        private void StartReceive()
        {
            try
            {
                _client.BeginReceive(new AsyncCallback(OnReceive), _client);
            }
            catch (Exception ex)
            {
              MessageBox.Show(ex.Message);
              if (_client.Client.Connected)
              {
                  StartReceive();
              }
            }
        }

        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint point = new IPEndPoint(IPAddress.Any, 0); //2.2 这里创建一个接受IP和端口号的对象传入到第一步的参数中去
                byte[] data = _client.EndReceive(ar, ref point); //2.1接受数据函数 第一个参数是指定一个IP地址和端口号
                //if (MessageReceived != null)
                //{
                //    MessageReceived(point, data);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                StartReceive();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _client.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var isAvailable = ISPortUsed(int.Parse(textBox1.Text.ToString()));

           
            if (isAvailable)
            {
                MessageBox.Show("端口: " + textBox1.Text.ToString() + " 打开！", "提示");
            }
            else
                MessageBox.Show("端口: " + textBox1.Text.ToString() + "关闭！", "提示");

        }

        private bool ISPortUsed(int port)
        {
            IList portUsed = PortUsing();
            return portUsed.Contains(port);
        }

        private IList PortUsing()
        {
            //获取本地计算机的网络连接和通信统计数据的信息 
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //返回本地计算机上的所有Tcp监听程序 
            IPEndPoint[] ipsTCP = ipGlobalProperties.GetActiveTcpListeners();

            //返回本地计算机上的所有UDP监听程序 
            IPEndPoint[] ipsUDP = ipGlobalProperties.GetActiveUdpListeners();

            //返回本地计算机上的Internet协议版本4(IPV4 传输控制协议(TCP)连接的信息。 
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            List<int> allPorts = new List<int>();
            allPorts.AddRange(ipsTCP.Select(n => n.Port));
            allPorts.AddRange(ipsUDP.Select(n => n.Port));
            allPorts.AddRange(tcpConnInfoArray.Select(n => n.LocalEndPoint.Port));

            return allPorts;
        }

    }
}
