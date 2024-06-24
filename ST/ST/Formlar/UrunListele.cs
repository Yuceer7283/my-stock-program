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
    public partial class UrunListele : Form
    {
        public UrunListele()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=TeknikServis;Persist Security Info=True;User ID=sa;Password=123456");
        void Listele()
        {
            string komutSatiri = "Select * from UrunTbl";
            SqlDataAdapter sqa = new SqlDataAdapter(komutSatiri, baglanti);
            DataTable dt = new DataTable();
            sqa.Fill(dt);
            dgwUrun.DataSource = dt;
            dgwUrun.Columns[0].HeaderText = "Ürün ID";
            dgwUrun.Columns[1].HeaderText = "Barkod No";
            dgwUrun.Columns[2].HeaderText = "Kategori İsmi";
            dgwUrun.Columns[3].HeaderText = "Marka İsmi";
            dgwUrun.Columns[4].HeaderText = "Ürün Adı";
            dgwUrun.Columns[5].HeaderText = "Ürün Miktarı";
            dgwUrun.Columns[6].HeaderText = "Alış Fiyatı";
            dgwUrun.Columns[7].HeaderText = "Satış Fiyatı";
            dgwUrun.Columns[8].HeaderText = "Tarih";
        }
        void kategoriGetir()
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
        private void UrunListele_Load(object sender, EventArgs e)
        {
            Listele();
            kategoriGetir();
        }
        int key = 0;
        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            EkleGuncel ms = new EkleGuncel();
            if (key == 0)
            {
                MessageBox.Show("Güncellenecek Ürünü Seçiniz");
            }
            else
            {
                try
                {
                    string sorgu = "Update UrunTbl set barkodno='" + txtVBarkod.Text + "',kategori='" + cmbKategori.Text + "',marka='" + cmdMarka.Text + "',urunadi='" + txtVUrunAdi.Text + "',miktari='" + txtVMiktar.Text + "',alisfiyati='"+txtVAlis.Text+"',satisfiyati='"+txtVSatis.Text+"' where urunID=" + key + "";

                    ms.Guncelleme(sorgu);
                    MessageBox.Show("Ürün Başarıyla Güncellendi");
                    Listele();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        void Clear()
        {
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
                if (item is ComboBox)
                {
                    item.Text="";
                }
            }
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            string sorgu = "Delete From UrunTbl Where urunID=@id";
            SqlCommand komut = new SqlCommand(sorgu, baglanti);
            komut.Parameters.AddWithValue("@id", dgwUrun.CurrentRow.Cells[0].Value);
            baglanti.Open();
            var uyari = MessageBox.Show("Seçtiğiniz Ürün Kaydını Silmek İstediğinize Emin misiniz?", "Sistem", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (uyari == DialogResult.Yes)
            {
                komut.ExecuteNonQuery();
                MessageBox.Show("Ürün Kaydı Başarıyla Silindi.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {

            }
            Listele();
            Clear();
            baglanti.Close();
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("select * from UrunTbl where barkodno like '" + txtAra.Text + "%' or marka like '" + txtAra.Text + "%' ", baglanti);
            SqlDataAdapter ks = new SqlDataAdapter(komut);
            DataTable tablo = new DataTable();
            ks.Fill(tablo);
            dgwUrun.DataSource = tablo;
        }

        private void dgwUrun_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtVBarkod.Text = dgwUrun.SelectedRows[0].Cells[1].Value.ToString();
            cmbKategori.Text = dgwUrun.SelectedRows[0].Cells[2].Value.ToString();
            cmdMarka.Text = dgwUrun.SelectedRows[0].Cells[3].Value.ToString();
            txtVUrunAdi.Text = dgwUrun.SelectedRows[0].Cells[4].Value.ToString();
            txtVMiktar.Text = dgwUrun.SelectedRows[0].Cells[5].Value.ToString();
            txtVAlis.Text = dgwUrun.SelectedRows[0].Cells[6].Value.ToString();
            txtVSatis.Text = dgwUrun.SelectedRows[0].Cells[7].Value.ToString();
            if (txtVBarkod.Text == "")
            {
                key = 0;
            }
            else
            {
                key = int.Parse(dgwUrun.SelectedRows[0].Cells[0].Value.ToString());
            }
        }

        private void cmbKategori_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdMarka.Items.Clear();
            cmdMarka.SelectedIndex = -1;
            baglanti.Open();
            SqlCommand sqm = new SqlCommand("select * from markaBilgi where kategori='" + cmbKategori.SelectedItem + "'", baglanti);
            SqlDataReader read = sqm.ExecuteReader();
            while (read.Read())
            {
                cmdMarka.Items.Add(read["marka"].ToString());
            }
            baglanti.Close();
        }
    }
}
