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



        /// 
        /// //////////////////////  VARIABLES
        /// 



        const int taille_case = 60;
        const int taille_grille = 10;
        Color clr1 = Color.DarkGray;
        Color clr2 = Color.White;

        List<(int x, int y)> pion_noir = new List<(int x, int y)>();
        List<(int x, int y)> pion_blanc = new List<(int x, int y)>();

        List<(int x, int y)> dame_noir = new List<(int x, int y)>();
        List<(int x, int y)> dame_blanc = new List<(int x, int y)>();

        (int x ,int y ) select_pion = (0,0) ;







        /// 
        /// //////////////////////  INITIALISATION
        /// 



        private void Form1_Load(object sender, EventArgs e)
        {

            pion_blanc.Clear();
            pion_noir.Clear();
            C_a_qui = 0;

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

            Joue_un_coup();
            textBox5.Text = "Joueur: Sélection d'un pion";

        }





        /// 
        /// //////////////////////  CLICK SUR LE DAMIER
        /// 



        bool select = false;
        bool vas_y = false;
        bool manger = false;
        bool re_manger = false;
        int A_old;
        int B_old;

        int souris_x;
        int souris_y;

        int case_x,  case_y ;
        int case_x_old, case_y_old, case_x_old1, case_y_old1;

        private void Form1_Click(object sender, EventArgs e)
        {

            if (equipe_IA != C_a_qui)
            {

                if (souris_x < (taille_grille * taille_case) && souris_y < (taille_grille * taille_case))
                {
                    case_x = (int)souris_x / taille_case;
                    case_y = (int)souris_y / taille_case;


                    int A = pion_noir.IndexOf((case_x, case_y));
                    int B = pion_blanc.IndexOf((case_x, case_y));


                    if ((A != -1 || B != -1) && select == false) // selection d'un pion, sur une case non-vide
                    {



                        A_old = A;
                        B_old = B;

                        if (A_old == -1) equipe = 0;
                        if (B_old == -1) equipe = 1;

                        select_pion = (case_x, case_y);
                        select = true;
                        this.Refresh();



                        textBox5.Text = "Joueur: Sélection case d'arrivée";
                    }
                    else  if ((A == -1 && B == -1) && select == true) // selection case d'arivée, sur case vide
                    {

                        case_x_old1 = case_x;
                        case_y_old1 = case_y;

                        if (A_old == -1)
                        {
                            (case_x_old, case_y_old) = pion_blanc[B_old];
                        }
                        if (B_old == -1)
                        {
                            (case_x_old, case_y_old) = pion_noir[A_old];
                        }



                        vas_y = false;
                        manger = false;
                        vas_y = verif_case_noire(case_x, case_y);
                        manger = verif_manger(case_x_old, case_y_old, equipe);
                        vas_y = verif_deplacement(case_x, case_y);


                        if (manger == true)
                        {
                            manger_pion(equipe, case_x, case_y, case_x_old, case_y_old);
                        }


                        if (vas_y == true)
                        {
                            if (A_old == -1)
                            {
                                pion_blanc[B_old] = (case_x, case_y);
                            }
                            if (B_old == -1)
                            {
                                pion_noir[A_old] = (case_x, case_y);
                            }



                            re_manger = false;

                            if (manger == true)
                            {
                                textBox5.Text = "Joueur: Continuez à manger";
                                select_pion = (case_x, case_y);

                                this.Refresh();


                                re_manger = verif_manger(case_x, case_y, equipe);
                            }

                            if (manger == false || re_manger == false)
                            {
                                select = false;
                                manger = false;

                                select_pion = (0, 0);

                                C_a_qui = equipe_IA;
                                Joue_un_coup();

                                this.Refresh();

                                textBox5.Text = "Joueur: Sélection d'un pion";
                            }


                        }


                    }
                    else
                    {
                        select_pion = (0, 0);
                        select = false;
                        this.Refresh();

                        textBox5.Text = "Probleme";
                    }

                }
                else // annule déplacement, si re-clique sur case ocuppée.
                {
                    select_pion = (0, 0);
                    select = false;
                    this.Refresh();

                    textBox5.Text = "Case occupée";
                }


            }

            this.Refresh();

            Joue_un_coup();


            verif_passage_dames();

            verif_fin_partie(sender, e);


        }




        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            souris_x = e.X;
            souris_y = e.Y;
        }






       

        /// 
        /// //////////////////////  GRAPHIQUE
        /// 

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

            if (vas_y == true)
            {

                Graphics g = e.Graphics;
                Pen p = new Pen(Brushes.Orange, 2);
                p.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                g.DrawLine(p, case_x_old1 * taille_case + taille_case / 2, case_y_old1 * taille_case + taille_case / 2, case_x_old * taille_case + taille_case / 2, case_y_old * taille_case + taille_case / 2);

            }
            if(meilleur.poids != -1)
            {
                Graphics g = e.Graphics;
                Pen p = new Pen(Brushes.Red, 5);
                p.StartCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
                g.DrawLine(p,destination.x * taille_case+ taille_case/2, destination.y * taille_case + taille_case / 2, meilleur.x*taille_case + taille_case / 2, meilleur.y * taille_case + taille_case / 2);
            }

                textBox1.Text = pion_noir.Count.ToString();
            textBox2.Text = pion_blanc.Count.ToString();



        }










        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        int equipe_IA = 0; // blanc

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void newgame_Click(object sender, EventArgs e)
        {
            Form1_Load(sender,e);
;       }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                equipe_IA = 0;
                checkBox2.Checked = false;
                Form1_Load(sender, e);
                Refresh();
            }


        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                equipe_IA = 1;
                checkBox1.Checked = false;
                Form1_Load(sender,e);
                Refresh();
            }
        }


    }




}
