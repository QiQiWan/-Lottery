using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace Lottery
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        ///     抽奖人数
        /// </summary>
        public int Count = 17;

        private int finalValue;

        private readonly Random _random = new Random();

        private readonly List<int> _selectedNumList = new List<int>();
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public MainWindow()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;
        }

        //监视转动是否停止，如果停止，显示价格
        private void timer_Tick(object sender, EventArgs e)
        {
            if (numberGroupMain.IsStoped())
            {
                //textBoxFinalPrice.Text = "￥" + finalValue.ToString("F2");//显示最终金额

                #region 按照一、二、三、四等奖显示

                switch (_selectedNumList.Count)
                {
                    case 1: //三等奖3个中的第1个
                        textBoxThirdPrice1.Text = _selectedNumList[0].ToString();
                        break;
                    case 2: //三等奖3个中的第2个
                        textBoxThirdPrice2.Text = _selectedNumList[1].ToString();
                        break;
                    case 3: //三等奖3个中的第3个
                        textBoxThirdPrice3.Text = _selectedNumList[2].ToString();
                        break;
                    case 4: //二等奖2个中的第1个
                        textBoxSecondPrice1.Text = _selectedNumList[3].ToString();
                        break;
                    case 5: //二等奖2个中的第2个
                        textBoxSecondPrice2.Text = _selectedNumList[4].ToString();
                        break;
                    case 6: //一等奖1个
                        textBoxFirstPrice.Text = _selectedNumList[5].ToString();
                        break;
                        //case 7: //四等奖4个中的第1个
                        //    this.textBoxFourthPrice1.Text = this.SelectedNumList[6].ToString();
                        //    break;
                        //case 8: //四等奖4个中的第2个
                        //    this.textBoxFourthPrice2.Text = this.SelectedNumList[7].ToString();
                        //    break;
                        //case 9: //四等奖4个中的第3个
                        //    this.textBoxFourthPrice3.Text = this.SelectedNumList[8].ToString();
                        //    break;
                }

                #endregion

                //string str = "";
                //foreach (var item in this.SelectedNumList)
                //{
                //    str += item.ToString() + '，';
                //}
                //textBoxFinalPrice.Text = str;
                _timer.Stop();

                #region 把结果写入Txt.文件

                var startpath = Directory.GetCurrentDirectory(); //获取exe所在目录
                var sw = File.AppendText(startpath + "\\抽奖结果.txt");
                var w = DateTime.Now + ":             " + finalValue;
                sw.WriteLine(w);
                sw.Close();

                #endregion

                if (_selectedNumList.Count >= 6) //抽完了，
                {
                    MessageBox.Show("( ⊙ o ⊙ )！？没中的晚上多吃点！");
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
                if (buttonStart.IsEnabled && buttonStop.IsEnabled == false)
                {
                    Start();
                    e.Handled = true;
                    return;
                }

                if (buttonStart.IsEnabled == false && buttonStop.IsEnabled)
                {
                    Stop();
                    e.Handled = true;
                }
            }
        }

        private void Start()
        {
            buttonStart.IsEnabled = false;
            buttonStop.IsEnabled = true;
            numberGroupMain.TurnStart();
            //textBoxFinalPrice.Text = "";
            _timer.Stop();
        }

        private void Stop()
        {
            if (_selectedNumList.Count >= Count)
            {
                MessageBox.Show("还抽啥？直接发奖呗！");
                return;
            }

            buttonStop.IsEnabled = false;
            var flag = true;
            while (flag)
            {
                
                finalValue = _random.Next(1, 18);
                if (_selectedNumList.Contains(finalValue) == false) //不在已抽中序列里面
                {
                    _selectedNumList.Add(finalValue);
                    flag = false;
                    break;
                }
            }
            numberGroupMain.TurnStop(finalValue); //使数字组停止
            _timer.Start();
        }
    }
}