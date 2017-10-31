using System;
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
    public class Game
    {
        public string game_type { get; set; }
        public string double_header_sw { get; set; }
        public string location { get; set; }
        public string away_time { get; set; }
        public string time { get; set; }
        public string home_time { get; set; }
        public string home_team_name { get; set; }
        public string description { get; set; }
        public string original_date { get; set; }
        public string home_team_city { get; set; }
        public string venue_id { get; set; }
        public string gameday_sw { get; set; }
        public string away_win { get; set; }
        public string home_games_back_wildcard { get; set; }
        public string away_team_id { get; set; }
        public string tz_hm_lg_gen { get; set; }
        public Status status { get; set; }
        public string home_loss { get; set; }
        public string home_games_back { get; set; }
        public string home_code { get; set; }
        public string away_sport_code { get; set; }
        public string home_win { get; set; }
        public string time_hm_lg { get; set; }
        public string away_name_abbrev { get; set; }
        public string league { get; set; }
        public string time_zone_aw_lg { get; set; }
        public string away_games_back { get; set; }
        public string home_file_code { get; set; }
        public string game_data_directory { get; set; }
        public string time_zone { get; set; }
        public string away_league_id { get; set; }
        public string home_team_id { get; set; }
        public string day { get; set; }
        public string time_aw_lg { get; set; }
        public string away_team_city { get; set; }
        public string tbd_flag { get; set; }
        public string tz_aw_lg_gen { get; set; }
        public string away_code { get; set; }
        public Game_Media game_media { get; set; }
        public string game_nbr { get; set; }
        public string time_date_aw_lg { get; set; }
        public string away_games_back_wildcard { get; set; }
        public string scheduled_innings { get; set; }
        public string venue_w_chan_loc { get; set; }
        public string first_pitch_et { get; set; }
        public string away_team_name { get; set; }
        public string time_date_hm_lg { get; set; }
        public string id { get; set; }
        public string home_name_abbrev { get; set; }
        public string tiebreaker_sw { get; set; }
        public string ampm { get; set; }
        public string home_division { get; set; }
        public string home_time_zone { get; set; }
        public string away_time_zone { get; set; }
        public string hm_lg_ampm { get; set; }
        public string home_sport_code { get; set; }
        public string time_date { get; set; }
        public string home_ampm { get; set; }
        public string game_pk { get; set; }
        public string venue { get; set; }
        public string home_league_id { get; set; }
        public string away_loss { get; set; }
        public string resume_date { get; set; }
        public string away_file_code { get; set; }
        public string aw_lg_ampm { get; set; }
        public string time_zone_hm_lg { get; set; }
        public string away_ampm { get; set; }
        public string gameday { get; set; }
        public string away_division { get; set; }
    }

    public class Status
    {
        public string reason { get; set; }
        public string ind { get; set; }
        public string status { get; set; }
        public string inning_state { get; set; }
        public string note { get; set; }
    }

    public class Game_Media
    {
    }

}
