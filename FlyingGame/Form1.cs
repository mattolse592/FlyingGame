﻿//Matthew Olsen
//Feb 21 2024
//Basic space game
//class practice

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlyingGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ChangeScreen(this, new GameScreen());
        }

        public static void ChangeScreen(object sender, UserControl next)
        {
            Form f;

            if (sender is Form)
            {
                f = (Form)sender;  //f is sender
            }
            else
            {
                UserControl current = (UserControl)sender;  //create UserControl from sender
                f = current.FindForm();                     //find Form UserControl is on
                f.Controls.Remove(current);                 //remove current UserControl
            }

            // add the new UserControl to the middle of the screen and focus on it
            next.Location = new Point((f.ClientSize.Width - next.Width) / 2, (f.ClientSize.Height - next.Height) / 2);
            f.Controls.Add(next);
            next.Focus();
        }
    }
}
