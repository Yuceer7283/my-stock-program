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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ST
{
    public partial class UrunEkle : Form
    {
        public UrunEkle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=TeknikServis;Persist Security Info=True;User ID=sa;Password=123456");
        bool durum;
        void barkodKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from UrunTbl", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkod.Text == read["barkodno"].ToString() || txtBarkod.Text == "") 
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }
        void Clear()
        {
            foreach (Control item in groupBox1.Controls)
            {
                if (item is System.Windows.Forms.TextBox)
                {
                    item.Text = "";
                }
                if (item is System.Windows.Forms.ComboBox)
                {
                    item.Text = "";
                }
            }
        }
        private void UrunEkle_Load(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from kategoriBilgi", baglanti);
            SqlDataReader oku = komut.ExecuteReader();
            while (oku.Read())
            {
                cmbKategori.Items.Add(oku["kategori"]);
            }
            baglanti.Close();
        }

        private void cmbKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdMarka.Items.Clear();
            cmdMarka.SelectedIndex = -1;
            baglanti.Open();
            SqlCommand sqm = new SqlCommand("select * from markaBilgi where kategori='"+cmbKategori.SelectedItem+"'", baglanti);
            SqlDataReader read = sqm.ExecuteReader();
            while (read.Read())
            {
                cmdMarka.Items.Add(read["marka"].ToString());
            }
            baglanti.Close();
        }

        private void btnYeniEkle_Click(object sender, EventArgs e)
        {
            barkodKontrol();
            if (durum == true)
            {
                string sorgu = "insert into UrunTbl values('" + txtBarkod.Text + "','" + cmbKategori.Text + "','" + cmdMarka.Text + "','" + txtUrunAdi.Text + "','" + int.Parse(txtMiktar.Text) + "','" + txtAFiyat.Text + "','" + txtSFiyat.Text + "','" + DateTime.Now.ToString() + "')";
                EkleGuncel ms = new EkleGuncel();
                try
                {
                    ms.Ekleme(sorgu);
                    MessageBox.Show("Ürün Başarıyla Eklendi", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                cmdMarka.Items.Clear();
                Clear();
            }
            else
            {
                MessageBox.Show("Böyle Bir Barkod Var veya Barkod Girilmedi.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtVBarkod_TextChanged(object sender, EventArgs e)
        {
            if (txtVBarkod.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is System.Windows.Forms.TextBox)
                    {
                        item.Text = "";
                    }
                }
            }
            lblMiktar.Text = "";
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from UrunTbl where barkodno like '" + txtVBarkod.Text + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtVKategori.Text = read["kategori"].ToString();
                txtVMarka.Text = read["marka"].ToString();
                txtVUrunAdi.Text = read["urunadi"].ToString();
                lblMiktar.Text = read["miktari"].ToString();
                txtVAlis.Text = read["alisfiyati"].ToString();
                txtVSatis.Text = read["satisfiyati"].ToString();
            }
            baglanti.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            baglanti.Open();
            SqlCommand komut = new SqlCommand("update UrunTbl set miktari=miktari+'" + int.Parse(txtVMiktar.Text) + "' where barkodno='"+txtVBarkod.Text+"'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close(); 
            foreach (Control item in groupBox2.Controls)
            {
                if (item is System.Windows.Forms.TextBox)
                {
                    item.Text = "";
                }
            }
            MessageBox.Show("Ürünün Miktarina Ekleme Yapıldı","Sistem",MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
