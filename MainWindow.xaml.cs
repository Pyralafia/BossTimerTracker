using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NHotkey;
using NHotkey.Wpf;

namespace BossTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static RoutedCommand CaptureTimerCmd = new RoutedCommand();


        List<TimeSpan[]> timeList = new List<TimeSpan[]>();
        List<string> visualList = new List<string>();
        bool first = true;

        public MainWindow()
        {
            InitializeComponent();

            HotkeyManager.Current.AddOrReplace("CaptureTimer", new KeyGesture(Key.NumPad5, ModifierKeys.Alt), CaptureTimer);
            
            timeListbx.ItemsSource = visualList;
        }

        private void CaptureTimer(object sender, HotkeyEventArgs e)
        {
            TimeSpan time = DateTime.Now.TimeOfDay; 
            string min = time.Minutes < 10 ? $"0{time.Minutes}" : $"{time.Minutes}";
            string sec = time.Seconds < 10 ? $"0{time.Seconds}" : $"{time.Seconds}";

            if (first)
            {
                TimeSpan[] newRow = new TimeSpan[4];


                newRow[0] = time;
                newRow[1] = TimeSpan.Zero;
                newRow[2] = TimeSpan.Zero;

                timeList.Add(newRow);
                visualList.Add($"{min}min {sec} - ");
                timeListbx.Items.Refresh();
                first = false;
            }
            else
            {
                TimeSpan tryTime = (time - timeList.Last()[0]) * speedSlider.Value;
                string tryMin = tryTime.Minutes < 10 ? $"0{tryTime.Minutes}" : $"{tryTime.Minutes}";
                string trysec = tryTime.Seconds < 10 ? $"0{tryTime.Seconds}" : $"{tryTime.Seconds}";

                timeList.Last()[1] = time;
                timeList.Last()[2] = tryTime;
                visualList[visualList.Count - 1] += $"{min}min {sec} -> {tryMin}min {trysec}";

                timeListbx.Items.Refresh();
                first = true;
            }
        }

        private void BossBtn_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan totalTime = TimeSpan.Zero;

            for (int i = 0; i < timeList.Count; i++)
            {
                totalTime += timeList[i][2];
            }

            TimeTxtBl.Text = $"{totalTime.Hours}h {totalTime.Minutes}min {totalTime.Seconds}";
            TryTxtBl.Text = (timeList.Count - 1).ToString();

            timeList.Clear();
            visualList.Clear();
        }
    }
}
