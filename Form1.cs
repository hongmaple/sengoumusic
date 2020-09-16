using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Media;
using System.IO;
using System.Runtime.InteropServices;
using System.Collections;

namespace 音频播放
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region//执行条件，存储路径
        int i = 0;
        Point mouseOff;//鼠标移动的坐标
        bool leftFalg;//标记为是否为左键选中
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, uint lParam);
        const uint WM_APPCOMMAND = 0x319;
        const uint APPCOMMAND_VOLUME_UP = 0x0a;
        const uint APPCOMMAND_VOLUME_DOWN = 0x09;
        const uint APPCOMMAND_VOLUME_MUTE = 0x08; 
        //OpenFileDialog open = new OpenFileDialog();     //实例化一个通用对话框
        Dictionary<string, string> listsongs = new Dictionary<string, string>(); //用来存储音乐文件的全路径
        #endregion
        #region//双击播放
        public void play()
        {
            if (listBox1.SelectedIndex<0)
            {
                MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            timer2.Enabled = true;
            if (i >= 3 || i < 0)
            {
                i = 0;
            }
            this.pictureBox1.Image = imageList1.Images[i];//设置动画
            i++;
            timer1.Enabled = true;
            if (listBox1.SelectedIndex != -1)
            {
                axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                label2.Text = listBox1.SelectedItem.ToString();
            }
        }
        public void play2()
        {
            timer2.Enabled = true;
            if (i >= 3 || i < 0)
            {
                i = 0;
            }
            this.pictureBox1.Image = imageList1.Images[i];
            i++;
            timer1.Enabled = true;
        }
        #endregion
        #region//listbox1控件下一首
        public void Slistbox1()
        {
            try
            {
                if (listBox1.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择歌曲","操作提示",MessageBoxButtons.OK,MessageBoxIcon.Information);
                    return;
                }
                int index = listBox1.SelectedIndex; //获得当前选中歌曲的索引
                index++;
                if (comboBox1.Text == "单曲循环")
                {
                    axWindowsMediaPlayer1.settings.setMode("loop", true);
                    axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                    return;
                }
                else if (comboBox1.Text == "顺序播放")
                {
                    if (index == listBox1.Items.Count)
                    {
                        index = 0;
                    }
                    listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                    //sp.SoundLocation = listsongs[index];
                    label2.Text = listBox1.SelectedItem.ToString();
                    axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                    return;
                }
                if (index == listBox1.Items.Count)
                {
                    index = 0;
                }
                listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                //sp.SoundLocation = listsongs[index];
                label2.Text = listBox1.SelectedItem.ToString();
                axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region//listbox1控件上一首
        public void Slistbox2()
        {
            try
            {
                if (listBox1.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int index = listBox1.SelectedIndex; //获得当前选中歌曲的索引
                if (index == 0)
                {
                    return;
                }
                index--;
                if (comboBox1.Text == "单曲循环")
                {
                    axWindowsMediaPlayer1.settings.setMode("loop", true);
                    axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                    return;
                }
                else if (comboBox1.Text == "顺序播放")
                {
                    if (index == listBox1.Items.Count)
                    {
                        index = 0;
                    }
                    listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                    //sp.SoundLocation = listsongs[index];
                    label2.Text = listBox1.SelectedItem.ToString();
                    axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                    return;
                }
                if (index == listBox1.Items.Count)
                {
                    index = 0;
                }
                listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                //sp.SoundLocation = listsongs[index];
                label2.Text = listBox1.SelectedItem.ToString();
                axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
            }
            //}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        private void button4_Click(object sender, EventArgs e)
        {
            loadmusic(sender, e);
        }
        FileInfo[] fileInfos = null;
        private void loadmusic(object sender, EventArgs e)
        {
            try
            {
                DirectoryInfo folder = new DirectoryInfo(@"D:\CloudMusic");//本地计算机音乐路径
                fileInfos = folder.GetFiles("*.mp3");
                if (fileInfos.Length == 0)
                {
                    MessageBox.Show("没有扫描到音乐");
                }
                foreach (FileInfo file in fileInfos)
                {
                    listsongs.Add(file.Name, file.FullName);
                    listBox1.Items.Add(file.Name);  //将音乐文件的文件名加载到listBox中
                    timer2.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loadmusic(sender, e);
            comboBox1.SelectedIndex = 1;
            listBox1.SelectedIndex = 0;
            timer2.Enabled = true;
        }
        #region//定时器
        private void timer1_Tick(object sender, EventArgs e)
        {
            //播放进度定时器
            try
            {
                progressBar1.Maximum = (int)axWindowsMediaPlayer1.currentMedia.duration;//进度量
                progressBar1.Minimum = 0;
                progressBar1.Step = (int)(progressBar1.Maximum / 20);
                progressBar1.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;//设置值
                textBox1.Text = axWindowsMediaPlayer1.Ctlcontrols.currentPositionString;//渲染进度
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void timer2_Tick_1(object sender, EventArgs e)
        {
            //动画定时器
            if (i >=3 || i < 0)
            {
                i = 0;
            }
            this.pictureBox1.Image = imageList1.Images[i];
            i++;
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            //播放键
            if (listBox1.SelectedIndex < 0)
            {
                MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (listBox1.SelectedIndex >= 0)
            {
                play();
                timer1.Enabled = true;
            }
        }
        //从下拉列表选定项而下拉列表关闭时触发
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            axWindowsMediaPlayer1.settings.setMode("loop", true);//让歌曲循环播放
        }
        private void listBox1_DoubleClick_1(object sender, EventArgs e)//歌曲列表，双击歌名执行代码
        {
            play();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //关闭
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //暂停键
            axWindowsMediaPlayer1.Ctlcontrols.pause();//播发器暂停
            timer1.Enabled = false;//关闭定时器
            timer2.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int b = tabControl1.SelectedIndex;
                if (b == 0)
                {

                    int index = listBox1.SelectedIndex; //获得当前选中歌曲的索引
                    if (index == 0)
                    {
                        return;
                    }
                    index--;

                    if (comboBox1.Text == "单曲循环")
                    {
                        if (index == listBox1.Items.Count)
                        {
                            index = 0;
                        }
                        listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                        label2.Text = listBox1.SelectedItem.ToString();
                        axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                        axWindowsMediaPlayer1.settings.setMode("loop", true);
                    }
                    else if (comboBox1.Text == "顺序播放")
                    {
                        if (index == listBox1.Items.Count)
                        {
                            index = 0;
                        }
                        listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                        label2.Text = listBox1.SelectedItem.ToString();
                        axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                    }
                    play();
                    timer1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBox1.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int a = tabControl1.SelectedIndex;
                if (a == 0)
                {
                    int index = listBox1.SelectedIndex; //获得当前选中歌曲的索引

                    index++;
                    if (comboBox1.Text == "单曲循环")
                    {
                        if (index == listBox1.Items.Count)
                        {
                            index = 0;
                        }
                        listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                        label2.Text = listBox1.SelectedItem.ToString();
                        axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                        axWindowsMediaPlayer1.settings.setMode("loop", true);//循环播放的方法
                    }
                    else if (comboBox1.Text == "顺序播放")
                    {
                        //到底了，从头来
                        if (index == listBox1.Items.Count)
                        {
                            index = 0;
                        }
                        listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                        label2.Text = listBox1.SelectedItem.ToString();//获取路径
                        axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];//把路径设置到播放器控件中
                    }
                    play();//播放方法
                    timer1.Enabled = true;//启动播放进度定时器
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //加音量
            SendMessage(this.Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_UP * 0x10000);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //減音量
            SendMessage(this.Handle, WM_APPCOMMAND, 0x30292, APPCOMMAND_VOLUME_DOWN * 0x10000);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        //自动播放，循环播放，自动启动
        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
          
            //当前播放器的播放状态发生改变的是偶，判断当前音乐播放器的播放状态是否到达了Ended， 如果是则进行下一曲
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsMediaEnded)
            {
                if (listBox1.SelectedIndex < 0)
                {
                    MessageBox.Show("请选择歌曲", "操作提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                int a = tabControl1.SelectedIndex;
                if (a == 0)
                {
                    int index = listBox1.SelectedIndex; //获得当前选中歌曲的索引

                    index++;
                    if (comboBox1.Text == "单曲循环")
                    {
                        axWindowsMediaPlayer1.settings.setMode("loop", true);
                        axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                        return;
                    }
                    else if (comboBox1.Text == "顺序播放")
                    {
                        if (index == listBox1.Items.Count)
                        {
                            index = 0;
                        }
                        listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                        label2.Text = listBox1.SelectedItem.ToString();
                        axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                        return;
                    }

                    if (index == listBox1.Items.Count)
                    {
                        index = 0;
                    }
                    listBox1.SelectedIndex = index; //将改变后的索引重新赋值给我当前选中项的索引
                    label2.Text = listBox1.SelectedItem.ToString();
                    axWindowsMediaPlayer1.URL = listsongs[listBox1.SelectedItem.ToString()];
                }
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsReady)     //播放器准备就绪后播放
            {
                //捕获异常 并忽略异常
                try
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                catch (Exception)
                {

                }

            }
        }

        //鼠标按下事件
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseOff = new Point(-e.X, -e.Y);//得到变量的值
                leftFalg = true;//点击左键，按下鼠标时标记为true
            }
        }

        //鼠标移动事件
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                Point mouseSet = Control.MousePosition;
                mouseSet.Offset(mouseOff.X, mouseOff.Y);//设置移动后的坐标
                Location = mouseSet;//设置本地
            }
        }

        //鼠标松开
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (leftFalg)
            {
                leftFalg = false;//释放鼠标后标记为false
            }

        }
    }
}
