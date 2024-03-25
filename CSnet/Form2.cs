using LiuQF.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSnet
{
    public partial class Form2 : Form
    {
        // IntPtr m_hObject;
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //TODO: use XML (better support in C#) instead of INI, if we actually want to save data.

            //默认地址  64F9C0 0000 000000

            //配置文件
            //public string PLCIP = MyINI.GetIniKeyValueForStr("PLCSet", "PLCIP", Application.StartupPath + "\\config.ini")
            textBox1.Text = MyINI.GetIniKeyValueForStr("macAddress", "1", Application.StartupPath + "\\config.ini");
            textBox2.Text = MyINI.GetIniKeyValueForStr("macAddress", "2", Application.StartupPath + "\\config.ini");
            textBox3.Text = MyINI.GetIniKeyValueForStr("macAddress", "3", Application.StartupPath + "\\config.ini");
            textBox4.Text = MyINI.GetIniKeyValueForStr("macAddress", "4", Application.StartupPath + "\\config.ini");
            textBox5.Text = MyINI.GetIniKeyValueForStr("macAddress", "5", Application.StartupPath + "\\config.ini");
            textBox6.Text = MyINI.GetIniKeyValueForStr("macAddress", "6", Application.StartupPath + "\\config.ini");
            textBox7.Text = MyINI.GetIniKeyValueForStr("macAddress", "7", Application.StartupPath + "\\config.ini");
            textBox8.Text = MyINI.GetIniKeyValueForStr("macAddress", "8", Application.StartupPath + "\\config.ini");
            textBox9.Text = MyINI.GetIniKeyValueForStr("macAddress", "9", Application.StartupPath + "\\config.ini");
            textBox10.Text = MyINI.GetIniKeyValueForStr("macAddress", "10", Application.StartupPath + "\\config.ini");
            textBox11.Text = MyINI.GetIniKeyValueForStr("macAddress", "11", Application.StartupPath + "\\config.ini");
            textBox12.Text = MyINI.GetIniKeyValueForStr("macAddress", "12", Application.StartupPath + "\\config.ini");
            textBox13.Text = MyINI.GetIniKeyValueForStr("macAddress", "13", Application.StartupPath + "\\config.ini");
            textBox14.Text = MyINI.GetIniKeyValueForStr("macAddress", "14", Application.StartupPath + "\\config.ini");
            textBox15.Text = MyINI.GetIniKeyValueForStr("macAddress", "15", Application.StartupPath + "\\config.ini");
            textBox16.Text = MyINI.GetIniKeyValueForStr("macAddress", "16", Application.StartupPath + "\\config.ini");
        }

        public static List<string> GetStoredMacAddresses()
        {
            List<string> macAddresses = new List<string>();

            for (int i = 0; i < 16; i++)
            {
                //read mac addresses from user input (Form 2)
                string address = MyINI.GetIniKeyValueForStr("macAddress", $"{i + 1}", Application.StartupPath + "\\config.ini");
                if (address != "000000") // TODO: do some more input validation
                {
                    macAddresses.Add(address);
                }
            }

            return macAddresses;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MyINI.WritePrivateProfileString("macAddress", "1", textBox1.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "2", textBox2.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "3", textBox3.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "4", textBox4.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "5", textBox5.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "6", textBox6.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "7", textBox7.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "8", textBox8.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "9", textBox9.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "10 ", textBox10.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "11 ", textBox11.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "12 ", textBox12.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "13 ", textBox13.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "14 ", textBox14.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "15 ", textBox15.Text, Application.StartupPath + "\\config.ini");
            MyINI.WritePrivateProfileString("macAddress", "16 ", textBox16.Text, Application.StartupPath + "\\config.ini");

            MessageBox.Show("Data has been saved successfully!");
            // Form2 fw = new Form2();
            // fw.Close();
            // Process.GetCurrentProcess().Kill();
           // Application.Exit();
        }
    }
}
