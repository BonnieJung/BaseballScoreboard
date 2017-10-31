using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/**********************************************
 * Author    : Bonnie(Bo-Hye) Jung
 * Course    : INFO5150
 * Project   : Project#1 - MLB Scoreboard
 ***********************************************/

namespace Project1_MLBScoreBoard
{
    public class MlbInfo 
    {
        //public mlbPerYear[] mlbpy { get; set; }

        //public class mlbPerYear : IEnumerable
        //{
            //public int year { get; set; }
            public mlbPerDay[] mlbpd { get; set; }

            public IEnumerator GetEnumerator()
            {
                foreach (mlbPerDay d in mlbpd)
                {
                    yield return d;
                }
            }
        //}

        public class mlbPerDay
        {
            public string subject { get; set; }
            public string copyright { get; set; }
            public Data data { get; set; }
        }

        public class Data
        {
            public Games games { get; set; }
        }

        public class Games
        {
            public string next_day_date { get; set; }
            public DateTime modified_date { get; set; }
            public string month { get; set; }
            public string year { get; set; }
            public object game { get; set; }
            public string day { get; set; }
        }
    }
}
