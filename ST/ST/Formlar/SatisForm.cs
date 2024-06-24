using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace ST
{
    public partial class SatısForm : Form
    {
        public SatısForm()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=TeknikServis;Persist Security Info=True;User ID=sa;Password=123456");
        void Listele()
        {
            baglanti.Open();
            SqlDataAdapter cmd = new SqlDataAdapter("select * from sepetTbl", baglanti);
            DataSet ds = new DataSet();
            cmd.Fill(ds, "sepetTbl");
            dataGridView1.DataSource = ds.Tables["sepetTbl"];
            dataGridView1.Columns[3].HeaderText = "Barkod No";
            dataGridView1.Columns[4].HeaderText = "Ürün Adı";
            dataGridView1.Columns[5].HeaderText = "Ürün Miktarı";
            dataGridView1.Columns[6].HeaderText = "Satış Fiyatı";
            dataGridView1.Columns[7].HeaderText = "Toplam Fiyatı";
            dataGridView1.Columns[8].HeaderText = "Tarih";
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[2].Visible = false;
            baglanti.Close();

        }
        private void Anasayfa_Load(object sender, EventArgs e)
        {
            hesapla();
            Listele();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MusteriEkle ms = new MusteriEkle();
            ms.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MusteriListele ml = new MusteriListele();
            ml.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UrunEkle uk = new UrunEkle();
            uk.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MarkaEkle mk = new MarkaEkle();
            mk.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            KategoriEkle ke = new KategoriEkle();
            ke.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            UrunListele ul = new UrunListele();
            ul.ShowDialog();
        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {
            if (txtTc.Text == "")
            {
                txtAdSoyad.Text = "";
                txtTelNo.Text = "";
            }
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("select * from MusteriTbl where MTc like '" + txtTc.Text + "'", baglanti);
            SqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                txtAdSoyad.Text = read["MAd"].ToString();
                txtTelNo.Text = read["MTel"].ToString();
            }
            baglanti.Close();
        }

        private void txtBarkod_TextChanged(object sender, EventArgs e)
        {
            Clear();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from UrunTbl where barkodno like '" + txtBarkod.Text + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtUrunAd.Text = read["urunadi"].ToString();
                txtSatis.Text = read["satisfiyati"].ToString();
            }
            baglanti.Close();
          
        }

        private void Clear()
        {
            if (txtBarkod.Text == "")
            {
                foreach (Control item in groupBox2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktar)
                        {
                            item.Text = "";
                        }
                    }

                }
            }
        }

        private void Anasayfa_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Multiply)
            {
                txtMiktar.Text = txtBarkod.Text.Substring(txtBarkod.Text.Length - 1);
                txtBarkod.Text = "";
            }
        }
        bool durum = true;
        void barkodKontrol()
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("select * from sepetTbl",baglanti);
            SqlDataReader read = cmd.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkod.Text == read["barkodno"].ToString())
                  {
                    durum = false;
                  }
            }
            baglanti.Close();
        }
        private void btnEkle_Click(object sender, EventArgs e)
        {
            barkodKontrol();
            if (durum == true)
            {
                string sorgu = "insert into sepetTbl values('" + txtTc.Text + "','" + txtAdSoyad.Text + "','" + txtTelNo.Text + "','" + txtBarkod.Text + "','" + txtUrunAd.Text + "','" + int.Parse(txtMiktar.Text) + "','" + txtSatis.Text + "','" + int.Parse(txtToplamF.Text) + "','" + DateTime.Now.ToString() + "')";
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
                hesapla();
                Listele();
            }
            else
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("update sepetTbl set miktari=miktari+'" + int.Parse(txtMiktar.Text) + "' where barkodno='" + txtBarkod.Text + "'", baglanti);
                komut.ExecuteNonQuery();
                SqlCommand komut2 = new SqlCommand("update sepetTbl set toplamfiyati=miktari*satisfiyati where barkodno='"+txtBarkod.Text+"'",baglanti);
                komut2.ExecuteNonQuery();
                baglanti.Close();
                hesapla();
                Listele();
                
            }
            txtMiktar.Text = "1";
            foreach (Control item in groupBox2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMiktar)
                    {
                        item.Text = "";
                    }
                }
            }
        }
       
        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamF.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatis.Text)).ToString();
            }
            catch
            {
                MessageBox.Show("Lütfen Miktarı Silmeyiniz.","Sistem",MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMiktar.Text = "1";
            }
        }

        private void txtSatis_TextChanged(object sender, EventArgs e)
        {
            txtToplamF.Text = "";
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            string sorgu = "Delete From sepetTbl Where barkodno=@id";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@id", dataGridView1.CurrentRow.Cells[3].Value);
            baglanti.Open();
            var uyari = MessageBox.Show("Seçtiğiniz Ürünü Sepetten Silmek İstediğinize Emin misiniz?", "Sistem", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (uyari == DialogResult.Yes)
            {
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ürün Sepetten Başarıyla Silindi.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

            }
            hesapla();
            Listele();
            Clear();
            
        }
        void hesapla()
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select sum(toplamfiyati) from sepetTbl",baglanti);
                lblGenel.Text = komut.ExecuteScalar() + " TL";
                baglanti.Close();

            }
            catch (Exception)
            {

                ;
            }
        }
        private void btnİptal_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("delete from sepetTbl",baglanti);
            cmd.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ürünler Sepetten Çıkarıldı", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            hesapla();
            Listele();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SatisListele sl = new SatisListele();
            sl.ShowDialog();
        }

        private void btnSat_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++) 
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into satisTbl(tc,adsoyad,telefon,barkodno,urunadi,miktari,satisfiyati,toplamfiyati,tarih) values(@tc,@adsoyad,@telefon,@barkodno,@urunadi,@miktari,@satisfiyati,@toplamfiyati,@tarih)",baglanti);
                komut.Parameters.AddWithValue("@tc", txtTc.Text);
                komut.Parameters.AddWithValue("@adsoyad", txtAdSoyad.Text);
                komut.Parameters.AddWithValue("@telefon", txtTelNo.Text);
                komut.Parameters.AddWithValue("@barkodno", dataGridView1.Rows[i].Cells["barkodno"].Value.ToString());
                komut.Parameters.AddWithValue("@urunadi", dataGridView1.Rows[i].Cells["urunadi"].Value.ToString());
                komut.Parameters.AddWithValue("@miktari", int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()));
                komut.Parameters.AddWithValue("@satisfiyati", dataGridView1.Rows[i].Cells["satisfiyati"].Value.ToString());
                komut.Parameters.AddWithValue("@toplamfiyati", dataGridView1.Rows[i].Cells["toplamfiyati"].Value.ToString());
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                SqlCommand komut2 = new SqlCommand("update UrunTbl set miktari=miktari-'" + int.Parse(dataGridView1.Rows[i].Cells["miktari"].Value.ToString()) + "' where barkodno='" + dataGridView1.Rows[i].Cells["barkodno"].Value.ToString() + "'", baglanti);
                komut2.ExecuteNonQuery();
                baglanti.Close();
            }
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("delete from sepetTbl", baglanti);
            cmd.ExecuteNonQuery();
            baglanti.Close();
            hesapla();
            Listele();
        }
    }
}
