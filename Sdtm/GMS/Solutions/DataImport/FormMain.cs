using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DataImport
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                MessageBox.Show("ttt");
            }
            
        }

        private void buttonImport_Click(object sender, EventArgs e)
        {
            //SqlConnection conn = new SqlConnection("SERVER=.;DATABASE=pubs;PWD=;UID=sa;");
            //SqlCommand cmd = new SqlCommand("SELECT*FROM [table]", cmd);
            //DataSet ds = new DataSet();
            //SqlDataAdapter adp = new SqlDataAdapter(cmd);
            //adp.Fill(ds);
        }
   
  
    }
}
