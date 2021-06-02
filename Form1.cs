﻿using System;
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

        (int x ,int y ) select_pion = (0,0) ;







        /// 
        /// //////////////////////  INITIALISATION
        /// 



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





        /// 
        /// //////////////////////  CLICK SUR LE DAMIER
        /// 



        bool select = false;
        bool vas_y = false;
        bool manger = false;
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


                if ((A != -1 || B != -1 ) && select == false ) // selection d'un pion, sur une case non-vide
                {
                    A_old = A;
                    B_old = B;

                    if (A_old == -1) equipe = 0;
                    if (B_old == -1) equipe = 1;

                    select_pion = (case_x, case_y);
                    select = true;
                    this.Refresh();
                }
                else if ((A == -1 && B == -1) && select == true) // selection case d'arivée, sur case vide
                {


                    vas_y = false;
                    manger = false;
                    vas_y = verif_case_noire(case_x, case_y);
                    manger = verif_manger(case_x, case_y); 
                    vas_y = verif_deplacement(case_x, case_y);


                    if ( vas_y == true)
                    {
                        if (A_old == -1)
                            pion_blanc[B_old] = (case_x, case_y);
                        if (B_old == -1)
                            pion_noir[A_old] = (case_x, case_y);


                        select_pion = (0, 0);
                        select = false;
                        this.Refresh();
                    }
                    else
                    {
                        select_pion = (0, 0);
                        select = false;
                        this.Refresh();
                    }

                }
                else // annule déplacement, si re-clique sur case ocuppée.
                {
                    select_pion = (0,0);
                    select = false;
                    this.Refresh();
                }



            }

            

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            souris_x = e.X;
            souris_y = e.Y;
        }






        /// 
        /// //////////////////////  LOGIQUE INTERNE
        /// 

        int equipe = 0; // 0 = blanc, 1 = noir.


        private bool verif_case_noire(int x, int y)  // retourne si la destination est une case noire
        {

            if (x % 2 == 0)
                vas_y = y % 2 != 0 ? true : false;
            else
                vas_y = y % 2 != 0 ? false : true;

            return vas_y;

        }

        

        private bool verif_deplacement(int x, int y)  // retourne si la le déplacement est correct
        {
            bool go = false;

            if (manger == false) // deplacement simple
            {

                if (equipe == 1)  // si l'équipe est noire
                {
                    if (y == select_pion.y + 1 && (x == select_pion.x - 1 || x == select_pion.x + 1)) // si la case d'arrivée est valide
                        go = true;
                    else
                        go = false;
                }

                if (equipe == 0)  // si l'équipe est blanc
                {
                    if (y == select_pion.y - 1 && (x == select_pion.x - 1 || x == select_pion.x + 1)) // si la case d'arrivée est valide
                        go = true;
                    else
                        go = false;
                }
            }

            if (manger == true) // deplacement complexe
            {
                if (equipe == 1)  // si l'équipe est noire
                {
                    if ( (y == select_pion.y + 2 || y == select_pion.y - 2 ) && (x == select_pion.x - 2 || x == select_pion.x + 2)) // si la case d'arrivée est valide
                        go = true;
                    else
                        go = false;
                }

                if (equipe == 0)  // si l'équipe est blanc
                {
                    if ( (y == select_pion.y + 2 || y == select_pion.y - 2) && (x == select_pion.x - 2 || x == select_pion.x + 2)) // si la case d'arrivée est valide
                        go = true;
                    else
                        go = false;
                }
            }


                return go;

        }


        private bool verif_manger(int x, int y)  // regarde et élimine un pion mangé
        {
            bool miam = false;

            if (equipe == 0) // blanc
            {
                int exist1 = pion_noir.IndexOf((select_pion.x + 1, select_pion.y + 1)); // mange derriere droite
                int exist2 = pion_noir.IndexOf((select_pion.x + 1, select_pion.y - 1)); // mange  gauche
                int exist3 = pion_noir.IndexOf((select_pion.x - 1, select_pion.y + 1)); // mange avant droite
                int exist4 = pion_noir.IndexOf((select_pion.x - 1, select_pion.y - 1)); // mange avant gauche

                int vide1 = pion_noir.IndexOf((select_pion.x + 2, select_pion.y + 2)) + pion_blanc.IndexOf((select_pion.x + 2, select_pion.y + 2));
                int vide2 = pion_noir.IndexOf((select_pion.x + 2, select_pion.y - 2)) + pion_blanc.IndexOf((select_pion.x + 2, select_pion.y - 2)); 
                int vide3 = pion_noir.IndexOf((select_pion.x - 2, select_pion.y + 2)) + pion_blanc.IndexOf((select_pion.x - 2, select_pion.y + 2));
                int vide4 = pion_noir.IndexOf((select_pion.x - 2, select_pion.y - 2)) + pion_blanc.IndexOf((select_pion.x - 2, select_pion.y - 2)); 


                if ( (exist1 != -1 && vide1 == -2) || (exist2 != -1 && vide2 == -2) || (exist3 != -1 && vide3 == -2) || (exist4 != -1 && vide4 == -2))
                    miam = true;
            }

            if (equipe == 1) // noir
            {
                int exist1 = pion_blanc.IndexOf((select_pion.x + 1, select_pion.y + 1)); // mange avant droite
                int exist2 = pion_blanc.IndexOf((select_pion.x + 1, select_pion.y - 1)); // mange avant gauche
                int exist3 = pion_blanc.IndexOf((select_pion.x - 1, select_pion.y + 1)); // mange derriere droite
                int exist4 = pion_blanc.IndexOf((select_pion.x - 1, select_pion.y - 1)); // mange derriere gauche

                int vide1 = pion_noir.IndexOf((select_pion.x + 2, select_pion.y + 2)) + pion_blanc.IndexOf((select_pion.x + 2, select_pion.y + 2));
                int vide2 = pion_noir.IndexOf((select_pion.x + 2, select_pion.y - 2)) + pion_blanc.IndexOf((select_pion.x + 2, select_pion.y - 2));
                int vide3 = pion_noir.IndexOf((select_pion.x - 2, select_pion.y + 2)) + pion_blanc.IndexOf((select_pion.x - 2, select_pion.y + 2));
                int vide4 = pion_noir.IndexOf((select_pion.x - 2, select_pion.y - 2)) + pion_blanc.IndexOf((select_pion.x - 2, select_pion.y - 2));

                if ((exist1 != -1 && vide1 == -2) || (exist2 != -1 && vide2 == -2) || (exist3 != -1 && vide3 == -2) || (exist4 != -1 && vide4 == -2))
                    miam = true;
            }

            return miam;
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



        }












    }




}
