using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace poker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int hand_out = 1;
        bool rais = false;
        private void Form1_Load(object sender, EventArgs e)
        {
            b_hand1.BackColor = Color.Cyan;
            txtmoney1.Text = "500";
           
        }
       
        private void button26_Click(object sender, EventArgs e)
        {
            try
            {
                meet_player[0] = true;
                meet_player[1] = true;
             int bet_min=int.Parse(txtbet1.Text);
             set_first(bet_min);
             if (hand_out < 4)
                 hand_out++;
             else
                 hand_out = 0;
             set_graphic();
             enable_player[1] =true;
             hand_out = 2;
             if (q_learning_player == hand_out)
                 qlearning();
              }
            catch 
            {
                MessageBox.Show("ENTER BB s bet"); 
            }
            
        }

        ///////////////////////////////////////////////////////////////////////////////
        int pot = 0;
        string[] cards_table = new string[5];
        int min_bet;
        ArrayList all_cards = new ArrayList();
        string[] cards_number = new string[14];
        string[] cards_type = new string[4];
        int round = 0, hand_poke = 0;
        string[,] cards = new string[5, 2];
        string[,] cards1 = new string[5, 4];
        string[,] cards2 = new string[5, 2];
        string[] position = new string[5];
        int[] bet = new int[5];
        int[] money = new int[5];
        bool[,] visible_button = new bool[5, 4];
        string[] winer_bet = new string[5];//txt
        string[,] game_play = new string[5, 4];//txt
        string winner_position;//txt
        int q_learning_player;//txt
        bool[] enable_player = new bool[5];
        bool[] meet_player = new bool[5];
        
        int rais_round_count, count_player = 5, rais_round_position;
        public void set_first(int bet1)
        {
            for (int i = 2; i <= 10; i++)
            {
                cards_number[i] = i.ToString();
            }
            
            cards_number[1] = "A";
            cards_number[11] = "J";
            cards_number[12] = "Q";
            cards_number[13] = "K";
            cards_type[0] = "D";
            cards_type[1] = "H";
            cards_type[2] = "S";
            cards_type[3] = "C";
            position[0] = "SB";
            position[1] = "BB";
            position[2] = "UDG";
            position[3] = "Middle";
            position[4] = "DB";
            Random r = new Random();
            q_learning_player =  r.Next(2, 4);
            min_bet = bet1;
            first_bet(bet1);
            put_first_card();
            set_table_card();
            if (hand_poke != 0)
                set_position();
            if (round == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    enable_player[i] = true;
                    visible_button[i, 0] = true;
                    visible_button[i, 1] = false;
                    visible_button[i, 2] = true;
                    visible_button[i, 3] = true;
                    if (position[i].ToString() == "BB")
                        visible_button[i, 1] = true;
                }
             }
          
        }

        ////////////////////////// qlearning //////////////////////////
      

        ///////////////////////////////////////////////////////////////
        public void set_position()
        {
            string k = position[4];
            for (int i = 4; 1 <= i; i--)
            {
                position[i] = position[i - 1];
            }
            position[0] = k;
        }
        public void put_first_card()
        {
            Random r = new Random();
            string[] c1 = new string[2];
            for (int i = 0; i < 5; i++)
            {
                c1 = set_card();
                cards[i, 0] = c1[0] + c1[1];
                cards1[i, 0] = c1[0];
                cards1[i, 1] = c1[1];
                c1 = set_card();
                cards[i, 1] = c1[0] + c1[1];
                cards1[i, 2] = c1[0];
                cards1[i, 3] = c1[1];
            }
          
        }
        public void set_table_card()
        {
          //  if (round == 1)
            string[] c1 = new string[2];
            c1 = set_card();
            cards2[0, 0] = c1[0];
            cards2[0, 1] = c1[1];
            cards_table[0] = c1[0] + c1[1];

            c1 = set_card();
            cards2[1, 0] = c1[0];
            cards2[1, 1] = c1[1];
            cards_table[1] = c1[0] + c1[1];

            c1 = set_card();
            cards2[2, 0] = c1[0];
            cards2[2, 1] = c1[1];
            cards_table[2] = c1[0] + c1[1];
            
          //  if (round == 2)
            c1 = set_card();
            cards2[3, 0] = c1[0];
            cards2[3, 1] = c1[1];
            cards_table[3] = c1[0] + c1[1];
            
           // if (round == 3)
            c1 = set_card();
            cards2[4, 0] = c1[0];
            cards2[4, 1] = c1[1];
            cards_table[4] = c1[0] + c1[1];
            
        }
        public string[] set_card()
        {
            bool flag = true;
            string card1 = "";
            string[] c1 = new string[2];
            Random r = new Random();
            while (flag)
            {
                flag = false;
                c1[0] = cards_number[r.Next(1, 13)];
                c1[1] = cards_type[r.Next(0, 3)];
                
                card1 = c1[0] + c1[1];
                for (int i = 0; i < all_cards.Count; i++)
                {
                    if (all_cards[i].ToString() == card1)
                    {
                        flag = true;
                    }
                }
            }
            all_cards.Add(card1);
            return c1;
        }
        public void first_bet(int bet1)
        {
            int bet2 = 0;
            for (int i = 0; i < 5; i++)
            {
                money[i] = 500;
                if (position[i] == "BB")
                {
                    money[i] = money[i] - bet1;
                    bet[i] = bet1;
                }
                else if (position[i] == "SB")
                {
                    bet2 = bet1 / 2;
                    money[i] = money[i] - bet2;
                    bet[i] = bet2;
                }
                else
                    bet[i] = 0;
             }
            pot = pot + bet1 + bet2;
          
        }
        //////////////////////////////////////////////////////////////////////////
        public void new_round()
        {
            for (int i = 0; i < 5; i++)
            {
               if (enable_player[i])
                {
                    visible_button[i, 0] = true;
                    visible_button[i, 1] = true;
                    visible_button[i, 2] = false;
                    visible_button[i, 3] = true;
                    meet_player[i] = false;
                }
               else
                   meet_player[i] = true;

            }
            round++;
            if(enable_player[0])
            hand_out = 0;
            if (enable_player[1])
                hand_out = 1;
            if (enable_player[2])
                hand_out = 2;
            if (enable_player[3])
                hand_out = 3;
            if (enable_player[4])
                hand_out = 4;
            set_graphic();
            if(round==4)
            {
                
                show_down();
            }
        }
        public void show_down()
        {
            int[] c1 =new int[5];
            for (int i = 0; i < 5; i++)
            {
                c1[i] = -1;
                if(enable_player[i])
                {
                    c1[i]=rull_show_down(i);
                } 
            }
            int temp = 0,index=0;
            for (int i = 0; i < 5; i++)
            {
                if (enable_player[i] && temp<c1[i] )
                {
                    temp = c1[i];
                    index = i;
                }
            }
            if(temp<0)
            {
                int t1 = 0,index1=-1;
                for (int i = 0; i < 5; i++)
                {
                    if(t1<high_card(i))
                    {
                        t1 = high_card(i);
                        index1 = i;
                    }
                    
                }
                set_winner_txt(index1);
            }
            else
            {
                set_winner_txt(index);
            }
        }
        public void set_winner_txt(int hand)
        {
            string ss = "";
            if (hand == 0)
            { p0.BackColor = Color.Gold; ss = "SB"; }
            if (hand == 1)
            { p1.BackColor = Color.Gold; ss = "BB"; }
            if (hand == 2)
            { p2.BackColor = Color.Gold; ss = "UDG"; }
            if (hand == 3)
            { p3.BackColor = Color.Gold; ss = "Middle"; }
            if (hand == 4)
            { p4.BackColor = Color.Gold; ss = "DB"; }
            winner_position = hand_out.ToString();
            winer_bet[0] = txtbet0.Text;
            winer_bet[1] = txtbet1.Text;
            winer_bet[2] = txtbet2.Text;
            winer_bet[3] = txtbet3.Text;
            winer_bet[4] = txtbet4.Text;
            MessageBox.Show("<<<<<<<<<<<<<<<<  winner are " + ss+" >>>>>>>>>>>>>>>>>>>>>>");
            StreamWriter re = new StreamWriter("result.txt",true);
            string hh="";
            if(hand==q_learning_player)
                hh="1";
            else
                hh="0";
            re.WriteLine(hh + "/" + q_learning_player.ToString() + "/" + game_play[q_learning_player, 0] + "/" + game_play[q_learning_player, 1] + "/" + game_play[q_learning_player, 2] + "/" + game_play[q_learning_player, 3] + "/" + winer_bet[q_learning_player] + "/" + cards[q_learning_player, 0] + "-" + cards[q_learning_player, 1]);
            re.Close();
        }
        public int rull_show_down(int hand_out)
        {
            if (royal_flash(hand_out))
                return 10;
            if (straight_flash(hand_out))
                return 9;
            if (four_of_kind(hand_out))
                return 8;
            if (full_house(hand_out))
                return 7;
            if(flush(hand_out))
                return 6;
            if (flush(hand_out))
                return 5;
            if (three_of_kind(hand_out))
                return 4;
            if (two_pair(hand_out))
                return 3;
            if (a_pair(hand_out))
                return 2;
            return -1;
        }
        public bool royal_flash(int hand_out)
        {
            if (cards1[hand_out, 0] == "A")
            {
                int[] card_ = new int[14];
                for (int i = 0; i < 14; i++)
                    card_[i] = 0;
                int color1 = 1, color2 = 1;
                if (cards1[hand_out, 1] == cards1[hand_out, 3])
                {
                    color1++;
                    color2++;
                }
                for (int i = 0; i < 5; i++)
                {
                    if (cards2[i, 1] == cards1[hand_out, 1])
                        color1++;
                    if (cards2[i, 1] == cards1[hand_out, 3])
                        color2++;
                }
                if (5 <= color1 || 5 <= color2)
                {
                    if (cards1[hand_out, 0] == "A")
                        card_[1] = 1;
                    else if (cards1[hand_out, 0] == "J")
                        card_[11] = 1;
                    else if (cards1[hand_out, 0] == "Q")
                        card_[12] = 1;
                    else if (cards1[hand_out, 0] == "K")
                        card_[13] = 1;
                    else
                        card_[int.Parse(cards1[hand_out, 0])] = 1;


                    if (cards1[hand_out, 2] == "A")
                        card_[1] = 1;
                    else if (cards1[hand_out, 2] == "J")
                        card_[11] = 1;
                    else if (cards1[hand_out, 2] == "Q")
                        card_[12] = 1;
                    else if (cards1[hand_out, 2] == "K")
                        card_[13] = 1;
                    else
                        card_[int.Parse(cards1[hand_out, 2])] = 1;

                    for (int i = 0; i < 5; i++)
                    {
                        if (cards2[i, 0] == "A")
                            card_[1] = 1;
                        else if (cards2[i, 0] == "J")
                            card_[11] = 1;
                        else if (cards2[i, 0] == "Q")
                            card_[12] = 1;
                        else if (cards2[i, 0] == "K")
                            card_[13] = 1;
                        else
                            card_[int.Parse(cards2[i, 0])] = 1;
                    }

                    for (int i = 0; i < 14; i++)
                    {
                        try
                        {
                            if (card_[i] == 1 & card_[i + 1] == 1 & card_[i + 2] == 1 & card_[i + 3] == 1 & card_[i + 4] == 1)
                                return true;
                        }
                        catch { }
                    }

                }
            }
            return false;
        }
        public bool straight_flash(int hand_out)
        {
            
            {
                int[] card_ = new int[14];
                for (int i = 0; i < 14; i++)
                    card_[i] = 0;
                int color1 = 1, color2 = 1;
                if (cards1[hand_out, 1] == cards1[hand_out, 3])
                {
                    color1++;
                    color2++;
                }
                for (int i = 0; i < 5; i++)
                {
                    if (cards2[i, 1] == cards1[hand_out, 1])
                        color1++;
                    if (cards2[i, 1] == cards1[hand_out, 3])
                        color2++;
                }
                if (5 <= color1 || 5 <= color2)
                {
                    if (cards1[hand_out, 0] == "A")
                        card_[1] = 1;
                    else if (cards1[hand_out, 0] == "J")
                        card_[11] = 1;
                    else if (cards1[hand_out, 0] == "Q")
                        card_[12] = 1;
                    else if (cards1[hand_out, 0] == "K")
                        card_[13] = 1;
                    else
                        card_[int.Parse(cards1[hand_out, 0])] = 1;


                    if (cards1[hand_out, 2] == "A")
                        card_[1] = 1;
                    else if (cards1[hand_out, 2] == "J")
                        card_[11] = 1;
                    else if (cards1[hand_out, 2] == "Q")
                        card_[12] = 1;
                    else if (cards1[hand_out, 2] == "K")
                        card_[13] = 1;
                    else
                        card_[int.Parse(cards1[hand_out, 2])] = 1;

                    for (int i = 0; i < 5; i++)
                    {
                        if (cards2[i, 0] == "A")
                            card_[1] = 1;
                        else if (cards2[i, 0] == "J")
                            card_[11] = 1;
                        else if (cards2[i, 0] == "Q")
                            card_[12] = 1;
                        else if (cards2[i, 0] == "K")
                            card_[13] = 1;
                        else
                            card_[int.Parse(cards2[i, 0])] = 1;
                    }

                    for (int i = 0; i < 14; i++)
                    {
                        try
                        {
                            if (card_[i] == 1 & card_[i + 1] == 1 & card_[i + 2] == 1 & card_[i + 3] == 1 & card_[i + 4] == 1)
                                return true;
                        }
                        catch { }
                    }

                }
            }
            return false;
        }
        public bool four_of_kind(int hand_out)
        {
            int[] card_ = new int[14];
            int count1 = 1,count2=1;
            if (cards1[hand_out, 0] == cards1[hand_out, 2])
            {
                count1++;
                count2++;
            }
            for (int i = 0; i < 5; i++)
            {
                if (cards2[i, 0] == cards1[hand_out, 0])
                    count1++;
                if (cards2[i, 0] == cards1[hand_out, 2])
                    count2++;
            }
            if (4 <= count1 || 4 <= count2)
                return true;
            return false;
        }
        public bool full_house(int hand_out)
        {
            int[] card_ = new int[14];
            int count1 = 1, count2 = 1;
            if (cards1[hand_out, 0] == cards1[hand_out, 2])
            {
                count1++;
                count2++;
            }
            for (int i = 0; i < 5; i++)
            {
                if (cards2[i, 0] == cards1[hand_out, 0])
                    count1++;
                if (cards2[i, 0] == cards1[hand_out, 2])
                    count2++;
            }
            if(3<=count1 & 3<=count2)
            {
                for (int i = 0; i < 5; i++)
                {
                    count1 = 0;
                    for (int j = 0; j < 5; j++)
                    {
                        if (cards2[i, 0] == cards2[j, 0])
                            count1++;
                        
                    }
                    if (2 <= count1)
                        return true;
                }
            }
            return false;
        }
        public bool flush(int hand_out)
        {
             int color1 = 1, color2 = 1;
                if (cards1[hand_out, 1] == cards1[hand_out, 3])
                {
                    color1++;
                    color2++;
                }
                for (int i = 0; i < 5; i++)
                {
                    if (cards2[i, 1] == cards1[hand_out, 1])
                        color1++;
                    if (cards2[i, 1] == cards1[hand_out, 3])
                        color2++;
                }
                if (5 <= color1 || 5 <= color2)
                    return true;
                return false;
        }
        public bool straight(int hand_out)
        {
              int[] card_ = new int[14];
                for (int i = 0; i < 14; i++)
                    card_[i] = 0;
             if (cards1[hand_out, 0] == "A")
                        card_[1] = 1;
                    else if (cards1[hand_out, 0] == "J")
                        card_[11] = 1;
                    else if (cards1[hand_out, 0] == "Q")
                        card_[12] = 1;
                    else if (cards1[hand_out, 0] == "K")
                        card_[13] = 1;
                    else
                        card_[int.Parse(cards1[hand_out, 0])] = 1;


                    if (cards1[hand_out, 2] == "A")
                        card_[1] = 1;
                    else if (cards1[hand_out, 2] == "J")
                        card_[11] = 1;
                    else if (cards1[hand_out, 2] == "Q")
                        card_[12] = 1;
                    else if (cards1[hand_out, 2] == "K")
                        card_[13] = 1;
                    else
                        card_[int.Parse(cards1[hand_out, 2])] = 1;

                    for (int i = 0; i < 5; i++)
                    {
                        if (cards2[i, 0] == "A")
                            card_[1] = 1;
                        else if (cards2[i, 0] == "J")
                            card_[11] = 1;
                        else if (cards2[i, 0] == "Q")
                            card_[12] = 1;
                        else if (cards2[i, 0] == "K")
                            card_[13] = 1;
                        else
                            card_[int.Parse(cards2[i, 0])] = 1;
                    }

                    for (int i = 0; i < 14; i++)
                    {
                        try
                        {
                            if (card_[i] == 1 & card_[i + 1] == 1 & card_[i + 2] == 1 & card_[i + 3] == 1 & card_[i + 4] == 1)
                                return true;
                        }
                        catch { }
                    }

                    return false;
        
        }
        public bool three_of_kind(int hand_out)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (cards2[i, 0] == cards2[j, 0])
                    {
                        if (cards2[i, 0] == cards1[hand_out, 0] || cards2[i, 0] == cards1[hand_out, 2])
                            return true;
                    }
                }
            }
            if (cards1[hand_out, 0]==cards1[hand_out, 2])
            {
                for (int i = 0; i < 5; i++)
                {
                    if (cards2[i, 0] == cards1[hand_out, 0])
                        return true;
                }
            }
            return false;
        }
        public bool two_pair(int hand_out)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (cards2[i, 0] == cards2[j, 0])
                    {
                        for (int i1 = 0; i1 < 5; i1++)
                          if (cards2[i1, 0] == cards1[hand_out, 0] || cards2[i1, 0] == cards1[hand_out, 2] )
                            return true;
                    }
                }
            }
            
            return false;
        }
        public bool a_pair(int hand_out)
        {
              for (int i1 = 0; i1 < 5; i1++)
                   if (cards2[i1, 0] == cards1[hand_out, 0] || cards2[i1, 0] == cards1[hand_out, 2])
                                return true;
            return false;
        }
        public int high_card(int hand_out)
        {
            if (cards1[hand_out, 0] == "A" || cards1[hand_out, 2] == "A")
                return 14;
            if (cards1[hand_out, 0] == "K" || cards1[hand_out, 2] == "K")
                return 13;
            if (cards1[hand_out, 0] == "Q" || cards1[hand_out, 2] == "Q")
                return 12;
            if (cards1[hand_out, 0] == "J" || cards1[hand_out, 2] == "J")
                return 11;
            if (int.Parse(cards1[hand_out, 0]) < int.Parse(cards1[hand_out, 2]))
                return int.Parse(cards1[hand_out, 2]);
            else
                return int.Parse(cards1[hand_out, 0]);
            return 1;
        }
        public void qlearning()
        {
            if (radioButton1.Checked == false)
            {
                Random r = new Random();
                while (true)
                {
                    int h = r.Next(0, 3);
                    int h2 = r.Next(0, 3);
                    int h3 = r.Next(0, 3);
                    if (visible_button[q_learning_player, h])
                    {
                       /* if (h == 0 && h2 == 2 & h3==2)
                        {
                            b_fold_Click(null, null);
                            break;
                        }*/
                        if (h == 1)
                        { b_check_Click(null, null); break; }
                        if (h == 2)
                        { b_call_Click(null, null); break; }
                        if (h == 3)
                        {
                            int p1 = r.Next(min_bet, 400);
                            if (p1 != min_bet)
                            {
                                txtbet0.Text = p1.ToString();
                                b_raise_Click(null, null);
                                break;
                            }

                        }
                    }
                }
            }
            else
            {
                StreamReader s1 = new StreamReader("result.txt");


            }

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////
      
        int pre_raise;
        public bool poker_player_action(string position1, string action, int money1)
        {
            if (action == "check")
            {
                for (int i = 0; i < 5; i++)
                {
                    if (position[i].ToString() == position1)
                    {
                        
                        game_play[i, round] = game_play[i, round] + "1-";
                    }
                }

            }
            if (action == "fold")
            {
                for (int i = 0; i < 5; i++)
                {
                    if (position[i].ToString() == position1)
                    {
                        enable_player[i] = false;
                        game_play[i, round] = game_play[i, round] + "0-";
                        count_player--;
                    }
                }

            }
            if (action == "call")
            {
                for (int i = 0; i < 5; i++)
                {
                    if (position[i].ToString() == position1)
                    {
                        game_play[i, round] = game_play[i, round] + "2-";
                        if (money[i]+bet[i] <= min_bet)
                        {
                            bet[i] = bet[i] + money[i];
                            money[i] = 0;
                        }
                        else
                        {
                            money[i] = money[i] + bet[i] - min_bet;
                            bet[i] =  min_bet;
                        }
                        pot = pot + bet[i];
                    }
                }
            }
            if (action == "raise")
            {
                rais_round_count = 0;
                rais = true;
                for (int i = 0; i < 5; i++)
                {
                    if (enable_player[i])
                    {
                        visible_button[i, 0] = true;
                        visible_button[i, 1] = false;
                        visible_button[i, 2] = true;
                        visible_button[i, 3] = true;
                    }
                    if (position[i].ToString() == position1)
                    {
                        rais_round_position = i;
                        pre_raise = i;
                        while (true)
                        {
                            pre_raise--;
                            if (pre_raise < 0)
                                pre_raise = 4;
                            if (enable_player[pre_raise])
                                break;

                        }
                            game_play[i, round] = game_play[i, round] + "3-";
                         if (money[i] + bet[i] < money1)
                        {
                            bet[i] = bet[i] + money[i];
                            money[i] = 0;
                        }
                        else
                        {
                            money[i] = money[i]+bet[i] - money1;
                            bet[i] =  money1;
                            
                        }

                        min_bet = money1;
                    }
                }
                return true;
            }
            return false;
        }
        private void b_fold_Click(object sender, EventArgs e)
        {
            string ss="";
            int index=0;
            Button b = (Button)sender;
            try
            {
                if (b.Name == "b_fold0" && hand_out == 0)
                {
                    poker_player_action("SB", "fold", 0);
                    ss = "SB";
                    enable_player[0] = false;
                    b_fold0.Enabled = false;
                    b_check0.Enabled = false;
                    b_call0.Enabled = false;
                    b_raise0.Enabled = false;
                    meet_player[0] = true;
                    if (rais)
                    {

                        if (pre_raise == 0)
                        {
                            set_new_round();
                            rais = false;

                        }
                    }


                }

                if (b.Name == "b_fold1" && hand_out == 1)
                {
                    poker_player_action("BB", "fold", 0);
                    ss = "BB";
                    enable_player[1] = false;
                    b_fold1.Enabled = false;
                    b_check1.Enabled = false;
                    b_call1.Enabled = false;
                    b_raise1.Enabled = false;
                    if (rais)
                    {
                        if (pre_raise == 1)
                        {
                            set_new_round();
                            rais = false;

                        }
                    }

                    meet_player[1] = true;

                }
              
                    if (b.Name == "b_fold2" && hand_out == 2)
                    {
                        poker_player_action("UDG", "fold", 0);
                        ss = "UDG";
                        enable_player[2] = false;
                        b_fold2.Enabled = false;
                        b_check2.Enabled = false;
                        b_call2.Enabled = false;
                        b_raise2.Enabled = false;
                        if (rais)
                        {
                            if (pre_raise == 2)
                            {
                                set_new_round();
                                rais = false;
                            }
                        }
                        meet_player[2] = true;
                    }
                    if (b.Name == "b_fold3" && hand_out == 3)
                    {
                        poker_player_action("Middle", "fold", 0);
                        ss = "Middle";
                        enable_player[3] = false;
                        b_fold3.Enabled = false;
                        b_check3.Enabled = false;
                        b_call3.Enabled = false;
                        b_raise3.Enabled = false;
                        if (rais)
                        {
                            if (pre_raise == 3)
                            {
                                set_new_round();
                                rais = false;
                            }
                        }
                        meet_player[3] = true;
                    }
                    if (b.Name == "b_fold4" && hand_out == 4)
                    {
                        poker_player_action("DB", "fold", 0);
                        ss = "DB";
                        enable_player[4] = false;
                        b_fold4.Enabled = false;
                        b_check4.Enabled = false;
                        b_call4.Enabled = false;
                        b_raise4.Enabled = false;
                        meet_player[4] = true;
                        if (rais)
                        {
                            if (pre_raise == 4)
                            {
                                set_new_round();
                                rais = false;
                            }
                        }
                        else
                        {
                            set_new_round();

                        }

                    }
                
            }
           catch
            {
                if (q_learning_player == 2)
                {
                    poker_player_action("UDG", "fold", 0);
                    ss = "UDG";
                    enable_player[2] = false;
                    b_fold2.Enabled = false;
                    b_check2.Enabled = false;
                    b_call2.Enabled = false;
                    b_raise2.Enabled = false;
                    if (rais)
                    {
                        if (pre_raise == 2)
                        {
                            set_new_round();
                            rais = false;
                        }
                    }
                    meet_player[2] = true;
                }
                if (q_learning_player == 3)
                {
                    poker_player_action("Middle", "fold", 0);
                    ss = "Middle";
                    enable_player[3] = false;
                    b_fold3.Enabled = false;
                    b_check3.Enabled = false;
                    b_call3.Enabled = false;
                    b_raise3.Enabled = false;
                    if (rais)
                    {
                        if (pre_raise == 3)
                        {
                            set_new_round();
                            rais = false;
                        }
                    }
                    meet_player[3] = true;
                }
                if (q_learning_player == 4)
                {
                    poker_player_action("DB", "fold", 0);
                    ss = "DB";
                    enable_player[4] = false;
                    b_fold4.Enabled = false;
                    b_check4.Enabled = false;
                    b_call4.Enabled = false;
                    b_raise4.Enabled = false;
                    meet_player[4] = true;
                    if (rais)
                    {
                        if (pre_raise == 4)
                        {
                            set_new_round();
                            rais = false;
                        }
                    }
                    else
                    {
                        set_new_round();

                    }

                }
            }
            

            if (winner())
                set_handout();
            else
            {
                set_graphic();


                set_winner_txt(hand_out);
                MessageBox.Show("winer is" + ss);
            }
            set_graphic();
        }
        private void b_check_Click(object sender, EventArgs e)
        {

            Button b = (Button)sender;
          try
            { 
            if (b.Name == "b_check0" && hand_out == 0)
            {
                poker_player_action("SB", "check", 0);
              
                if (rais)
                {
                    if (pre_raise == 0)
                    {
                        rais = false;
                        hand_out = 0;
                    }
                }
               
                    meet_player[0] = true;

            }

            if (b.Name == "b_check1" && hand_out == 1)
            {
                poker_player_action("BB", "check", 0);
              
                if (rais)
                {
                    if (pre_raise == 1)
                    {
                        rais = false;
                        hand_out = 1;
                    }
                }
               
                    meet_player[1] = true;

            }
           
                if (b.Name == "b_check2" && hand_out == 2)
                {
                    poker_player_action("UDG", "check", 0);

                    if (rais)
                    {
                        if (pre_raise == 2)
                        {
                            rais = false;
                            hand_out = 2;
                        }
                    }

                    meet_player[2] = true;

                }

                if (b.Name == "b_check3" && hand_out == 3)
                {
                    poker_player_action("Middle", "check", 0);

                    if (rais)
                    {
                        if (pre_raise == 3)
                        {
                            rais = false;
                            hand_out = 3;
                        }
                    }

                    meet_player[3] = true;

                }

                if (b.Name == "b_check4" && hand_out == 4)
                {
                    poker_player_action("DB", "check", 0);
                    meet_player[4] = true;

                    if (rais)
                    {
                        if (pre_raise == 4)
                        {
                            rais = false;
                            hand_out = 4;
                        }
                    }
                    else
                    {
                        set_new_round();

                    }

                }
            }
           catch
            {
                if (q_learning_player == 2)
                {
                    poker_player_action("UDG", "check", 0);

                    if (rais)
                    {
                        if (pre_raise == 2)
                        {
                            rais = false;
                            hand_out = 2;
                        }
                    }

                    meet_player[2] = true;

                }

                if (q_learning_player == 3)
                {
                    poker_player_action("Middle", "check", 0);

                    if (rais)
                    {
                        if (pre_raise == 3)
                        {
                            rais = false;
                            hand_out = 3;
                        }
                    }

                    meet_player[3] = true;

                }

                if (q_learning_player == 4)
                {
                    poker_player_action("DB", "check", 0);
                    meet_player[4] = true;

                    if (rais)
                    {
                        if (pre_raise == 4)
                        {
                            rais = false;
                            hand_out = 4;
                        }
                    }
                    else
                    {
                        set_new_round();

                    }

                }
            }

            if (winner())
                set_handout();
            set_graphic();
        }
        private void b_call_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
           try
            { 
            if (b.Name == "b_call0" && hand_out == 0)
            {
                poker_player_action("SB", "call", 0);

                if (rais)
                {
                    if (pre_raise == 0)
                    {
                        rais = false;
                        set_new_round();
                    }
                }
              
                    meet_player[0] = true;
            }

            if (b.Name == "b_call1" && hand_out == 1)
            {
                poker_player_action("BB", "call", 0);

                if (rais)
                {
                    if (pre_raise == 1)
                    {
                        rais = false;
                        set_new_round();
                    }
                }
            
                    meet_player[1] = true;

            }
           
                if (b.Name == "b_call2" && hand_out == 2)
                {
                    poker_player_action("UDG", "call", 0);

                    if (rais)
                    {
                        if (pre_raise == 2)
                        {
                            rais = false;
                            set_new_round();
                        }
                    }

                    meet_player[2] = true;
                }
                if (b.Name == "b_call3" && hand_out == 3)
                {
                    poker_player_action("Middle", "call", 0);

                    if (rais)
                    {
                        if (pre_raise == 3)
                        {
                            rais = false;
                            set_new_round();
                        }
                    }

                    meet_player[3] = true;
                }
                if (b.Name == "b_call4" && hand_out == 4)
                {
                    poker_player_action("DB", "call", 0);
                    meet_player[4] = true;
                    if (rais)
                    {
                        if (pre_raise == 4)
                        {
                            rais = false;
                            set_new_round();
                        }
                    }
                    else
                    {
                        set_new_round();

                    }




                }
            }
            catch
            {
                if (q_learning_player== 2)
                {
                    poker_player_action("UDG", "call", 0);

                    if (rais)
                    {
                        if (pre_raise == 2)
                        {
                            rais = false;
                            set_new_round();
                        }
                    }

                    meet_player[2] = true;
                }
                if (q_learning_player == 3)
                {
                    poker_player_action("Middle", "call", 0);

                    if (rais)
                    {
                        if (pre_raise == 3)
                        {
                            rais = false;
                            set_new_round();
                        }
                    }

                    meet_player[3] = true;
                }
                if (q_learning_player == 4)
                {
                    poker_player_action("DB", "call", 0);
                    meet_player[4] = true;
                    if (rais)
                    {
                        if (pre_raise == 4)
                        {
                            rais = false;
                            set_new_round();
                        }
                    }
                    else
                    {
                        set_new_round();

                    }




                }
            }
             if (winner())
                set_handout();
             set_graphic();
        }
        private void b_raise_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
           try
            { 
            if (b.Name == "b_raise0" && hand_out == 0)
            {
                meet_player[0] = true;
                if (500<int.Parse(txtbet0.Text))
                    txtbet0.Text = "500";
                    poker_player_action("SB", "raise", int.Parse(txtbet0.Text));
                
                    
            }

            if (b.Name == "b_raise1" && hand_out == 1)
            {
                meet_player[1] = true;
                if (500 < int.Parse(txtbet1.Text))
                    txtbet1.Text = "500";
                poker_player_action("BB", "raise", int.Parse(txtbet1.Text));
            }
            if (b.Name == "b_raise2" && hand_out == 2)
            {
                meet_player[2] = true;
                if (500 < int.Parse(txtbet2.Text))
                    txtbet2.Text = "500";
                poker_player_action("UDG", "raise", int.Parse(txtbet2.Text));
            }

                if (b.Name == "b_raise3" && hand_out == 3)
                {
                    meet_player[3] = true;
                    if (500<int.Parse(txtbet3.Text))
                        txtbet3.Text = "500";
                    poker_player_action("Middle", "raise", int.Parse(txtbet3.Text));
                    
                        
                }

                if (b.Name == "b_raise4" && hand_out == 4)
                {
                    meet_player[4] = true;
                    if (500 < int.Parse(txtbet4.Text))
                        txtbet4.Text = "500";
                    poker_player_action("DB", "raise", int.Parse(txtbet4.Text));
                }
            }
           catch
            {
                if (q_learning_player == 2)
                {
                    meet_player[2] = true;
                    if (500<int.Parse(txtbet2.Text))
                        txtbet2.Text = "500";
                    poker_player_action("UDG", "raise", int.Parse(txtbet2.Text));
                }
                if (q_learning_player == 3)
                {
                    meet_player[3] = true;
                    if (500 < int.Parse(txtbet3.Text))
                        txtbet3.Text = "500";
                    poker_player_action("Middle", "raise", int.Parse(txtbet3.Text));
                }

                if (q_learning_player == 4)
                {
                    meet_player[4] = true;
                    if (500<int.Parse(txtbet4.Text))
                        txtbet4.Text = "500";
                        poker_player_action("DB", "raise", int.Parse(txtbet4.Text));
                }
            }
            
       
            if (winner())
                set_handout();
            
            set_graphic();
        }
        public bool winner()
        {
            int c = 0;
            for (int i = 0; i < 5; i++)
            {
                if (enable_player[i])
                    c++;
            }
            if (c != 1)
            {

                return true;
            }
            else
            {
                   round = 10;
                hand_out = 10;
                return false;
            }
        }
        int hand_q;
        public void set_handout()
         {
             int h1 = hand_out;
             while (true)
             {
                 if (hand_out < 4)
                     hand_out++;
                 else
                     hand_out = 0;
                 if (enable_player[hand_out])
                     break;
             }
             hand_q = hand_out;
         }
        public void set_new_round()
        {
            bool flag = true;
            for (int i = 0; i < 5; i++)
            {
                if (meet_player[i] == false)
                    flag = false;
            }
            if (flag)
                new_round();
        }
        /////////////////////////////////////////////////////////////////////////////////
        public void set_graphic()
        {
            txtposition0.Text = position[0];//p_
            txtposition1.Text = position[1];//p_
            txtposition2.Text = position[2];//p_
            txtposition3.Text = position[3];//p_
            txtposition4.Text = position[4];//p_
            /////////////////////////////////////////////
            txtbet1.Text = min_bet.ToString();//p_
            /////////////////////////////////////////////
            b_fold0.Enabled = visible_button[0, 0];//p_
            b_fold1.Enabled = visible_button[1, 0];//p_
            b_fold2.Enabled = visible_button[2, 0];//p_
            b_fold3.Enabled = visible_button[3, 0];//p_
            b_fold4.Enabled = visible_button[4, 0];//p_
            //////////////////////////////////////////////////////
            b_check0.Enabled = visible_button[0, 1];//p_
            b_check1.Enabled = visible_button[1, 1];//p_
            b_check2.Enabled = visible_button[2, 1];//p_
            b_check3.Enabled = visible_button[3, 1];//p_
            b_check4.Enabled = visible_button[4, 1];//p_
            /////////////////////////////////////////////////////////
            b_call0.Enabled = visible_button[0, 2];//p_
            b_call1.Enabled = visible_button[1, 2];//p_
            b_call2.Enabled = visible_button[2, 2];//p_
            b_call3.Enabled = visible_button[3, 2];//p_
            b_call4.Enabled = visible_button[4, 2];//p_
            //////////////////////////////////////////////////////////
            b_raise0.Enabled = visible_button[0, 3];//p_
            b_raise1.Enabled = visible_button[1, 3];//p_
            b_raise2.Enabled = visible_button[2, 3];//p_
            b_raise3.Enabled = visible_button[3, 3];//p_
            b_raise4.Enabled = visible_button[4, 3];//p_
            //////////////////////////////////////////////////////////////
            txtcard0.Text = cards[0, 0] + "-" + cards[0, 1];//p_
            txtcard1.Text = cards[1, 0] + "-" + cards[1, 1];//p_
            txtcard2.Text = cards[2, 0] + "-" + cards[2, 1];//p_
            txtcard3.Text = cards[3, 0] + "-" + cards[3, 1];//p_
            txtcard4.Text = cards[4, 0] + "-" + cards[4, 1];//p_
            //////////////////////////////////////////////////////////////
            if (round == 0)
                txtround.Text = "PreFlap";
            if (round == 1)
            {
                txtround.Text = "Flap";
                txttable_card0.Visible = true;//p_
                txttable_card1.Visible= true;//p_
                txttable_card2.Visible = true;//p_
                txttable_card0.Text = cards_table[0];//p_
                txttable_card1.Text = cards_table[1];//p_
                txttable_card2.Text = cards_table[2];//p_
            }
            if (round == 2)
            {
                txtround.Text = "Turn";
                txttable_card3.Visible = true;//p_
                txttable_card3.Text = cards_table[3];//p_
            }
            if (round == 3)
            {
                txtround.Text = "River";
                txttable_card4.Visible = true;//p_
                txttable_card4.Text = cards_table[4];//p_
            }
            //////////////////////////////////////////////////////////////
            txtpot.Text = pot.ToString();//p_
            txtmoney0.Text = money[0].ToString();//p_
            txtmoney1.Text = money[1].ToString();//p_
            txtmoney2.Text = money[2].ToString();//p_
            txtmoney3.Text = money[3].ToString();//p_
            txtmoney4.Text = money[4].ToString();//p_
            //////////////////////////////////////////////////////////////
            txtbet0.Text = bet[0].ToString();//p_
            txtbet1.Text = bet[1].ToString();//p_
            txtbet2.Text = bet[2].ToString();//p_
            txtbet3.Text = bet[3].ToString();//p_
            txtbet4.Text = bet[4].ToString();//p_
            //////////////////////////////////////////////////////////////
            switch (hand_out)
            {
                case 0:
                    {
                        b_hand0.BackColor = Color.Cyan;
                        b_hand1.BackColor = Color.White;
                        b_hand2.BackColor = Color.White;
                        b_hand3.BackColor = Color.White;
                        b_hand4.BackColor = Color.White;
                    }
                    break;
                case 1:
                    {
                        b_hand0.BackColor = Color.White;
                        b_hand1.BackColor = Color.Cyan;
                        b_hand2.BackColor = Color.White;
                        b_hand3.BackColor = Color.White;
                        b_hand4.BackColor = Color.White;
                    }
                    break;
                case 2:
                    {
                        b_hand0.BackColor = Color.White;
                        b_hand1.BackColor = Color.White;
                        b_hand2.BackColor = Color.Cyan;
                        b_hand3.BackColor = Color.White;
                        b_hand4.BackColor = Color.White;
                    }
                    break;
                case 3:
                    {
                        b_hand0.BackColor = Color.White;
                        b_hand1.BackColor = Color.White;
                        b_hand2.BackColor = Color.White;
                        b_hand3.BackColor = Color.Cyan; ;
                        b_hand4.BackColor = Color.White;
                    }
                    break;
                case 4:
                    {
                        b_hand0.BackColor = Color.White;
                        b_hand1.BackColor = Color.White;
                        b_hand2.BackColor = Color.White;
                        b_hand3.BackColor = Color.White;
                        b_hand4.BackColor = Color.Cyan;
                    }
                    break;
            }
            if(enable_player[0]==false)
            {
                b_fold0.Enabled = false;
                b_check0.Enabled = false;
                b_call0.Enabled = false;
                b_raise0.Enabled = false;
                b_hand0.BackColor = Color.Red;
                b_hand0.Text = "fold";
            }
            if (enable_player[1] == false)
            {
                b_fold1.Enabled = false;
                b_check1.Enabled = false;
                b_call1.Enabled = false;
                b_raise1.Enabled = false;
                b_hand1.BackColor = Color.Red;
                b_hand1.Text = "fold";
            }
            if (enable_player[2] == false)
            {
                b_fold2.Enabled = false;
                b_check2.Enabled = false;
                b_call2.Enabled = false;
                b_raise2.Enabled = false;
                b_hand2.BackColor = Color.Red;
                b_hand2.Text = "fold";
            }
            if (enable_player[3] == false)
            {
                b_fold3.Enabled = false;
                b_check3.Enabled = false;
                b_call3.Enabled = false;
                b_raise3.Enabled = false;
                b_hand3.BackColor = Color.Red;
                b_hand3.Text = "fold";
            }
            if (enable_player[4] == false)
            {
                b_fold4.Enabled = false;
                b_check4.Enabled = false;
                b_call4.Enabled = false;
                b_raise4.Enabled = false;
                b_hand4.BackColor = Color.Red;
                b_hand4.Text = "fold";
            }
            if (q_learning_player == 2)
                p2.BackColor = Color.Magenta;
            if (q_learning_player == 3)
                p3.BackColor = Color.Magenta;
            if (q_learning_player == 4)
                p4.BackColor = Color.Magenta;
           if(hand_q==q_learning_player && enable_player[hand_q])
            qlearning();
            txtminbet.Text = min_bet.ToString();
            
        }
    }
}
