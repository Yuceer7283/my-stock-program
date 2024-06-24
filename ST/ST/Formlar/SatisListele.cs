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
    public partial class SatisListele : Form
    {
        public SatisListele()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=TeknikServis;Persist Security Info=True;User ID=sa;Password=123456");
        DataSet dt = new DataSet();
        private void Listele()
        {
            baglanti.Open();
            SqlDataAdapter cmd = new SqlDataAdapter("select * from satisTbl", baglanti);
            DataSet ds = new DataSet();
            cmd.Fill(ds, "satisTbl");
            dataGridView1.DataSource = ds.Tables["satisTbl"];
            dataGridView1.Columns[0].HeaderText = "Tc No";
            dataGridView1.Columns[1].HeaderText = "Ad Soyad";
            dataGridView1.Columns[2].HeaderText = "Telefon";
            dataGridView1.Columns[3].HeaderText = "Barkod No";
            dataGridView1.Columns[4].HeaderText = "Ürün Adı";
            dataGridView1.Columns[5].HeaderText = "Ürün Miktarı";
            dataGridView1.Columns[6].HeaderText = "Satış Fiyatı";
            dataGridView1.Columns[7].HeaderText = "Toplam Fiyatı";
            dataGridView1.Columns[8].HeaderText = "Tarih";
            baglanti.Close();
        }
        private void SatisListele_Load(object sender, EventArgs e)
        {
            Listele();
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand cmd = new SqlCommand("delete from satisTbl", baglanti);
            cmd.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Temizlendi", "Sistem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Listele();
        }
    }
}
