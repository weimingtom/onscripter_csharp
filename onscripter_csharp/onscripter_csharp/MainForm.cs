﻿/*
 * Created by SharpDevelop.
 * User: wmt
 * Date: 2021-6-20
 * Time: 8:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace onscripter_csharp
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
			this.FormBorderStyle = FormBorderStyle.FixedDialog;
			this.Size = new Size(640, 480);
			this.MaximizeBox = false;
			this.Size = new Size(640, 480 + (480 - this.ClientRectangle.Height));
			this.CenterToScreen();
		}
		
		
		protected override void OnPaint(PaintEventArgs e)
        {
            Bitmap bufferBmp = new Bitmap(this.ClientRectangle.Width, this.ClientRectangle.Height);
            Graphics g = Graphics.FromImage((System.Drawing.Image)bufferBmp);
            this.DrawGame(g);

            e.Graphics.DrawImage(bufferBmp, 0, 0);
            g.Dispose();
            base.OnPaint(e);
        }
		
		private Brush bgBrush = new SolidBrush(Color.Blue);
        private void DrawGame(Graphics g)
        {
        	g.FillRectangle(bgBrush, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height);
        }
        
        public void refresh() {
        	this.Invalidate();
        }
        
        void MainFormKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
        	Debug.WriteLine("MainFormKeyDown " + e.KeyCode);
		}
		
		void MainFormKeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			Debug.WriteLine("MainFormKeyUp " + e.KeyCode);
		}
		
		void MainFormMouseClick(object sender, MouseEventArgs e)
		{
			Debug.WriteLine("MainFormMouseClick " + e.X + ", " + e.Y);
		}
	}
}
