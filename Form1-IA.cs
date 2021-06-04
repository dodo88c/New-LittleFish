using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace New_LittleFish
{
    public partial class Form1 : Form
    {

        /// 
        /// //////////////////////  IA
        /// 


        int C_a_qui = 0; //blanc

        private void Joue_un_coup()
        {


            if(equipe_IA == C_a_qui)
            {

                List<(int x, int y)> pion_IA = new List<(int x, int y)>();
                List<(int x, int y)> pion_adv = new List<(int x, int y)>();

                if( equipe_IA == 0)
                {
                    pion_IA = pion_blanc;
                    pion_adv = pion_noir;
                }
                else
                {
                    pion_IA = pion_noir;
                    pion_adv = pion_blanc;
                }






                pion_IA.Clear();












                if (equipe_IA == 0)
                {
                    pion_blanc = pion_IA;
                    pion_noir = pion_adv;
                    C_a_qui = 1;
                }
                else
                {
                    pion_noir = pion_IA;
                    pion_blanc = pion_adv;
                    C_a_qui = 0;
                }


                textBox1.Text = pion_noir.Count.ToString();
                textBox2.Text = pion_blanc.Count.ToString();
                this.Refresh();


            }


        }

















    }
}
