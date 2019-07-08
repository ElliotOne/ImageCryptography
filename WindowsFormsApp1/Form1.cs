using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            errorProvider1.Clear();
            if (String.IsNullOrEmpty(txtFirstname.Text))
            {
                errorProvider1.SetError(txtFirstname, "Enter firstname");
            }
            else if (String.IsNullOrEmpty(txtLastname.Text))
            {
                errorProvider1.SetError(txtLastname, "Enter lastname");
            }
            else if (!rdbMale.Checked && !rdbFemale.Checked)
            {
                errorProvider1.SetError(groupBox1, "Select gender");
            }
            else if (String.IsNullOrEmpty(txtDescription.Text))
            {
                errorProvider1.SetError(txtDescription, "Enter description");
            }
            else if (pictureBox1.Image == null)
            {
                errorProvider1.SetError(pictureBox1, "Choose picture");
            }
            else
            {

                string gender = String.Empty;
                if (rdbMale.Checked)
                {
                    gender = "Male";
                }
                else
                {
                    gender = "Female";
                }

                Bitmap img = new Bitmap(pictureBox1.Image);

                string text = txtFirstname.Text + "*" + txtLastname.Text + "*" +
                              gender + "*" + txtDescription.Text;

                Bitmap bitmap = SteganographyHelper.embedText(text, img);
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    bitmap.Save(saveFileDialog1.FileName, ImageFormat.Png);
                }

                pictureBox1.Image = null;
                txtFirstname.Clear();
                txtLastname.Clear();
                rdbMale.Checked = false;
                rdbFemale.Checked = false;
                txtDescription.Clear();
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {

                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                Bitmap bitmap = new Bitmap(fs);
                string data = SteganographyHelper.extractText(bitmap);
                string[] output = data.Split('*');
                txtFirstname.Text = output[0];
                txtLastname.Text = output[1];
                string gender = output[2] == "Male" ? "Male" : "Woman";
                if (gender == "Male")
                {
                    rdbMale.Checked = true;
                }
                else
                {
                    rdbFemale.Checked = true;
                }

                txtDescription.Text = output[3];
                pictureBox1.Image = Image.FromStream(fs);
                fs.Close();
            }
            
        }

        private void BtnLoadPicture_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read, FileShare.None);
                pictureBox1.Image = Image.FromStream(fs);
                fs.Close();
            }
        }
    }
}
