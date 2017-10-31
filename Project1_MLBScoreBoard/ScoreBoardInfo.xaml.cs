using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using System.Windows.Shapes;

/**********************************************
 * Author    : Bonnie(Bo-Hye) Jung
 * Course    : INFO5150
 * Project   : Project#1 - MLB Scoreboard
 ***********************************************/

namespace Project1_MLBScoreBoard
{
    /// <summary>
    /// Interaction logic for ScoreBoardInfo.xaml
    /// </summary>
    public partial class ScoreBoardInfo : Window
    {
       // DateTime selectedD = MainWindow.selectedDate;
        MlbInfo.mlbPerDay mlbInfoOfselectedDay = new MlbInfo.mlbPerDay(); //temp it should be coming from mainwindow
        private const string YEAR_INFO_FILE = "YearInfo.txt";

        public ScoreBoardInfo(MlbInfo.mlbPerDay mlb)
        {
            mlbInfoOfselectedDay = mlb;
            InitializeComponent();
            var games = mlb.data.games.game as IEnumerable<object>;

            //first one with general information
            TabItem tabG = new TabItem();
            tabG.Name = "tabGeneral";
            tabG.Header = "Games General Info";
            ScrollViewer scrV0 = new ScrollViewer();
            scrV0.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrV0.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            StackPanel sp = new StackPanel();
            sp.Margin = new Thickness(5, 5, 5, 5);

            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(mlb))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(mlb);
                TextBlock tb = new TextBlock();
                tb.TextWrapping = TextWrapping.Wrap;
                tb.Text = string.Format(" {0, -20} : {1}", name, value.ToString()); 
                sp.Children.Add(tb);
            }
            scrV0.Content = sp;
            tabG.Content = scrV0;
            tabControl1.Items.Add(tabG);


            //each game
            int gameNum = 1;
            foreach(var game in games)
            {
                TabItem tab = new TabItem();
                tab.Name = "tab" + gameNum;
                tab.Header = "Game " + gameNum;
                ScrollViewer scrV = new ScrollViewer();
                scrV.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrV.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                StackPanel s1 = new StackPanel();
                s1.Margin = new Thickness(5, 5, 5, 5);

                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(game))
                {
                    string name = descriptor.Name;
                    object value = descriptor.GetValue(game);
                    if (value != null)
                    {
                        TextBlock tb = new TextBlock();
                        tb.TextWrapping = TextWrapping.Wrap;
                        tb.Text = string.Format(" {0, -30}: \t{1}", name, value.ToString()); 
                        s1.Children.Add(tb);
                    }
                }
                scrV.Content = s1;
                tab.Content = scrV;
                tabControl1.Items.Add(tab);
                
                gameNum++;
            }
        }//end of ScoreBoardInfo

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            saveYear();
        }

        private void saveYear()
        {
            string yearInfo = mlbInfoOfselectedDay.data.games.year;
            using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + YEAR_INFO_FILE))
            {
                writer.Write(yearInfo);
            }
        }
    }
}
