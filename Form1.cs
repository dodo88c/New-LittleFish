using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms;

namespace New_LittleFish
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        const int taille_case = 60;
        const int taille_grille = 10;
        Color clr1 = Color.DarkGray;
        Color clr2 = Color.White;

        List<(int x, int y)> pion_noir = new List<(int x, int y)>();
        List<(int x, int y)> pion_blanc = new List<(int x, int y)>();

        (int x ,int y ) select_pion = (0,0) ;



        private void Form1_Load(object sender, EventArgs e)
        {



            for (var n = 0; n < taille_grille; n++) // genere les lignes
            {
                for (var m = 0; m < taille_grille; m++) // genere les colonnes
                {
                    if (m % 2 == 0 && n < 4)
                    {
                        int A = n % 2 != 0 ? 0 : 1;

                        pion_noir.Add((m+A, n));
                        
                    }

                    if (m % 2 == 0 && n > 5) 
                    {
                        int A = n % 2 != 0 ? 0 : 1;

                        pion_blanc.Add((m+A, n));
                    }

                }
            }
        }



        bool select = false;
        int A_old;
        int B_old;

        int souris_x;
        int souris_y;

        private void Form1_Click(object sender, EventArgs e)
        {            
            
            if (souris_x < (taille_grille * taille_case) && souris_y < (taille_grille * taille_case) )
            {
                int case_x = (int)souris_x / taille_case,
                    case_y = (int)souris_y / taille_case;

                
                int A = pion_noir.IndexOf((case_x,case_y));
                int B = pion_blanc.IndexOf((case_x, case_y));


                if ((A != -1 || B != -1 ) && select == false )
                {
                    A_old = A;
                    B_old = B;

                    select_pion = (case_x, case_y);
                    select = true;
                    this.Refresh();
                }

                if ((A == -1 && B == -1) && select == true)
                {

                    if (A_old == -1)
                        pion_blanc[B_old] = (case_x, case_y);
                    if (B_old == -1)
                        pion_noir[A_old] = (case_x, case_y);


                    select_pion = (0, 0);
                    select = false;
                    this.Refresh();
                }


            }

            

        }





        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            for (var n = 0; n < taille_grille; n++)
            {
                for (var m = 0; m < taille_grille; m++)
                {
                    if (n % 2 == 0)
                    {
                        Color BackColor = m % 2 != 0 ? clr1 : clr2;
                        Pen p = new Pen(BackColor, 3);
                        Rectangle r = new Rectangle(taille_case*n, taille_case*m, taille_case, taille_case);
                        e.Graphics.DrawRectangle(p, r);
                        SolidBrush brush = new SolidBrush(BackColor);
                        e.Graphics.FillRectangle(brush, r);

                    }
                       
                    else
                    {
                        Color BackColor = m % 2 != 0 ? clr2 : clr1;
                        Pen p = new Pen(BackColor, 3);
                        Rectangle r = new Rectangle(taille_case*n, taille_case*m, taille_case, taille_case);
                        e.Graphics.DrawRectangle(p, r);
                        SolidBrush brush = new SolidBrush(BackColor);
                        e.Graphics.FillRectangle(brush, r);
                    }
                        

                }
            }



            foreach ((int x, int y) pion in pion_blanc)
            {
                Pen p = new Pen(Color.Gray, 3);
                Rectangle r = new Rectangle(pion.x * taille_case+10, pion.y * taille_case+10, taille_case - 20, taille_case - 20);
                e.Graphics.DrawEllipse(p, r);

                SolidBrush brush = new SolidBrush(Color.AntiqueWhite);
                e.Graphics.FillEllipse(brush, r);
            }
            foreach ((int x, int y) pion in pion_noir)
            {
                Pen p = new Pen(Color.Black, 3);
                Rectangle r = new Rectangle(pion.x * taille_case+10, pion.y * taille_case+10, taille_case-20, taille_case-20);
                e.Graphics.DrawEllipse(p, r);

                SolidBrush brush = new SolidBrush(Color.Black);
                e.Graphics.FillEllipse(brush, r);
            }


            if ( select == true)
            {
                Pen p = new Pen(Color.Red, 3);
                Rectangle r = new Rectangle(select_pion.x * taille_case + 10, select_pion.y * taille_case + 10, taille_case - 20, taille_case - 20);
                e.Graphics.DrawEllipse(p, r);

            }



        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            souris_x = e.X;
            souris_y = e.Y;
        }
    }




}
