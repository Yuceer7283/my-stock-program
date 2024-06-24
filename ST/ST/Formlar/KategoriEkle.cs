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
    public partial class KategoriEkle : Form
    {
        public KategoriEkle()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=TeknikServis;Persist Security Info=True;User ID=sa;Password=123456");
        bool durum;
        void kategoriEngelle()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from kategoriBilgi", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (textBox1.Text == read["kategori"].ToString() || textBox1.Text == "")
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            kategoriEngelle();
            if (durum == true)
            {
                string sorgu = "insert into kategoriBilgi values('" + textBox1.Text + "')";
                EkleGuncel ms = new EkleGuncel();
                try
                {
                    ms.Ekleme(sorgu);
                    MessageBox.Show("Kategori Başarıyla Eklendi", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                textBox1.Text = "";
            }
            else
            {
                MessageBox.Show("Böyle Bir Kategori Var veya Kategori Girilmedi.","Sistem",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
;
        }
    }
}
