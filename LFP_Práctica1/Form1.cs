using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace LFP_Práctica1
{
    public partial class Form1 : Form
    {
        Token token = new Token();
        List<string> error = new List<string>();
        RichTextBox[] richboxes = new RichTextBox[100];
        int estado = 0;
        string auxLexema;
        string desconocido = "desconocido";
        int numero = 1;
        char[] cadena = new char[5000];
        List<char> caracter = new List<char>();
        int fila = 1;
        int columna = 0;
        int id;
        int nodo;
        List<Token> lista = new List<Token>();
        List<string> reservadas = new List<string>(new string[] { "valla", "empresa", "fondo", "tamanio", "horizontal", "vertical", "color", "pixel", "posicionx", "posiciony" });
        List<string> color = new List<string>(new string[] { "azul", "rojo", "amarillo", "verde", "AZUL", "ROJO", "AMARILLO", "VERDE" });


        public Form1()
        {
            InitializeComponent();
            richboxes[0] = richTextBox1;
            richboxes[1] = richTextBox2;
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void acercaDeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nombre: Alejandro Jjosep Xavier López Castillo" + "\n" + "Carnet:    201314735", "Acerca de:", MessageBoxButtons.OKCancel, MessageBoxIcon.None);
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int numero = tabControl2.TabCount;
            string title = "Archivo " + (numero + 1).ToString();
            richboxes[numero] = new RichTextBox();
            TabPage myTabPage = new TabPage(title);
            tabControl2.TabPages.Add(myTabPage);

            richboxes[numero].Dock = DockStyle.Fill;
            myTabPage.Controls.Add(richboxes[numero]);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }


        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {

            richboxes[tabControl2.SelectedIndex].Focus();
            richboxes[tabControl2.SelectedIndex].Clear();



            openFileDialog1.Filter = "vp502 Files (.vp502)|*.vp502";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                richboxes[tabControl2.SelectedIndex].Focus();
                richboxes[tabControl2.SelectedIndex].Clear();


                System.IO.StreamReader sr = new

                System.IO.StreamReader(openFileDialog1.FileName);

                tabControl2.SelectedTab.Text = openFileDialog1.SafeFileName;
                richboxes[tabControl2.SelectedIndex].SelectedText = (sr.ReadToEnd());



                sr.Close();
            }
        }

        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();

            saveFile1.DefaultExt = "*.vp502";
            saveFile1.Filter = "vp502 Files|*.vp502";

            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK &&
               saveFile1.FileName.Length > 0)
            {

                richboxes[tabControl2.SelectedIndex].SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
            }
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void verToolStripMenuItem_Click(object sender, EventArgs e)
        {

            auxLexema = "";
            string a = "";
            fila = 1;
            columna = 0;
            lista.Clear();
            caracter.Clear();

            a = richboxes[tabControl2.SelectedIndex].Text;
           

            caracter.AddRange(a);


            Automata();

        }

        public void Automata()
        {

            caracter.Add('*');

            numero = 1;
            using (System.IO.StreamWriter sr = new System.IO.StreamWriter("Tabla de Errores.html", false))
            {
                sr.Write("<!DOCTYPE html><html><style>table, th, td { border: 1px solid black; }tr:nth-child(even){background-color: #f2f2f2}th { background - color: #4CAF50; color: Blue; }</style><body><table style = \"width:100%\"><tr><th>#</th><th> Fila </th><th> Columna </th><th> Carácter </th><th> Descripción </th></tr>");



                for (int i = 0; i < caracter.Count(); i++)
                {



                    switch (estado)
                    {
                        case 0:

                            if (caracter.ElementAt(i).Equals('\r') || caracter.ElementAt(i).Equals('\n'))
                            {
                                fila += 1;
                                columna = 0;
                                estado = 0;
                            }

                            else if (caracter.ElementAt(i).Equals('\t') || caracter.ElementAt(i).Equals('\b') || caracter.ElementAt(i).Equals(' '))
                            {
                                columna += 1;
                                estado = 0;
                            }

                            else if (caracter.ElementAt(i).Equals('<'))
                            {
                                auxLexema = auxLexema + caracter.ElementAt(i).ToString();
                                columna += 1;
                                estado = 1;
                            }


                            else if (caracter.ElementAt(i).Equals('>'))
                            {
                                auxLexema = auxLexema + caracter.ElementAt(i).ToString();
                                columna += 1;
                                estado = 2;
                            }



                            else if (caracter.ElementAt(i).Equals('/'))
                            {
                                auxLexema = auxLexema + caracter.ElementAt(i).ToString();
                                columna += 1;
                                estado = 3;
                            }

                            else if (char.IsLetter(caracter.ElementAt(i)))
                            {
                                auxLexema = auxLexema + caracter.ElementAt(i).ToString().ToLower();
                                columna += 1;
                                estado = 4;
                            }

                            else if (caracter.ElementAt(i).Equals('"'))
                            {
                                auxLexema = auxLexema + caracter.ElementAt(i).ToString();
                                columna += 1;
                                estado = 6;
                            }


                            else if (char.IsDigit(caracter.ElementAt(i)))
                            {
                                auxLexema = auxLexema + caracter.ElementAt(i).ToString();
                                columna += 1;
                                estado = 5;
                            }


                            else
                            {

                                if (caracter.ElementAt(i).Equals('*') && i == caracter.Count - 1)
                                {
                                    Console.WriteLine("Fin Análisis Léxico");

                                }

                                else
                                {

                                    error.Add(caracter.ElementAt(i).ToString());

                                    sr.Write("<tr>");
                                    sr.Write("<td>" + (numero++) + "</td>");
                                    sr.Write("<td>" + fila + "</td>");
                                    sr.Write("<td>" + columna + "</td>");
                                    sr.Write("<td>" + caracter.ElementAt(i).ToString() + "</td>");
                                    sr.Write("<td>" + desconocido + "</td>");
                                    sr.Write("</tr>");

                                    columna += 1;



                                    estado = 0;
                                }


                            }

                            break;

                        case 1:


                            id = 1;
                            addToken(Tipo.TOKEN_MENORQ);

                            i = i - 1;
                            estado = 0;
                            break;

                        case 2:

                            id = 2;
                            addToken(Tipo.TOKEN_MAYORQ);
                            i = i - 1;
                            estado = 0;
                            break;

                        case 3:

                            id = 3;
                            addToken(Tipo.TOKEN_CERRARETIQUETA);

                            i = i - 1;
                            estado = 0;
                            break;

                        case 4:

                            if (char.IsLetter(caracter.ElementAt(i)))
                            {
                                columna += 1;
                                auxLexema = auxLexema + caracter.ElementAt(i).ToString().ToLower();

                                estado = 4;

                            }
                            else
                            {

                                if (reservadas.Contains(auxLexema))
                                {
                                    id = 4;


                                    addToken(Tipo.TOKEN_RESERVADA);

                                    i = i - 1;
                                    estado = 0;

                                }

                                else if (color.Contains(auxLexema))
                                {

                                    id = 5;
                                    addToken(Tipo.TOKEN_COLOR);

                                    i = i - 1;
                                    estado = 0;

                                }
                                else
                                {

                                    error.Add(caracter.ElementAt(i).ToString());

                                    sr.Write("<tr>");
                                    sr.Write("<td>" + (numero++) + "</td>");
                                    sr.Write("<td>" + fila + "</td>");
                                    sr.Write("<td>" + columna + "</td>");
                                    sr.Write("<td>" + auxLexema + "</td>");
                                    sr.Write("<td>" + "Error Léxico" + "</td>");
                                    sr.Write("</tr>");
                                    auxLexema = "";
                                    columna += 1;

                                    estado = 0;
                                }

                            }

                            break;


                        case 5:

                            if (char.IsDigit(caracter.ElementAt(i)))
                            {
                                columna += 1;
                                auxLexema = auxLexema + caracter.ElementAt(i).ToString();
                                estado = 5;

                            }
                            else

                            {
                                id = 7;
                                addToken(Tipo.TOKEN_NUMERO);

                                i = i - 1;
                                estado = 0;
                            }

                            break;


                        case 6:

                            if (caracter.ElementAt(i).Equals('"'))
                            {

                                id = 6;
                                auxLexema = auxLexema + caracter.ElementAt(i).ToString();
                                addToken(Tipo.TOKEN_IDENTIFICADOR);
                                //  i = i - 1;
                                estado = 0;


                            }
                            else
                            {


                                columna += 1;
                                auxLexema = auxLexema + caracter.ElementAt(i).ToString();

                                estado = 6;


                            }

                            break;




                        default:

                            break;


                    }

                }
                sr.Write("</table></body></html>");

                sr.Close();


            }
        }



        private void addToken(Tipo tipo)
        {

            lista.Add(new Token(auxLexema, fila, columna, id, tipo));
            auxLexema = "";
            estado = 0;

        }



        private void archivosDeSalidaToolStripMenuItem_Click(object sender, EventArgs e)
        {






        }


        public void reporteToken()
        {
            numero = 1;
            using (System.IO.StreamWriter sr = new System.IO.StreamWriter("Tabla de Símbolos.html", false))
            {
                sr.Write("<!DOCTYPE html><html><style>table, th, td { border: 1px solid black; }tr:nth-child(even){background-color: #f2f2f2}th { background - color: #4CAF50; color: Blue; }</style><body><table style = \"width:100%\"><tr><th>#</th><th> Lexema </th><th> Fila </th><th> Columna </th><th> Id Token </th><th> Token </th></tr>");

                foreach (Token prime in lista)
                {

                    // Console.WriteLine(prime.Lexema + ", " + prime.Fila + ", " + prime.Columna + ", " + prime.Id + ", " + prime.TipoToken);


                    sr.Write("<tr>");
                    sr.Write("<td>" + (numero++) + "</td>");
                    sr.Write("<td>" + prime.Lexema + "</td>");
                    sr.Write("<td>" + prime.Fila + "</td>");
                    sr.Write("<td>" + prime.Columna + "</td>");
                    sr.Write("<td>" + prime.Id + "</td>");
                    sr.Write("<td>" + prime.TipoToken + "</td>");

                    sr.Write("</tr>");


                }

                sr.Write("</table></body></html>");

                sr.Close();
            }


        }

        private void tablaDeSímbolosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            reporteToken();

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = @"Tabla de Símbolos.html";
            info.UseShellExecute = true;
            Process.Start(info);
        }

        private void tablaDeErroresToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = @"Tabla de Errores.html";
            info.UseShellExecute = true;
            Process.Start(info);
        }

        private void abrirToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            pintarFondo();
            //pintarPixel();
            graphviz();



            System.Diagnostics.Process.Start("valla.png");


        }

        public void pintarFondo()
        {

            int horizontal = 0, vertical = 0;
            string colorFondo = "";
            int posx = 0;
            int posy = 0;
            string colorPixel = "";
            System.IO.StreamWriter grafo = new System.IO.StreamWriter("valla.txt", false);
            nodo = 1;






            for (int y = 0; y < lista.Count; y++)
            {
                if (lista.ElementAt(y).Lexema.Equals("tamanio") && lista.ElementAt(y - 3).Lexema.Equals("fondo"))
                {
                    for (int z = 0; z < lista.Count; z++)
                    {

                        if (lista.ElementAt(z).Lexema.Equals("horizontal") && lista.ElementAt(z - 1).Lexema.Equals("/"))
                        {

                            horizontal = Convert.ToInt32(lista.ElementAt(z - 3).Lexema);

                        }

                        else if (lista.ElementAt(z).Lexema.Equals("vertical") && lista.ElementAt(z - 1).Lexema.Equals("/"))
                        {

                            vertical = Convert.ToInt32(lista.ElementAt(z - 3).Lexema);

                        }


                        else if (lista.ElementAt(z).Lexema.Equals("color") && lista.ElementAt(z - 1).Lexema.Equals("/"))
                        {
                            if (lista.ElementAt(z - 3).Lexema.Equals("verde"))
                            {
                                colorFondo = "fillcolor=green,";
                            }
                            else if (lista.ElementAt(z - 3).Lexema.Equals("rojo"))
                            {
                                colorFondo = "fillcolor=red,";
                            }
                            else if (lista.ElementAt(z - 3).Lexema.Equals("azul"))
                            {
                                colorFondo = "fillcolor=blue,";
                            }
                            else if (lista.ElementAt(z - 3).Lexema.Equals("amarillo"))
                            {
                                colorFondo = "fillcolor=yellow,";
                            }

                        }

                        else if (lista.ElementAt(z).Lexema.Equals("fondo") && lista.ElementAt(z - 1).Lexema.Equals("/"))
                        {

                            break;

                        }



                    }

                    grafo.Write(" digraph G{  nodesep = 0.2; ranksep=0; node[shape = circle,style=filled," + colorFondo + "fixedsize=true,fontsize=5]; edge[style = invis];");

                    for (int j = 0; j < vertical; j++)
                    {

                        grafo.Write("{ rank = same; ");
                        for (int k = 0; k < horizontal; k++)
                        {

                            grafo.Write("nodo" + nodo + "; ");
                            nodo++;
                        }

                        grafo.Write("}");

                    }

                    nodo = 1;
                    for (int j = 0; j < vertical - 1; j++)
                    {
                        grafo.Write("nodo" + nodo + "->");
                        for (int k = 0; k < horizontal; k++)
                        {
                            nodo++;
                        }
                        grafo.Write("nodo" + nodo + "; ");
                    }
                }

               else if (lista.ElementAt(y).Lexema.Equals("pixel") && lista.ElementAt(y - 1).Lexema.Equals("<"))
                {

                    for (int z = y; z < lista.Count; z++)
                    {

                        if (lista.ElementAt(z).Lexema.Equals("posicionx") && lista.ElementAt(z - 1).Lexema.Equals("/"))
                        {

                            posx = Convert.ToInt32(lista.ElementAt(z - 3).Lexema);
                           

                        }

                        else if (lista.ElementAt(z).Lexema.Equals("posiciony") && lista.ElementAt(z - 1).Lexema.Equals("/"))
                        {

                            posy = Convert.ToInt32(lista.ElementAt(z - 3).Lexema);
                            
                        }


                        else if (lista.ElementAt(z).Lexema.Equals("color") && lista.ElementAt(z - 1).Lexema.Equals("/"))
                        {
                            if (lista.ElementAt(z - 3).Lexema.Equals("verde"))
                            {
                                colorPixel = "fillcolor=green";
                            }
                            else if (lista.ElementAt(z - 3).Lexema.Equals("rojo"))
                            {
                                colorPixel = "fillcolor=red";
                            }
                            else if (lista.ElementAt(z - 3).Lexema.Equals("azul"))
                            {
                                colorPixel = "fillcolor=blue";
                            }
                            else if (lista.ElementAt(z - 3).Lexema.Equals("amarillo"))
                            {
                                colorPixel = "fillcolor=yellow";
                            }

                        }

                        else if (lista.ElementAt(z).Lexema.Equals("pixel") && lista.ElementAt(z - 1).Lexema.Equals("/"))
                        {
                            

                                nodo = 0;

                                nodo = posx + ((posy - 1) * horizontal);




                                grafo.Write("nodo" + nodo + "[" + colorPixel + "]");

                            break;
                           

                        }





                    }





                }


                



            }


            grafo.Write("}");
            grafo.Close();


        }





        public void graphviz()
        {

            string a = "dot -Tpng \"" + Directory.GetCurrentDirectory() + "\\valla.txt\" -o \"" + Directory.GetCurrentDirectory() + "\\valla.png\"";
            Process.Start("cmd.exe", "/c " + a);
            Console.WriteLine("dot -Tpng \"" + Directory.GetCurrentDirectory() + "\\valla.txt\" -o \"" + Directory.GetCurrentDirectory() + "\\valla.png\"");

        }

        private void manualDeUsuarioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Manual de Usuario.pdf");
        }

        private void manualTécnicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Manual Técnico.pdf");
        }
    }
}
