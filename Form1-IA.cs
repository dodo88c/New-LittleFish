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



        List<(int x, int y)> pion_IA = new List<(int x, int y)>();
        List<(int x, int y)> pion_adv = new List<(int x, int y)>();

        List<(int poids, int x, int y, int dir_vert, int dir_hor, bool mange)> list_coup_a_jouer = new List<(int poids ,int x,int y, int dir_vert, int dir_hor, bool mange)>();

        (int[] HG, int[] HD, int[] BD, int[] BG) direction_valide  = ( new int[10], new  int[10], new int[10], new int[10]);

        (int poids, int x, int y, int dir_vert, int dir_hor, bool mange) meilleur = (-1, 0, 0, 0, 0, false);
        (int x, int y) destination = (0, 0);



        private void Joue_un_coup()
        {
            list_coup_a_jouer.Clear();

            if (equipe_IA == C_a_qui)
            {
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


                prochain_coup();



                meilleur = (-1, 0, 0, 0, 0, false);

                foreach ((int poids, int x, int y, int dir_vert, int dir_hor, bool mange) coup in list_coup_a_jouer)
                {
                    if (coup.poids >= meilleur.poids)
                        meilleur = coup;
                }


                (int x, int y) origine = (meilleur.x, meilleur.y);
                destination = (0,0);


                if (meilleur.mange == false)
                {
                    destination = (meilleur.x + 1, meilleur.y + 1);

                    if (meilleur.dir_vert == 0)
                        destination.y = meilleur.y - 1;
                    if (meilleur.dir_hor == 0)
                        destination.x = meilleur.x - 1;
                }
                if (meilleur.mange == true)
                {
                    destination = (meilleur.x + 2, meilleur.y + 2);

                    if (meilleur.dir_vert == 0)
                        destination.y = meilleur.y - 2;
                    if (meilleur.dir_hor == 0)
                        destination.x = meilleur.x - 2;


                    if(meilleur.dir_vert == 0 && meilleur.dir_hor == 0)
                        pion_adv.Remove((origine.x-1, origine.y-1));
                    if (meilleur.dir_vert == 0 && meilleur.dir_hor == 1)
                        pion_adv.Remove((origine.x + 1, origine.y - 1));
                    if (meilleur.dir_vert == 1 && meilleur.dir_hor == 0)
                        pion_adv.Remove((origine.x - 1, origine.y + 1));
                    if (meilleur.dir_vert == 1 && meilleur.dir_hor == 1)
                        pion_adv.Remove((origine.x + 1, origine.y + 1));


                }


                int A = pion_IA.IndexOf((origine.x, origine.y));
                pion_IA[A] = (destination.x, destination.y);




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





        int[] dir  = new int[10];
        int[] dir1 = new int[10];
        int[] dir2 = new int[10];

        int dir_vert = 0;
        int dir_hor = 0;



        private void prochain_coup()
        {

            int I = 0;
            foreach ((int x, int y) pion in pion_IA)
            {

                verif_deplacement_possible(pion.x, pion.y);



                for (int II = 0; II < 4; II++) {

                    if (equipe_IA == 0 && II == 0)
                    {
                        dir1 = direction_valide.HG;
                        dir2 = direction_valide.HD;
                        dir_vert = 0;
                    }
                    if (equipe_IA == 1 && II == 0)
                    {
                        dir1 = direction_valide.BG;
                        dir2 = direction_valide.BD;
                        dir_vert = 1;
                    }

                    if (equipe_IA == 0 && II == 2)
                    {
                        dir1 = direction_valide.BG;
                        dir2 = direction_valide.BD;
                        dir_vert = 1;

                    }
                    if (equipe_IA == 1 && II == 2)
                    {
                        dir1 = direction_valide.HG;
                        dir2 = direction_valide.HD;
                        dir_vert = 0;

                    }

                    if(II == 1 || II == 3)
                    {
                        dir = dir2;
                        dir_hor = 1;
                    }
                    if(II == 0 || II == 2)
                    {
                        dir = dir1;
                        dir_hor = 0;
                    }


                    (int poids, int x, int y, int dir_vert, int dir_hor, bool mange) prochain = (0, 0, 0, 0, 0, false);



                    if (dir[1] == 0 && dir[2] == 2 && II < 2)
                    {
                        prochain = ((0, pion.x, pion.y, dir_vert, dir_hor, false));   // risque de se faire manger au prochan tour;
                    }
                    else if (dir[1] == 0 && dir[2] == 1 && II < 2)
                    {
                        prochain = ((1, pion.x, pion.y, dir_vert, dir_hor, false));   // avance vers un allié;
                    }
                    else if (dir[1] == 0 && dir[2] == 0 && II < 2)
                    {
                        prochain = ((1, pion.x, pion.y, dir_vert, dir_hor, false));   // avance vers rien;
                    }
                    else if (dir[1] == 0 && dir[2] == -1 && II < 2 )
                    {
                        prochain = ((2, pion.x, pion.y, dir_vert, dir_hor, false));   // protege dans un coin;
                    }
                    else if (dir[1] == 2 && dir[2] == 0)
                    {
                        prochain = ((3, pion.x, pion.y, dir_vert, dir_hor, true));   // mange au moins 1;
                    }
                    else
                        prochain = ((-1, pion.x, pion.y, dir_vert, dir_hor, false));   // pas possible de jouer


                    if (prochain.poids != -1)
                        list_coup_a_jouer.Add(prochain);
                }



                I++;
            }

        }












        /*
            direction_valide, fonctionement :         

            X X X X X X X X X X X X X
            X 0                   1 X
            X   1               1   X           0 : case vide
            X     0           1     X           1 : case alliée
            X       0       0       X           2 : case ennemie
            X         1   1         X           X : -1 hors plateau
            X           0           X
            X         2   0         X
            X       0       2       X
            X     2           2     X
            X   0               2   X
            X X X X X X X X X X X X X
        */


        private void verif_deplacement_possible(int x, int y)
        {

            for(int i = 0; i < 10; i++)
            {

                if ((x - i) < 0 || (y - i) < 0)
                {   direction_valide.HG[i] = -1;  }
                else if ((x - i) > 9 || (y - i) > 9)
                {   direction_valide.HG[i] = -1;  }
                else 
                {
                    int A = pion_IA.IndexOf((x - i, y - i));
                    int B = pion_adv.IndexOf((x - i, y - i));

                    if (A != -1)
                        direction_valide.HG[i] = 1;
                    else if (B != -1)
                        direction_valide.HG[i] = 2;
                    else
                        direction_valide.HG[i] = 0;
                }



                if ((x + i) < 0 || (y - i) < 0)
                { direction_valide.HD[i] = -1; }
                else if ((x + i) > 9 || (y - i) > 9)
                { direction_valide.HD[i] = -1; }
                else
                {
                    int A = pion_IA.IndexOf((x + i, y - i));
                    int B = pion_adv.IndexOf((x + i, y - i));

                    if (A != -1)
                        direction_valide.HD[i] = 1;
                    else if (B != -1)
                        direction_valide.HD[i] = 2;
                    else
                        direction_valide.HD[i] = 0;
                }



                if ((x + i) < 0 || (y + i) < 0)
                { direction_valide.BD[i] = -1; }
                else if ((x + i) > 9 || (y + i) > 9)
                { direction_valide.BD[i] = -1; }
                else
                {
                    int A = pion_IA.IndexOf((x + i, y + i));
                    int B = pion_adv.IndexOf((x + i, y + i));

                    if (A != -1)
                        direction_valide.BD[i] = 1;
                    else if (B != -1)
                        direction_valide.BD[i] = 2;
                    else
                        direction_valide.BD[i] = 0;
                }


                if ((x - i) < 0 || (y + i) < 0)
                { direction_valide.BG[i] = -1; }
                else if ((x - i) > 9 || (y + i) > 9)
                { direction_valide.BG[i] = -1; }
                else
                {
                    int A = pion_IA.IndexOf((x - i, y + i));
                    int B = pion_adv.IndexOf((x - i, y + i));

                    if (A != -1)
                        direction_valide.BG[i] = 1;
                    else if (B != -1)
                        direction_valide.BG[i] = 2;
                    else
                        direction_valide.BG[i] = 0;
                }
            }
        }


















    }
}
