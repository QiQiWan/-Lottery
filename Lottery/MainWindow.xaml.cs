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
                #region 按照一、二、三、四等奖显示
                switch (this.SelectedNumList.Count)
                {
                    case 1: //一等奖1个
                        this.textBoxFirstPrice.Text = this.SelectedNumList[0].ToString();
                        break;
                    case 2: //二等奖2个中的第1个
                        this.textBoxSecondPrice.Text = this.SelectedNumList[1].ToString();
                        break;
                    case 3: //二等奖2个中的第2个
                        this.textBoxSecondPrice.Text = this.SelectedNumList[1].ToString() + "，" + this.SelectedNumList[2].ToString();
                        break;
                    case 4: //三等奖3个中的第1个
                        this.textBoxThirdPrice.Text = this.SelectedNumList[3].ToString();
                        break;
                    case 5: //三等奖3个中的第2个
                        this.textBoxThirdPrice.Text = this.SelectedNumList[3].ToString() + "，" + this.SelectedNumList[4].ToString();
                        break;
                    case 6: //三等奖3个中的第3个
                        this.textBoxThirdPrice.Text = this.SelectedNumList[3].ToString() + "，" + this.SelectedNumList[4].ToString() + "，" + this.SelectedNumList[5].ToString();
                        break;
                    case 7: //四等奖4个中的第1个
                        this.textBoxFourthPrice.Text = this.SelectedNumList[6].ToString();
                        break;
                    case 8: //四等奖4个中的第2个
                        this.textBoxFourthPrice.Text = this.SelectedNumList[6].ToString() + "，" + this.SelectedNumList[7].ToString();
                        break;
                    case 9: //四等奖4个中的第3个
                        this.textBoxFourthPrice.Text = this.SelectedNumList[6].ToString() + "，" + this.SelectedNumList[7].ToString() + "，" + this.SelectedNumList[8].ToString();
                        break;
                    case 10: //四等奖4个中的第4个
                        this.textBoxFourthPrice.Text = this.SelectedNumList[6].ToString() + "，" + this.SelectedNumList[7].ToString() + "，" + this.SelectedNumList[8].ToString() + "，" + this.SelectedNumList[9].ToString();
                        break;
                }
                #endregion
                //string str = "";
                //foreach (var item in this.SelectedNumList)
                //{
                //    str += item.ToString() + '，';
                //}
                //textBoxFinalPrice.Text = str;
                timer.Stop();

                if (this.SelectedNumList.Count >= 10)//抽完了，
                {
                    MessageBox.Show("( ⊙ o ⊙ )！？没抽到的找张旻报仇吧！");
                    buttonStart.IsEnabled = false;
                    buttonStop.IsEnabled = false;
                    //numberGroupMain.TurnStop(1); //使滚轮停止到1处。
                }
                else
                {
                    buttonStart.IsEnabled = true;
                }
            }
        }

        //开始按钮点击
        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Start();
        }

        //停止按钮点击
        private void buttonStop_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            Stop();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) //绑定空格键盘命令
            {
                if (buttonStart.IsEnabled == true && buttonStop.IsEnabled == false)
                {
                    Start();
                    e.Handled = true;
                    return;
                }

                if (buttonStart.IsEnabled == false && buttonStop.IsEnabled == true)
                {
                    Stop();
                    e.Handled = true;
                    return;
                }
            }
        }

        private void Start()
        {
            buttonStart.IsEnabled = false;
            buttonStop.IsEnabled = true;
            numberGroupMain.TurnStart();
            //textBoxFinalPrice.Text = "";
            timer.Stop();
        }

        private void Stop()
        {
            if (this.SelectedNumList.Count >= Count)
            {
                MessageBox.Show("还抽啥？直接发奖呗！");
                return;
            }

            buttonStop.IsEnabled = false;
            bool flag = true;
            while (flag)
            {
                finalValue = random.Next(1, 16);
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
