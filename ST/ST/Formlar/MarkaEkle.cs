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
    public partial class MarkaEkle : Form
    {
        public MarkaEkle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=TeknikServis;Persist Security Info=True;User ID=sa;Password=123456");
        bool durum;
        void markaEngelle()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from markaBilgi", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (comboBox1.Text==read["kategori"].ToString() && textBox1.Text == read["marka"].ToString() || comboBox1.Text== "" || textBox1.Text == "")
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            markaEngelle();
            if (durum == true)
            {
                string sorgu = "insert into markaBilgi values('" + comboBox1.Text + "','" + textBox1.Text + "')";
                EkleGuncel ms = new EkleGuncel();
                try
                {
                    ms.Ekleme(sorgu);
                    MessageBox.Show("Marka Başarıyla Eklendi", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                textBox1.Text = "";
                comboBox1.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Böyle Bir Marka Var veya Marka Girilmedi.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MarkaEkle_Load(object sender, EventArgs e)
        {
            baglanti.Open(); 
            SqlCommand komut = new SqlCommand("select * from kategoriBilgi", baglanti); 
            SqlDataReader oku = komut.ExecuteReader(); 
            while (oku.Read()) 
            {
                comboBox1.Items.Add(oku["kategori"]); 
            }
            baglanti.Close();   
        }
    }
}
