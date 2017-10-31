using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

/**********************************************
 * Author    : Bonnie(Bo-Hye) Jung
 * Course    : INFO5150
 * Project   : Project#1 - MLB Scoreboard
 ***********************************************/


namespace Project1_MLBScoreBoard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
   
    public partial class MainWindow : Window
    {
        public int yearInfo = 0;
        public bool isYearExist = false;
        public static DateTime selectedDate = new DateTime();
        private const string MLB_FILE_PER_YEAR = ""; //"allMLB.json";
        private const string YEAR_INFO_FILE = "YearInfo.txt";
        private const int START_YEAR = 2007;
        private const int CURRENT_YEAR = 2017;
        public List<MlbInfo.mlbPerDay> daysCollectionOneYear = new List<MlbInfo.mlbPerDay>(); //only for 1 year

        public MainWindow()
        {
            InitializeComponent();
            //load year info to combobox
            for (int i = START_YEAR; i < DateTime.Now.Year + 1; ++i)
                cb_years.Items.Add(i); // 2007~ current year(2017)

            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + YEAR_INFO_FILE))
            {
                isYearExist = true;
                try
                {
                    StreamReader sr_year = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + YEAR_INFO_FILE);
                    string stringFromFile = sr_year.ReadLine();
                    if (stringFromFile != null)
                    {
                        yearInfo = int.Parse(stringFromFile);
                        if (yearInfo != 0)
                        {
                            cb_years.SelectedIndex = yearInfo - START_YEAR;
                        }
                    }
                    sr_year.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("A problem when finding the year");
                }
            }

            if (yearInfo >= START_YEAR && yearInfo < CURRENT_YEAR)  //when user searching for another year without closing
            {
                string filename = "mlb" + yearInfo + ".json";
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + filename))
                {
                    try
                    {
                        StreamReader sr = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + filename);
                        string yearsJson = sr.ReadToEnd();
                        /////////////////////////////////////////////////////
                        //             Deserialize(json->object)           //
                        ///////////////////////////////////////////////////// 
                        List<MlbInfo.mlbPerDay> MLBsInAYear = JsonConvert.DeserializeObject<List<MlbInfo.mlbPerDay>>(yearsJson);
                        sr.Close();

                        daysCollectionOneYear = MLBsInAYear;
                        blakcOut();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Unable to read " + filename);
                    }
                }
            }//end of yearInfo filter
            else
            {
                isYearExist = false;
            }
        }

        public DateTime MyProperty
        {
            get { return (DateTime)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("MyProperty", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(new DateTime(2007, 01, 01)));



        //combobox for year
        private void cb_years_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            tb_loadingInfo.Text = "Loading...";
            //clear the previous memory
            daysCollectionOneYear.Clear(); 
             
            //you've got the year. time to enable the calendar with available date
            yearInfo = cb_years.SelectedIndex + START_YEAR;
            datepicker_.SetValue(MyPropertyProperty, new DateTime(yearInfo, 01, 01));
            string filename = "mlb" + yearInfo + ".json";

            //we already have json loaded
            bool isFound = false;
            //if we have year json(except for this year)
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + filename) && filename != "mlb"+CURRENT_YEAR+".json")
            {
                using (StreamReader sr1 = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + filename))
                {
                    string stringWeLookFor = "master_scoreboard_mlb_" + yearInfo;
                    var jsonContentThisFile = sr1.ReadToEnd();
                    if (jsonContentThisFile.Contains(stringWeLookFor))
                        isFound = true;
                    
                    /////////////////////////////////////////////////////
                    //             Deserialize(json->object)           //
                    ///////////////////////////////////////////////////// 
                    if (isFound)
                    {
                        string wholeJson = sr1.ReadLine();
                        //make it into collection(deserialize to list)
                        List<MlbInfo.mlbPerDay> MLBsInAYear2 = JsonConvert.DeserializeObject<List<MlbInfo.mlbPerDay>>(jsonContentThisFile);
                        daysCollectionOneYear = MLBsInAYear2;
                        blakcOut();
                    }//ifFound
                    else//if the year is not found
                    {
                        if (!isYearExist)
                        {
                            List<string> urls = new List<string>();
                            //datetime container
                            List<DateTime> datesInRangeL = getDatesInRangeL(yearInfo);

                            foreach (DateTime dt in datesInRangeL)
                                urls.Add(urlMaker(dt));

                            List<string> jsonsL = new List<string>();
                            using (WebClient client2 = new WebClient())
                            {
                                foreach (string u in urls)
                                {
                                    try
                                    {
                                        string jsonT = client2.DownloadString(u);
                                        jsonsL.Add(jsonT);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }//end of using webclient

                            List<string> jsonExeptionCaughtWhenDeserialization = new List<string>();
                            foreach (var j in jsonsL)
                            {
                                try
                                {
                                    MlbInfo.mlbPerDay mlbD = JsonConvert.DeserializeObject<MlbInfo.mlbPerDay>(j);
                                    daysCollectionOneYear.Add(mlbD);
                                }
                                catch (Exception ex)
                                {
                                    jsonExeptionCaughtWhenDeserialization.Add(j);
                                }
                            }//end of foreach to add daysCollection list
                        }
                    }//end of else
                }//end of isFound
            }//if file exists
            //file not existent anyways you have to make it
            else
            {
                isYearExist = false;
                if (!isYearExist)
                {
                    List<string> urls = new List<string>();
                    //datetime container
                    List<DateTime> datesInRangeL = getDatesInRangeL(yearInfo);

                    foreach (DateTime dt in datesInRangeL)
                        urls.Add(urlMaker(dt));

                    List<string> jsonsL = new List<string>();
                    using (WebClient client2 = new WebClient())
                    {
                        foreach (string u in urls)
                        {
                            try
                            {
                                string jsonT = client2.DownloadString(u);
                                jsonsL.Add(jsonT);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }

                    List<string> jsonExeptionCaughtWhenDeserialization = new List<string>();
                    foreach (var j in jsonsL)
                    {
                        try
                        {
                            MlbInfo.mlbPerDay mlbD = JsonConvert.DeserializeObject<MlbInfo.mlbPerDay>(j);
                            daysCollectionOneYear.Add(mlbD);
                        }
                        catch (Exception ex)
                        {
                            jsonExeptionCaughtWhenDeserialization.Add(j);
                        }
                    }
                }
            }//end of else

            
            MlbInfo thisYear = new MlbInfo();
            thisYear.mlbpd = daysCollectionOneYear.ToArray();
            saveFile();
            blakcOut();
            DateTime startDate = new DateTime(yearInfo, 01, 01);
            datepicker_.DisplayDateStart = startDate;
            datepicker_.DisplayDate = startDate;
            datepicker_.DisplayDateEnd = new DateTime(yearInfo, 12, 31);
            //if (yearInfo == CURRENT_YEAR)
            //    datepicker_.DisplayDate = DateTime.Now; 
            //else
            //    datepicker_.DisplayDateEnd = new DateTime(yearInfo, 12, 31);
            //finally make calendar available
            datepicker_.IsEnabled = true;
            tb_loadingInfo.Text = "Data for " + yearInfo + " is loaded!";
        }

        public void saveFile()
        {
            if (daysCollectionOneYear.Count > 0)
            {
                string jsonForTotal = JsonConvert.SerializeObject(daysCollectionOneYear);
                string filename2 = "mlb" + yearInfo + ".json";

                using (StreamWriter output2 = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + filename2))
                {
                    output2.WriteLine(jsonForTotal);
                }
            }
        }

        private void bt_confirm_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Confirm that you want this date?", "Confirmation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                selectedDate = (DateTime)datepicker_.SelectedDate;
                MlbInfo.mlbPerDay dayInfo = new MlbInfo.mlbPerDay();

                //get the day's info
                foreach (var d in daysCollectionOneYear)
                {
                    if(int.Parse(d.data.games.month) == selectedDate.Month && int.Parse(d.data.games.day)== selectedDate.Day)
                    {
                        dayInfo = d;
                        break;
                    }
                }
                //show the information window
                ScoreBoardInfo sBI = new ScoreBoardInfo(dayInfo);
                sBI.Show();
                saveYear();
            }
        }//end of confirm click

        private void saveYear()
        {
            yearInfo = cb_years.SelectedIndex + START_YEAR;
            using (StreamWriter writer = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + YEAR_INFO_FILE))
            {
                writer.Write(yearInfo);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            saveYear();
        }
        
        private void datepicker__SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            bt_confirm.IsEnabled = true;
        }

        #region helper

        static private List<DateTime> getDatesInRangeL(int yearInfo)
        {
            DateTime startingDate = new DateTime(yearInfo, 01, 01);
            DateTime endingDate = new DateTime(yearInfo, 12, 31);
            List<DateTime> datesInRangeL = new List<DateTime>();
            for (DateTime date = startingDate; date <= endingDate; date = date.AddDays(1))
                datesInRangeL.Add(date);
            return datesInRangeL;
        }

        //helper for getting everyday in selected year
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
                yield return day;
        }

        static private string urlMaker(DateTime dt)
        {
            string iY = dt.Year.ToString();
            string iM = dt.Month.ToString("00");
            string iD = dt.Day.ToString("00");
            string url = @"http://gd2.mlb.com/components/game/mlb/year_" + iY + "/month_" + iM + "/day_" + iD + "/master_scoreboard.json";
            return url;
        }
        #endregion

        private void blakcOut()
        {
            if (daysCollectionOneYear.Count > 0)
            {
                List<MlbInfo.mlbPerDay> dataGameDay = new List<MlbInfo.mlbPerDay>();
                List<MlbInfo.mlbPerDay> dataNoGameDay = new List<MlbInfo.mlbPerDay>();
                foreach (var mlb in daysCollectionOneYear)
                {
                    try
                    {
                        if (mlb.data.games.game == null)
                            dataNoGameDay.Add(mlb);
                        else  //how do you know the game day??
                            dataGameDay.Add(mlb);
                    }
                    catch (Exception ex)
                    {
                    }
                }


                DateTime end = new DateTime(yearInfo, 01, 01);
                for (DateTime st = new DateTime(yearInfo, 12, 31); st >= end;)
                {
                    bool isGameDay = false;
                    //foreach (var d2 in dataGameDay)
                    for (int i = dataGameDay.Count - 1; i >= 0; --i)
                    {
                        DateTime date2 = new DateTime(yearInfo, int.Parse(dataGameDay[i].data.games.month), int.Parse(dataGameDay[i].data.games.day));
                        if (st == date2)//found in GameDay
                        {
                            dataGameDay.RemoveAt(i);
                            isGameDay = true;
                            break;
                        }
                        else if (st > date2)
                            break;
                    }
                    if (!isGameDay)//if not found, i will add that day in the blackoutdates
                    {
                        CalendarDateRange range = new CalendarDateRange(st, st);
                        datepicker_.BlackoutDates.Add(range);
                    }
                    if (st.Day > 1)
                        st = new DateTime(yearInfo, st.Month, st.Day - 1);
                    else if (st.Month > 1)
                        st = new DateTime(yearInfo, st.Month - 1, DateTime.DaysInMonth(yearInfo, st.Month - 1));
                    else
                        break;
                }
            }
        }//end of blackOut
    }
}
