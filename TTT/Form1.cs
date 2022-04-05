﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace TTT
{
    public partial class Form1 : Form
    {
        //Boolean welcher angibt welcher Spieler am zug ist!
        // Spieler 1 entspricht activePlayer = false
        // Spieler 2 entspricht active Player = true
        // Spieler 1 beginnt!
        bool activePlayer = false;

        //Zähler der Spielzüge
        double spielzug = 0;

        //Gewinner String für die Gewinner MessageBox!
        String Gewinner = "";

        //Strings für Spielerfarben
        Color playerXColor = new Color();
        Color playerYColor = new Color();

        //Aktive Spielerfarbe
        Color activePlayerColor = new Color();

        //Alle Knöpfe
        ArrayList buttons;

        //Botzugriff
        Bot bot = new Bot();

        public Form1()
        {
            InitializeComponent();
            buttons = new ArrayList() { A1, A2, A3, B1, B2, B3, C1, C2, C3 };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ClearPlayground();  //Spielfeld Initialisieren und Bereitstellen!
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e) //Menüpunkt Aktion für Hilfe > About
        {
            // DialogBox mit ausgabe wer das Spiel programmiert hat.
            MessageBox.Show(this, "Dieses Spiel wurde programmiert von:\nSebastian Schindler \nDarkModz-Official\n", "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void GithubToolStripMenuItem_Click(object sender, EventArgs e)  //Menüpunkt Aktion für Hilfe > Github
        {
            //Offnet in neuem Browserfenster das Github-Profil von mir.
            System.Diagnostics.Process.Start("https://github.com/DarkModz-Official");
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)    //Menüpunkt Aktion Start > Exit
        {
            //Abfrage Ob das Spiel wirklich beendet werden soll. Wenn ja dann Spiel beenden, ansonsten weiter im Programm. 
            DialogResult exitresult = MessageBox.Show(this, "Möchten Sie das Spiel wirklich beenden?", "Bist du dir WIRKLICH sicher?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (exitresult == DialogResult.Yes)
            {
                Application.Exit(); //Spiel || Applikation beenden! 
            }
        }

        private void ButtonEnter(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Enabled)
            {
                ColorActive();
                button.BackColor = activePlayerColor;
            }
        }

        private void ButtonLeave(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            button.BackColor = activePlayerColor;
            if (button.Text == string.Empty)
            {
                button.BackColor = Color.White;
            }
        }

        private void ButtonClick(object sender, EventArgs e)
        {
            optionenToolStripMenuItem.Enabled = false;
            spielzug++;
            lbl_spielzüge.Text = "Spielzüge gespielt: " + spielzug.ToString();
            //Erstellen des Buttons als Objekt um auf den verwendeten Button zugreifen zu können. 

            Button button = (Button)sender;

            if (activePlayer)   //Abfrage welcher Spieler gerade aktiv ist
            {
                button.Text = "X";  //Button Text ändern zu X da Spieler X Button betätigt hat!
                button.BackColor = playerXColor;    // Hintergrund zu Spielerfarbe ändern
                lbl_player.Text = "Spieler O ist nun am Zug!";  //label ändern um bekannt zu geben welcher Spieler am Zug ist
            }
            else
            {
                button.Text = "O";    //Button Text ändern zu Y da Spieler Y Button betätigt hat!
                button.BackColor = playerYColor;    // Hintergrund zu Spielerfarbe ändern
                lbl_player.Text = "Spieler X ist nun am Zug!";  //label ändern um bekannt zu geben welcher Spieler am Zug ist
            }

            //Wechseln des Aktuellen Spielers
            activePlayer = !activePlayer;

            //Deaktivieren des Buttons nach Spielzug!
            button.Enabled = false; //Button deaktivieren um erneutes Betätigen zu verhindern!

            WinnerCheck();  //Überprüfen ob Spieler gewonnen hat!
        }

        private bool WinnerAvaible()
        {
            //Unötige Abfragen vermeiden
            if(spielzug < 4) return false;

            //Horizontale Abfragen

            if ((A1.Text == A2.Text) && (A2.Text == A3.Text) && (!A1.Enabled))    //erste Zeile abfragen!
            {
                return true;   //Gewinner ist bekannt!
            }
            else if ((B1.Text == B2.Text) && (B2.Text == B3.Text) && (!B1.Enabled)) //zweite Zeile abfragen!
            {
                return true;   //Gewinner ist bekannt!
            }
            else if ((C1.Text == C2.Text) && (C2.Text == C3.Text) && (!C1.Enabled))  //dritte Zeile abfragen!
            {
                return true;   //Gewinner ist bekannt!
            }

            // Vertikale Abfragen

            if ((A1.Text == B1.Text) && (B1.Text == C1.Text) && (!A1.Enabled))   //erste Spalte
            {
                return true;   //Gewinner ist bekannt!
            }
            else if ((A2.Text == B2.Text) && (B2.Text == C2.Text) && (!A2.Enabled)) //zweite Spalte
            {
                return true;   //Gewinner ist bekannt!
            }
            else if ((A3.Text == B3.Text) && (B3.Text == C3.Text) && (!A3.Enabled))    //dritte Spalte
            {
                return true;   //Gewinner ist bekannt!
            }

            //Diagonale Abfragen

            if ((A1.Text == B2.Text) && (B2.Text == C3.Text) && (!B2.Enabled))   //Diagonal von Oben Links nach unten Rechts!
            {
                return true;   //Gewinner ist bekannt!
            }
            else if ((A3.Text == B2.Text) && (B2.Text == C1.Text) && (!B2.Enabled)) //Diagonal von Oben Rechts nach unten Links!
            {
                return true;   //Gewinner ist bekannt!
            }

            return false; //Kein Gewinner ist bekannt!

        }

        private void WinnerCheck()
        {
            //Gewinner Nachricht ausgeben!
            if (WinnerAvaible())
            {
                if (activePlayer)
                    Gewinner = "O";
                else
                    Gewinner = "X";

                spielzug /= 2;
                spielzug = Math.Round(spielzug, MidpointRounding.AwayFromZero); //Berechnen wie viele Spielzüge von dem einen Spieler benötigt werden!

                DialogResult winnerRead = MessageBox.Show(this, "Spieler " + Gewinner + " hat das Spiel mit " + spielzug + " Spielzügen gewonnen!", "Gewinner", MessageBoxButtons.OK, MessageBoxIcon.Information);  //Ausgabe der MessageBox
                if (winnerRead == DialogResult.OK)
                {
                    ClearPlayground();  //Spielfeld bereinigen
                    spielzug = 0;   //Spielzüge auf 0 setzen
                }
            }
            else if (((!A1.Enabled) && (!A2.Enabled) && (!A3.Enabled)) && ((!B1.Enabled) && (!B2.Enabled) && (!B3.Enabled)) && ((!C1.Enabled) && (!C2.Enabled) && (!C3.Enabled)))   //Abfragen Ob unentschieden!
            {
                DialogResult winnerRead = MessageBox.Show(this, "Das Spiel ist Unentschieden!", "Unentschieden!", MessageBoxButtons.OK, MessageBoxIcon.Information);    //MessageBox für Unentschieden ausgeben!
                if (winnerRead == DialogResult.OK)
                {
                    ClearPlayground();  //Spielfeld bereinigen
                    spielzug = 0;   //Spielzüge auf 0 setzen
                }
            }

            //Bot logik
            if (activePlayer && bot.getEnabled) bot.choose();

            /*
             * ONLY DEBUG
             */
            String[] currentState = new String[9]; //Speichert das aktuelle Spielfeld
            int ina = 0;
            foreach (Button button in buttons)
            {
             
                if (button.Text == "X")
                    currentState[ina] = "2";
                if (button.Text == "O")
                    currentState[ina] = "1";
                if (button.Enabled) currentState[ina] = "0";
                ina++;
            }
            ina = 0;
            int ineedint = 0;
            
            textBox1.Text = currentState[0];
            for (int i = 1; i < 9; i++)
            {
                textBox1.Text += currentState[i];
                if (oldCurrentState[ina] != currentState[ina])
                {
                    ineedint = ina;
                }
                ina++;
            }
            label1.Text = "" + ineedint;
            oldCurrentState = currentState;
            if (WinnerAvaible())
            {
                if (activePlayer)
                {
                    whatToDos.Add(ineedint, textBox1.Text); SetToData();
                }
                else
                    whatToDos.Clear();
            }
            else if (((!A1.Enabled) && (!A2.Enabled) && (!A3.Enabled)) && ((!B1.Enabled) && (!B2.Enabled) && (!B3.Enabled)) && ((!C1.Enabled) && (!C2.Enabled) && (!C3.Enabled)))   //Abfragen Ob unentschieden!
            { SetToData(); }
            else if (activePlayer)
                whatToDos.Add(ineedint, textBox1.Text);
        }
        String[] oldCurrentState = new String[9]; //Speichert das aktuelle Spielfeld
        IDictionary<int, string> whatToDos = new Dictionary<int, string>();

        public void SetToData()
        {
            foreach(int id in whatToDos.Keys)
            {
                string writeText = "whatToDo.Add(\"" + whatToDos[id] + "\", " + id + ");\n";  // Create a text string
                String[] vs = File.ReadAllLines("botstrings.txt");
                foreach (String s in vs)
                {
                    if(!(writeText == s))
                        File.AppendAllText("botstrings.txt", writeText);  // Create a file and write the content of writeText to it
                }
            }
            whatToDos.Clear();
        }
        /**
         * DEBUG END
         */
        public void ClearPlayground()   //Spielfeld bereinigen
        {
            optionenToolStripMenuItem.Enabled = true;
            foreach(Button btn in buttons)
            {
                btn.Text = "";       //Button Text zurücksetzen
                btn.Enabled = true;  //Button wieder enablen!
                btn.BackColor = Color.White; //Farbe zurücksetzen
            }
        }

        private void NewGameToolStripMenuItem_Click(object sender, EventArgs e) //Aktion für MenüPunkt Start > New Game
        {
            ClearPlayground();  //Spielfeld bereinigen
            spielzug = 0;   //Spielzüge zurücksetzen
            activePlayer = false;  //Spieler zurücksetzen!
            playerXColor = Color.White; //Spielerfarbe zurücksetzen!
            playerYColor = Color.White; //Spielerfarbe zurücksetzen!
            toolStripMenuItem2.BackColor = playerXColor;
            toolStripMenuItem3.BackColor = playerYColor;
        }

        private void ColorPickerToolStripMenuItem_Click(object sender, EventArgs e) //Spielerfarbe für Spieler X
        {
            ColorDialog colorDialog = new ColorDialog();    //ColorPicker Objekt erstellen
            colorDialog.SolidColorOnly = true;              //Transparenz in Bildern verbieten
            colorDialog.ShowDialog();                       //Dialog anzeigen
            Color playerColor = colorDialog.Color;          //Farbe setzen!
            toolStripMenuItem2.BackColor = playerColor;   // Menupunkt einfärben
            playerXColor = playerColor;     //Farbe übergeben


        }

        private void SpielerfarbeÄndernToolStripMenuItem_Click(object sender, EventArgs e)  //Spielerfarbe für Spieler Y
        {
            ColorDialog colorDialog = new ColorDialog();    //ColorPicker Objekt erstellen
            colorDialog.SolidColorOnly = true;              //Transparenz in Bildern verbieten
            colorDialog.ShowDialog();                       //Dialog anzeigen
            Color playerColor = colorDialog.Color;          //Farbe setzen!
            toolStripMenuItem3.BackColor = playerColor;   // Menupunkt einfärben
            playerYColor = playerColor;     //Farbe übergeben
        }

        private void ColorActive()      //aktive Farbe abfragen!
        {
            if (activePlayer)
            {
                activePlayerColor = playerXColor;
            }
            else
                activePlayerColor = playerYColor;

        }

        private void DisableAllBotMenuItems()
        {
            offToolStripMenuItem.Checked = false;
            easyToolStripMenuItem.Checked = false;
            normalToolStripMenuItem.Checked = false;
            hardToolStripMenuItem.Checked = false;

        }

        private void offToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableAllBotMenuItems();
            offToolStripMenuItem.Checked = true;
            bot = new Bot(); // Wenn die übergabe leer ist ist der bot aus
        }

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableAllBotMenuItems();
            easyToolStripMenuItem.Checked = true;
            bot = new Bot(buttons, "easy"); // Übergabe wie stark der bot sein soll
        }

        private void normalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableAllBotMenuItems();
            normalToolStripMenuItem.Checked = true;
            bot = new Bot(buttons, "normal"); // Übergabe wie stark der bot sein soll
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DisableAllBotMenuItems();
            hardToolStripMenuItem.Checked = true;
            bot = new Bot(buttons, "hard"); // Übergabe wie stark der bot sein soll
        }
    }
}
