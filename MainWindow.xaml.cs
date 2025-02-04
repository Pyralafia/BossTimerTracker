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
        TextBlock[] playerDeathDisplayTxtBl;
        Label[] playerDeathDisplayLbl;
        TimeSpan tryTime;
        int nbPlayer = 1;
        int[] deathPlayer = {0, 0, 0, 0};
        bool first = true;

        public MainWindow()
        {
            InitializeComponent();

            HotkeyManager.Current.AddOrReplace("AddDeathP1", new KeyGesture(Key.NumPad1, ModifierKeys.Alt), AddDeathP1);
            HotkeyManager.Current.AddOrReplace("AddDeathP2", new KeyGesture(Key.NumPad2, ModifierKeys.Alt), AddDeathP2);
            HotkeyManager.Current.AddOrReplace("AddDeathP3", new KeyGesture(Key.NumPad3, ModifierKeys.Alt), AddDeathP3);
            HotkeyManager.Current.AddOrReplace("AddDeathP4", new KeyGesture(Key.NumPad4, ModifierKeys.Alt), AddDeathP4);
            HotkeyManager.Current.AddOrReplace("CaptureTimer", new KeyGesture(Key.NumPad5, ModifierKeys.Alt), CaptureTimer);
            HotkeyManager.Current.AddOrReplace("CaptureTimer2", new KeyGesture(Key.NumPad9), CaptureTimer);
            
            timeListbx.ItemsSource = visualList;
            playerDeathDisplayTxtBl = new TextBlock[4] { tryTxtBl1, tryTxtBl2, tryTxtBl3, tryTxtBl4 };
            playerDeathDisplayLbl = new Label[4] { p1Lbl, p2Lbl, p3Lbl, p4Lbl };
            showPlayerDeathTxt();
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
                tryTime = (time - timeList.Last()[0]) * speedSlider.Value;
                string tryMin = tryTime.Minutes < 10 ? $"0{tryTime.Minutes}" : $"{tryTime.Minutes}";
                string trysec = tryTime.Seconds < 10 ? $"0{tryTime.Seconds}" : $"{tryTime.Seconds}";

                timeList.Last()[1] = time;
                timeList.Last()[2] = tryTime;
                visualList[visualList.Count - 1] += $"{min}min {sec} -> {tryMin}min {trysec}";

                timeListbx.Items.Refresh();
                first = true;
            }
        }

        private void AddDeathP1(object sender, HotkeyEventArgs e)
        {
            deathPlayer[0]++;
            tryTxtBl1.Text = deathPlayer[0].ToString();
        }
        private void AddDeathP2(object sender, HotkeyEventArgs e)
        {
            deathPlayer[1]++;
            tryTxtBl2.Text = deathPlayer[1].ToString();
        }
        private void AddDeathP3(object sender, HotkeyEventArgs e)
        {
            deathPlayer[2]++;
            tryTxtBl3.Text = deathPlayer[2].ToString();
        }
        private void AddDeathP4(object sender, HotkeyEventArgs e)
        {
            deathPlayer[3]++;
            tryTxtBl4.Text = deathPlayer[3].ToString();
        }

        private void BossBtn_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan totalTime = TimeSpan.Zero;

            for (int i = 0; i < timeList.Count; i++)
            {
                totalTime += timeList[i][2];
            }

            TimeTxtBl.Text = $"{totalTime.Hours}h {totalTime.Minutes}min {totalTime.Seconds}";
            TimeDownTxtBl.Text = $"{totalTime.Minutes}min {totalTime.Seconds}";
            tryTotalTxtBl.Text = (timeList.Count - 1).ToString();

            //values reset
            for (int i = 0; i < 4; i++)
            {
                deathPlayer[i] = 0;
            }

            tryTxtBl1.Text = deathPlayer[0].ToString();
            tryTxtBl2.Text = deathPlayer[1].ToString();
            tryTxtBl3.Text = deathPlayer[2].ToString();
            tryTxtBl4.Text = deathPlayer[3].ToString();
            timeList.Clear();
            visualList.Clear();
        }

        private void showPlayerDeathTxt()
        {
            for (int i = 0; i < 4; i++)
            {
                if (i < nbPlayer)
                {
                    playerDeathDisplayTxtBl[i].Visibility = Visibility.Visible;
                    playerDeathDisplayLbl[i].Visibility = Visibility.Visible;
                }
                else
                {
                    playerDeathDisplayTxtBl[i].Visibility = Visibility.Hidden;
                    playerDeathDisplayLbl[i].Visibility = Visibility.Hidden;
                }
            }
        }

        private void nbPlayerSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            nbPlayer = (int)nbPlayerSlider.Value;

            if (IsLoaded)
            {
                showPlayerDeathTxt();
            }
        }
    }
}
