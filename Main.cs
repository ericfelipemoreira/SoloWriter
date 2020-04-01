using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SoloWriter
{
    public partial class Main : Form
    {
        private clsGeneral objGeneral;

        public Main()
        {
            InitializeComponent();
            objGeneral = new clsGeneral();
            LoadLastRoutine();
            
            LoadScreen(objGeneral.publicco);
        }

        #region Events
        private void TxtMain_TextChanged(object sender, EventArgs e)
        {
            SaveText(objGeneral.publicco);
        }

        private void TxtMain_KeyUp(object sender, KeyEventArgs e)
        {            

            if (e.KeyValue == 27)
            {
                this.Close();
            }
            else if (e.Shift && e.Control && e.KeyCode == Keys.Down)
            {
                if (this.txtMain.ForeColor == Color.White)
                {
                    this.txtMain.ForeColor = Color.Black;
                }
                else
                {
                    this.txtMain.ForeColor = Color.White;
                }
            }
            else if (e.Shift && e.Control && e.KeyCode == Keys.N)
            {
                SaveRoutine();
            }
            else if (e.Shift && e.Control && e.KeyCode == Keys.O)
            {
                OpenRoutine();
            }
            else if (e.Shift && e.Control && e.KeyCode == Keys.D1)
            {
                if (groupBox1.Visible == false)
                    groupBox1.Show();
                else
                    groupBox1.Hide();
            }
        }
        #endregion

        #region Manips
        private void SaveText(string path)
        {
            using (StreamWriter bw = new StreamWriter(File.Open(path, FileMode.Create)))
            {
                bw.Write(this.txtMain.Text, 0, this.txtMain.Text.Length);
                //this.txtMain.Select(this.txtMain.Text.Length - 1, 0);
                bw.Close();
            }

            if(this.txtMain.Text.Length > 5650)
            {
                this.txtMain.ScrollBars = ScrollBars.Vertical;
            }
            else if (this.txtMain.Text.Length <= 5650 && this.txtMain.ScrollBars == ScrollBars.Vertical)
            {
                this.txtMain.ScrollBars = ScrollBars.None;
            }
        }

        private void LoadScreen(string path)
        {
            using (StreamReader br = new StreamReader(File.OpenRead(path)))
            {
                try
                {

                    this.txtMain.Text = br.ReadToEnd();

                    br.Close();

                }
                catch (IOException)
                {
                    //the file is unavailable because it is:
                    //still being written to
                    //or being processed by another thread
                    //or does not exist (has already been processed)

                }
                finally
                {
                    if (br != null)
                        br.Close(); ;
                }
            }
            this.txtMain.SelectionStart = this.txtMain.Text.Length;
            this.txtMain.SelectionLength = 0;

        }

        private void SaveRoutine()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                objGeneral.publicco = saveFileDialog.FileName;
                FileStream fs = File.Create(objGeneral.publicco);

                fs.Close();

                this.txtMain.Text = "";
                LoadScreen(objGeneral.publicco);
            }
        }

        private void OpenRoutine()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                objGeneral.publicco = openFileDialog.FileName;

                this.txtMain.Text = "";
                LoadScreen(objGeneral.publicco);
            }
            else
            {
                objGeneral.publicco = @"A:\Workspace SoloWriter\text.txt";
            }
        }

        private void SaveLastModifiedRoutine()
        {
            if (objGeneral.publicco == "")
            {
                objGeneral.publicco = @"A:\Workspace SoloWriter\text.txt";
            }

            File.WriteAllText(@"A:\Workspace SoloWriter\lastmodified.txt", objGeneral.publicco);

        }

        private void LoadLastRoutine()
        {
            using (StreamReader br = new StreamReader(File.OpenRead(@"A:\Workspace SoloWriter\lastmodified.txt")))
            {
                try
                {

                    objGeneral.publicco = br.ReadToEnd();
                    br.Close();

                }
                catch (IOException)
                {
                    //the file is unavailable because it is:
                    //still being written to
                    //or being processed by another thread
                    //or does not exist (has already been processed)

                }
                finally
                {
                    if (br != null)
                        br.Close(); ;
                }

            }
        }
        #endregion

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveText(objGeneral.publicco);
            SaveLastModifiedRoutine();
        }

        private void label1_Click(object sender, EventArgs e)
        {

            groupBox1.Hide();
        }
    }
}
