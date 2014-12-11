using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Lottery
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer();

        private int finalValue = 0;

        private List<int> SelectedNumList = new List<int>();

        private Random random = new Random();

        /// <summary>
        /// 抽奖人数
        /// </summary>
        public int Count = 15;
        public MainWindow()
        {
            InitializeComponent();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_Tick);
        }

        //监视转动是否停止，如果停止，显示价格
        void timer_Tick(object sender, EventArgs e)
        {
            if (numberGroupMain.IsStoped())
            {
                //textBoxFinalPrice.Text = "￥" + finalValue.ToString("F2");//显示最终金额
                string str = "";
                foreach (var item in this.SelectedNumList)
                {
                    str += item.ToString() + '，';
                }
                textBoxFinalPrice.Text = str;
                timer.Stop();
                buttonStart.IsEnabled = true;
            }
        }

        //开始按钮点击
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            buttonStart.IsEnabled = false;
            buttonStop.IsEnabled = true;
            numberGroupMain.TurnStart();
            //textBoxFinalPrice.Text = "";
            timer.Stop();
        }

        //停止按钮点击
        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            if (this.SelectedNumList.Count >= Count)
            {
                MessageBox.Show("还抽啥？直接发奖呗！");
                e.Handled = true;
                return;
            }

            buttonStop.IsEnabled = false;
            bool flag = true;
            while (flag)
            {
                finalValue = random.Next(1,16);
                if (this.SelectedNumList.Contains(finalValue) == false) //不在已抽中序列里面
                {
                    this.SelectedNumList.Add(finalValue);
                    flag = false;
                    break;
                }
                else
                {
                    continue;
                }
            }
            numberGroupMain.TurnStop(finalValue);//使数字组停止
            timer.Start();
        }
    }
}
