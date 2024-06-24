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
    public partial class MusteriListele : Form
    {
        public MusteriListele()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=TeknikServis;Persist Security Info=True;User ID=sa;Password=123456");
        int key = 0;
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
        void Listele()
        {
            string komutSatiri = "Select * from MusteriTbl"; 
            SqlDataAdapter sqa = new SqlDataAdapter(komutSatiri, baglanti); 
            DataTable dt = new DataTable(); 
            sqa.Fill(dt); 
            dgwMusteri.DataSource = dt; 
            dgwMusteri.Columns[0].HeaderText = "Müşteri ID";
            dgwMusteri.Columns[1].HeaderText = "Müşteri TC No";
            dgwMusteri.Columns[2].HeaderText = "Müşteri Ad Soyad";          
            dgwMusteri.Columns[3].HeaderText = "Müşteri Telefon";
            dgwMusteri.Columns[4].HeaderText = "Müşteri Adres";
            dgwMusteri.Columns[5].HeaderText = "Müşteri E-Posta";
        }
        private void btnSil_Click(object sender, EventArgs e)
        {
            string sorgu = "Delete From MusteriTbl Where MId=@id"; 
            SqlCommand komut = new SqlCommand(sorgu, baglanti); 
            komut.Parameters.AddWithValue("@id", dgwMusteri.CurrentRow.Cells[0].Value); 
            baglanti.Open(); 
            var uyari = MessageBox.Show("Seçtiğiniz Müşteri Kaydını Silmek İstediğinize Emin misiniz?", "Sistem", MessageBoxButtons.YesNo, MessageBoxIcon.Question); 
            if (uyari == DialogResult.Yes) 
            {
                komut.ExecuteNonQuery(); 
                MessageBox.Show("Müşteri Kaydı Başarıyla Silindi.", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information); 
            }
            else
            {

            }
            Listele(); 
            Clear(); 
            baglanti.Close(); 
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            EkleGuncel ms = new EkleGuncel(); 
            if (key == 0) 
            {
                MessageBox.Show("Güncellenecek Müşteriyi Seçiniz");
            }
            else 
            {
                try
                {
                    string sorgu = "Update MusteriTbl set MTc='" + txtTC.Text + "',MAd='" + txtAdSoyad.Text + "',MTel='" + txtTelefon.Text + "',MAdres='" + rtxtAdres.Text + "',MEposta='" + txtMail.Text + "' where MId=" + key + "";
                   
                    ms.Guncelleme(sorgu); 
                    MessageBox.Show("Müşteri Başarıyla Güncellendi");
                    Listele();
                }
                catch (Exception ex) 
                {
                    MessageBox.Show(ex.Message); 
                }
            }
        }

        private void MusteriListele_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void txtAra_TextChanged(object sender, EventArgs e)
        {
            SqlCommand komut = new SqlCommand("select * from MusteriTbl where MAd like '" + txtAra.Text + "%' or MTc like '" + txtAra.Text + "%' ", baglanti);         
            SqlDataAdapter ks = new SqlDataAdapter(komut); 
            DataTable tablo = new DataTable(); 
            ks.Fill(tablo); 
            dgwMusteri.DataSource = tablo; 
        }

        private void dgwMusteri_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTC.Text = dgwMusteri.SelectedRows[0].Cells[1].Value.ToString();
            txtAdSoyad.Text = dgwMusteri.SelectedRows[0].Cells[2].Value.ToString();
            txtTelefon.Text = dgwMusteri.SelectedRows[0].Cells[3].Value.ToString();
            rtxtAdres.Text = dgwMusteri.SelectedRows[0].Cells[4].Value.ToString();
            txtMail.Text = dgwMusteri.SelectedRows[0].Cells[5].Value.ToString();
            if (txtAdSoyad.Text == "")
            {
                key = 0;
            }
            else
            {
                key = int.Parse(dgwMusteri.SelectedRows[0].Cells[0].Value.ToString());
            }
        }
    }
}
