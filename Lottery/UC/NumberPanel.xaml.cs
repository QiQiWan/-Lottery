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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lottery.UC
{
    /// <summary>
    /// NumberPanel.xaml 的交互逻辑
    /// </summary>
    public partial class NumberPanel : UserControl
    {
        //基础周期(秒)
        private readonly int BASE_PERIOD = 15;

        /// <summary>
        /// 滚动速度（个/秒）
        /// </summary>
        private double _Speed = 1;
        /// <summary>
        /// 转动速度
        /// </summary>
        public double Speed
        {
            get
            {
                return _Speed;
            }
            set
            {
                _Speed = value;
            }
        }

        Storyboard storyboard1 = new Storyboard();
        Storyboard storyboard2 = new Storyboard();
        DoubleAnimation animation1 = new DoubleAnimation();
        DoubleAnimation animation2 = new DoubleAnimation();

        //字符列表
        private string[] numberList = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15" };

        public NumberPanel()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            foreach (string i in numberList)
            {
                NumberItem item = new NumberItem();
                item.NumberValue = i;
                stackPanelMain.Children.Add(item);
            }
            foreach (string i in numberList)
            {
                NumberItem item = new NumberItem();
                item.NumberValue = i;
                stackPanelMain.Children.Add(item);
            }
            foreach (string i in numberList)
            {
                NumberItem item = new NumberItem();
                item.NumberValue = i;
                stackPanelMain.Children.Add(item);
            }
        }

        //开始转动
        public void TurnStart()
        {
            if (Speed <= 0)
            {
                return;
            }

            animation1.From = -60;
            animation1.To = -120 * numberList.Length - 60;
            animation1.Duration = new Duration(TimeSpan.FromSeconds(BASE_PERIOD));
            animation1.SpeedRatio = Speed;
            animation1.RepeatBehavior = RepeatBehavior.Forever;
            Storyboard.SetTargetName(animation1, stackPanelMain.Name);
            Storyboard.SetTargetProperty(animation1, new PropertyPath(Canvas.TopProperty));
            storyboard1.Children.Add(animation1);


            storyboard1.Begin(this, true);
        }

        //使转动停止在某个数字上
        public void TurnStopAt(int number)
        {
            if (Speed <= 0)
            {
                return;
            }
            double fromTop = (double)stackPanelMain.GetValue(Canvas.TopProperty);
            int numIndex = 0;
            for (int i = 0; i < this.numberList.Length; i++)
            {
                if (number.ToString() == this.numberList[i])
                {
                    numIndex = i;
                }
            }

            double toTop = -120 * (numberList.Length + numIndex - 1) - 60; //一个数字高120，宽120
            double interval = fromTop - toTop;
            if (interval < 0)
            {
                toTop += -120 * numberList.Length;
            }
            if (fromTop - toTop > 120 * numberList.Length)
            {
                fromTop -= 120 * numberList.Length;
            }

            animation2.From = fromTop;
            animation2.To = toTop;
            double numberCount = (fromTop - toTop) / 120;
            double duration = BASE_PERIOD * numberCount / (numberList.Length);
            animation2.Duration = new Duration(TimeSpan.FromSeconds(duration));
            animation2.SpeedRatio = 4;
            animation2.DecelerationRatio = 1;
            Storyboard.SetTargetName(animation2, stackPanelMain.Name);
            Storyboard.SetTargetProperty(animation2, new PropertyPath(Canvas.TopProperty));
            storyboard2.Children.Add(animation2);

            storyboard1.Stop(this);

            storyboard2.Begin(this, true);
        }


        /// <summary>
        /// 判断转动是否停止
        /// </summary>
        public bool IsStopped()
        {
            bool isStopped = storyboard2.GetCurrentState(this) != ClockState.Active;
            return isStopped;
        }
    }
}
