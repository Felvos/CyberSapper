using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Threading;

namespace Mitya_plus_Kashkin_ravno_Saper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int seconds = 0;
        public DispatcherTimer global_timer = new DispatcherTimer();


        public void restartgame()
        {
            var a = new MainWindow();
            a.Show();
            this.Close();
        }
        
        public MainWindow()
        {
            InitializeComponent();
            Field newMap = new Field(Input_info.height_field, Input_info.width_field, Input_info.count_mines, this);
            newMap.init_grid( this);

            global_timer.Tick += Global_timerOnTick;
            global_timer.Interval = new TimeSpan(0, 0, 1);
            global_timer.Start();
            
        }

        private void Global_timerOnTick(object? sender, EventArgs e)
        {
            seconds++;
            timer.Content = $"Время:  {seconds}";
        }


        private void Refresh_OnClick(object sender, RoutedEventArgs e)
        {
            restartgame();
        }

        private void Settings_OnClick(object sender, RoutedEventArgs e)
        {
            var b = new Settings();
            if (b.ShowDialog() == true)
            {
                restartgame();
            }
        }
    }
}