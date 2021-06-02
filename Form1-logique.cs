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
                    if ((y == select_pion.y + 2 || y == select_pion.y - 2) && (x == select_pion.x - 2 || x == select_pion.x + 2)) // si la case d'arrivée est valide
                        go = true;
                    else
                        go = false;
                }

                if (equipe == 0)  // si l'équipe est blanc
                {
                    if ((y == select_pion.y + 2 || y == select_pion.y - 2) && (x == select_pion.x - 2 || x == select_pion.x + 2)) // si la case d'arrivée est valide
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


                if (exist1 != -1 && vide1 == -2)
                {
                    miam = true;
                    pion_noir.Remove((select_pion.x + 1, select_pion.y + 1));
                }
                if (exist2 != -1 && vide2 == -2)
                {
                    miam = true;
                    pion_noir.Remove((select_pion.x + 1, select_pion.y - 1));
                }
                if (exist3 != -1 && vide3 == -2)
                {
                    miam = true;
                    pion_noir.Remove((select_pion.x - 1, select_pion.y + 1));
                }
                if (exist4 != -1 && vide4 == -2)
                {
                    miam = true;
                    pion_noir.Remove((select_pion.x - 1, select_pion.y - 1));
                }

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

                if (exist1 != -1 && vide1 == -2)
                {
                    miam = true;
                    pion_blanc.Remove((select_pion.x + 1, select_pion.y + 1));
                }
                if (exist2 != -1 && vide2 == -2)
                {
                    miam = true;
                    pion_blanc.Remove((select_pion.x + 1, select_pion.y - 1));
                }
                if (exist3 != -1 && vide3 == -2)
                {
                    miam = true;
                    pion_blanc.Remove((select_pion.x - 1, select_pion.y + 1));
                }
                if (exist4 != -1 && vide4 == -2)
                {
                    miam = true;
                    pion_blanc.Remove((select_pion.x - 1, select_pion.y - 1));
                }
            }



            textBox1.Text = pion_noir.Count.ToString();
            textBox2.Text = pion_blanc.Count.ToString();

            return miam;
        }





    }


}
