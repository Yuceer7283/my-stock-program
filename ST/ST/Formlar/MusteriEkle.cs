using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ST
{
    public partial class MusteriEkle : Form
    {
        public MusteriEkle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=TeknikServis;Persist Security Info=True;User ID=sa;Password=123456");
        void Clear()
        {
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = string.Empty;
                }
            }
            rtxtAdres.Text = string.Empty;
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            string sorgu = "insert into MusteriTbl values('" + txtTC.Text + "','" + txtAdSoyad.Text + "','" + txtTelefon.Text + "','" + rtxtAdres.Text + "','" + txtMail.Text + "')";
            EkleGuncel ms = new EkleGuncel();
            try
            {
                ms.Ekleme(sorgu);
                MessageBox.Show("Müşteri Başarıyla Eklendi", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message); 
            }
            Clear();
        }
    }
}
