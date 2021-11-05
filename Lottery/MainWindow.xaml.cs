using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
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

        private string finalValue;

        private readonly Random _random = new Random();

        private readonly List<string> _selectedNumList = new List<string>();
        private readonly DispatcherTimer _timer = new DispatcherTimer();

        public TextBox[] FirstPrice;
        public TextBox[] SecondPrice;
        public TextBox[] ThirdPrice;

        public MainWindow()
        {
            InitializeComponent();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += timer_Tick;

            // 创建文本框控件
            int fontsize = 24;
            int minWidth = 100;
            int boxHeight = 40;
            FirstPrice = new TextBox[Common.ThePrizeNumber.FirstRank];
            SecondPrice = new TextBox[Common.ThePrizeNumber.SecondRank];
            ThirdPrice = new TextBox[Common.ThePrizeNumber.ThirdRank];
            for (int i = 0; i < Common.ThePrizeNumber.FirstRank; i++)
            {
                FirstPrice[i] = new TextBox();
                FirstPrice[i].FontSize = fontsize;
                FirstPrice[i].MinWidth = minWidth;
                FirstPrice[i].HorizontalAlignment = HorizontalAlignment.Center;
                FirstPrice[i].VerticalAlignment = VerticalAlignment.Center;
                FirstPrice[i].Height = boxHeight;
                labelFirstPrize.Children.Add(FirstPrice[i]);
            }
            for (int i = 0; i < Common.ThePrizeNumber.SecondRank; i++)
            {
                SecondPrice[i] = new TextBox();
                SecondPrice[i].FontSize = fontsize;
                SecondPrice[i].MinWidth = minWidth;
                SecondPrice[i].HorizontalAlignment = HorizontalAlignment.Center;
                SecondPrice[i].VerticalAlignment = VerticalAlignment.Center;
                SecondPrice[i].Height = boxHeight;
                labelSecondPrice.Children.Add(SecondPrice[i]);
            }
            for (int i = 0; i < Common.ThePrizeNumber.ThirdRank; i++)
            {
                ThirdPrice[i] = new TextBox();
                ThirdPrice[i].FontSize = fontsize;
                ThirdPrice[i].MinWidth = minWidth;
                ThirdPrice[i].HorizontalAlignment = HorizontalAlignment.Center;
                ThirdPrice[i].VerticalAlignment = VerticalAlignment.Center;
                ThirdPrice[i].Height = boxHeight;
                labelThirdPrice.Children.Add(ThirdPrice[i]);
            }
        }

        //监视转动是否停止，如果停止，显示价格
        private void timer_Tick(object sender, EventArgs e)
        {
            if (numberGroupMain.IsStoped())
            {
                //textBoxFinalPrice.Text = "￥" + finalValue.ToString("F2");//显示最终金额

                #region 按照一、二、三、四等奖显示

                if (_selectedNumList.Count <= Common.ThePrizeNumber.ThirdRank)
                    ThirdPrice[_selectedNumList.Count - 1].Text = _selectedNumList[_selectedNumList.Count - 1].ToString();
                else
                    if (_selectedNumList.Count <= Common.ThePrizeNumber.ThirdRank + Common.ThePrizeNumber.SecondRank)
                    SecondPrice[_selectedNumList.Count - 1 - Common.ThePrizeNumber.ThirdRank].Text = _selectedNumList[_selectedNumList.Count - 1].ToString();
                else
                    if (_selectedNumList.Count <= Common.ThePrizeNumber.ThirdRank + Common.ThePrizeNumber.SecondRank + Common.ThePrizeNumber.FirstRank)
                        FirstPrice[_selectedNumList.Count - 1 - Common.ThePrizeNumber.ThirdRank - Common.ThePrizeNumber.SecondRank].Text =
                            _selectedNumList[_selectedNumList.Count - 1].ToString();

                
                //switch (_selectedNumList.Count)
                //{
                //    case 1: //三等奖3个中的第1个
                //        textBoxThirdPrice1.Text = _selectedNumList[0].ToString();
                //        break;
                //    case 2: //三等奖3个中的第2个
                //        textBoxThirdPrice2.Text = _selectedNumList[1].ToString();
                //        break;
                //    case 3: //三等奖3个中的第3个
                //        textBoxThirdPrice3.Text = _selectedNumList[2].ToString();
                //        break;
                //    case 4: //二等奖2个中的第1个
                //        textBoxSecondPrice1.Text = _selectedNumList[3].ToString();
                //        break;
                //    case 5: //二等奖2个中的第2个
                //        textBoxSecondPrice2.Text = _selectedNumList[4].ToString();
                //        break;
                //    case 6: //一等奖1个
                //        textBoxFirstPrice.Text = _selectedNumList[5].ToString();
                //        break;
                //}

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

                if (_selectedNumList.Count >= Common.ThePrizeNumber.ThirdRank + Common.ThePrizeNumber.SecondRank + Common.ThePrizeNumber.FirstRank) //抽完了，
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
            if (_selectedNumList.Count >= Common.AvailableName.Length)
            {
                MessageBox.Show("还抽啥？直接发奖呗！");
                return;
            }

            buttonStop.IsEnabled = false;
            var flag = true;
            if (_selectedNumList.Count > Common.NameList.Length)
            {
                numberGroupMain.TurnStop("我是空白!");
                MessageBox.Show("名单人员已经全部抽完啦!");
                return;
            }
            while (flag)
            {
                finalValue = Common.AvailableName[new Random().Next(0, Common.AvailableName.Length)];
                if (_selectedNumList.Contains(finalValue) == false && finalValue != "*") //不在已抽中序列里面
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