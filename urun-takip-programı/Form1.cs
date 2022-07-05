using System.Data.SqlClient;
using System.Data;

namespace urun_takip_programı
{
    public partial class AnaSayfa : Form
    {
        public static SqlConnection baglantı = new SqlConnection(@"Data Source=OPTER;Initial Catalog=urun_takip_programı;Integrated Security=True");

        int vakitsaniye = 0;
        int vakitdakika = 0;
        int vakitsaat = 0;
        int vakitgun = 0;

        public static DataTable verilerigoster(string veriler)
        {
            
            SqlDataAdapter SQLadapter = new SqlDataAdapter(veriler, baglantı);
            DataTable dt = new DataTable();
            SQLadapter.Fill(dt);
            return dt;

        }
        public void listeyiyenile()
        {
            veri_tablosu.DataSource = verilerigoster("select " +
                "id_numarasi as ID, " +
                "urun_kodu as Kodu, " +
                "urun_ismi as İsmi, " +
                "urun_miktari as  Miktar, " +
                "urun_yorumu as  Yorum, " +
                "urun_tarih as Tarih, " +
                "urun_alis as Alış, " +
                "urun_satis as Satış " +
                "from urun_tablosu");
        }       
        public AnaSayfa()
        {
            InitializeComponent();
            uygulama_calisma_suresi.Start();
        }

        private void uygulamayi_kapatma_butonu_Click(object sender, EventArgs e)
        {  
            string mesajicerigi = "Uygulamayı kapatmak istiyor musunuz?" + Environment.NewLine +
            "-Uygulama içeriğini kaydettiğinizden emin olun!" + Environment.NewLine +
            "-Uygulamayı kapattığınızda kaydetmediğiniz dosyaları bulamayabilirsiniz.";

            DialogResult dialog = new DialogResult();
            dialog = MessageBox.Show(mesajicerigi, "Uygulamayı kapat", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                MessageBox.Show("Uygulama kapıtılmadı");
            }
        }

        private void uygulamayi_yeniden_baslat_butonu_Click(object sender, EventArgs e)
        {
            string mesajicerigi = "Uygulamayı yeniden başlatmak istiyor musunuz?" + Environment.NewLine +
           "-Uygulama içeriğini kaydettiğinizden emin olun!" + Environment.NewLine +
           "-Uygulamayı yeniden başlattığınızda kaydetmediğiniz dosyaları bulamayabilirsiniz";

            DialogResult dialog = new DialogResult();
            dialog = MessageBox.Show(mesajicerigi, "Yeniden Başlat", MessageBoxButtons.YesNo);
            if (dialog == DialogResult.Yes)
            {
                Application.Restart();
            }
            else
            {
                MessageBox.Show("Yeniden başlatılamadı");
            }
        }

        private void uygulamayi_gizle_butonu_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        class kontrol
        {
            public string zaman_karekter(string a)
            {
                string akontrol = Convert.ToString(a);
                if (akontrol.Length == 1)
                {
                    a = "0" + akontrol;

                }
                else
                {
                    a = akontrol;
                }
                return a;
            }

            public string rakam_metin_kontrol(string metin)
            {
                if (metin.Any(c => !Char.IsDigit(c)))
                {
                    MessageBox.Show("Hatalı giriş");
                }
                return metin;
            }
        
        }
        private void uygulama_calisma_suresi_Tick(object sender, EventArgs e)
        {
            kontrol duzenle = new kontrol();
            uygulama_calisma_suresi.Interval = 1000;


            // Uygulama içi tarih ve zamanı gösteren sayaç
            int gun = Convert.ToInt32(DateTime.Now.DayOfWeek);
            string hg;
            switch (gun)
            {
                case 0:
                    hg = "Pazar";
                    break;
                case 1:
                    hg = "Pazartesi";
                    break;
                case 2:
                    hg = "Salı";
                    break;
                case 3:
                    hg = "Çarşamba";
                    break;
                case 4:
                    hg = "Perşembe";
                    break;
                case 5:
                    hg = "Cuma";
                    break;
                case 6:
                    hg = "Cumartesi";
                    break;
                default:
                    hg = "Hata Oluştu";
                    break;
            }

            string v = "Tarih: " + hg.ToString() + " " +
                            duzenle.zaman_karekter(DateTime.Now.Day.ToString()) + "." +
                            duzenle.zaman_karekter(DateTime.Now.Month.ToString()) + "." +
                            DateTime.Now.Year.ToString() + " Saat: " + DateTime.Now.ToShortTimeString();
            TarihveZaman_label.Text = v;
            // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^


            // Uygulama açık kalma süresini hesaplayan kodlar
            vakitsaniye = vakitsaniye + 1;

            if (vakitsaniye == 60)
            {
                vakitdakika = vakitdakika + 1;
                vakitsaniye = 0;
            }
            if (vakitdakika == 60)
            {
                vakitsaat = vakitsaat + 1;
                vakitdakika = 0;
            }
            if (vakitsaat == 24)
            {
                vakitgun = vakitgun + 1;
                vakitsaat = 0;
            }
            uygulama_calisma_sayaci_label.Text = Convert.ToString(duzenle.zaman_karekter(Convert.ToString(vakitgun)) + ":" +
                duzenle.zaman_karekter(Convert.ToString(vakitsaat)) + ":" +
                duzenle.zaman_karekter(Convert.ToString(vakitdakika)) + ":" +
                duzenle.zaman_karekter(Convert.ToString(vakitsaniye)));
            // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^

            // Radio butonlarını sürekli kontrol ediyor
            if (ekle_radiobutton.Checked == true)
            {
                //butonlar
                verilerin_kayidini_duzenle_button.Visible = false;
                kayit_veriyi_sil_button.Visible = false;
                verileri_kayida_ekle_button.Visible = true;
                // ürün yorum
                urun_yorum_textbox.Visible = true;
                urun_yorum_label.Visible = true;
                // Ekle grup
                Ekle_grup.Visible = true;
                guncelle_grup.Visible = false;
                // id 
                id_numarasi_textbox.Visible = false;
                id_label.Visible = false;
            }
            if (sil_radiobutton.Checked == true)
            {
                //butonlar 
                verilerin_kayidini_duzenle_button.Visible = false;
                kayit_veriyi_sil_button.Visible = true;
                verileri_kayida_ekle_button.Visible = false;
                /// ürün yorum
                urun_yorum_textbox.Visible = false;
                urun_yorum_label.Visible = false;
                // Ekle grup
                Ekle_grup.Visible = false;
                guncelle_grup.Visible = false;
                // id 
                id_numarasi_textbox.Visible = true;
                id_label.Visible = true;
                
            }
            if (guncelle_radiobutton.Checked == true)
            {
                //butonlar 
                verilerin_kayidini_duzenle_button.Visible = true;
                kayit_veriyi_sil_button.Visible = false;
                verileri_kayida_ekle_button.Visible = false;
                /// ürün yorum
                urun_yorum_textbox.Visible = true;
                urun_yorum_label.Visible = true;
                // Ekle grup
                Ekle_grup.Visible = false;
                guncelle_grup.Visible = true;
                // id 
                id_numarasi_textbox.Visible = false;
                id_label.Visible = false;
            }
                // ^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^           
        }

        private void opter_logo_MouseHover(object sender, EventArgs e)
        {
            uygulama_calisma_sayaci_label.Visible = true;
        }

        private void opter_logo_MouseLeave(object sender, EventArgs e)
        {
            uygulama_calisma_sayaci_label.Visible = false;
        }
        private void kayitli_verileri_goster_button_Click(object sender, EventArgs e)
        {
            listeyiyenile();
        }

        private void verilerin_kayidini_duzenle_button_Click(object sender, EventArgs e)
        {
            try { 
                kontrol kontrol = new kontrol();
                kontrol.rakam_metin_kontrol(Convert.ToString(id_numarasi_textbox_guncelle.Text));
                kontrol.rakam_metin_kontrol(Convert.ToString(urun_kodu_textbox_guncelle.Text));
                kontrol.rakam_metin_kontrol(Convert.ToString(urun_miktari_textbox_guncelle.Text));
                kontrol.rakam_metin_kontrol(Convert.ToString(urun_alis_textbox_guncelle.Text));
                kontrol.rakam_metin_kontrol(Convert.ToString(urun_satis_textbox_guncelle.Text));

                if (this.guncelle_grup.Controls.OfType<TextBox>().Where(f => string.IsNullOrWhiteSpace(f.Text)).Count() > 0)
                {
                    this.guncelle_grup.Controls.OfType<TextBox>().Where(f => string.IsNullOrWhiteSpace(f.Text)).ToList().ForEach(f => { MessageBox.Show(f.Name + " boş bırakılamaz"); });
                }
                else
            {
                SqlCommand guncelle = new SqlCommand("update urun_tablosu set " +
                    "urun_kodu = @urun_kodu, " +
                    "urun_ismi = @urun_ismi, " +
                    "urun_miktari = @urun_miktari, " +
                    "urun_yorumu = @urun_yorumu, " +
                    "urun_tarih = @urun_tarih, " +
                    "urun_alis = @urun_alis, " +
                    "urun_satis = @urun_satis " +
                    "where id_numarasi=@id_numarasi", baglantı);
                baglantı.Open();
                guncelle.Parameters.AddWithValue("@id_numarasi", id_numarasi_textbox_guncelle.Text);
                guncelle.Parameters.AddWithValue("@urun_kodu", urun_kodu_textbox_guncelle.Text);
                guncelle.Parameters.AddWithValue("@urun_ismi", urun_ismi_textbox_guncelle.Text);
                guncelle.Parameters.AddWithValue("@urun_miktari", urun_miktari_textbox_guncelle.Text);
                guncelle.Parameters.AddWithValue("@urun_yorumu", urun_yorum_textbox.Text);
                guncelle.Parameters.AddWithValue("@urun_tarih", urun_tarih_maskedTextBox_guncelle.Text);
                guncelle.Parameters.AddWithValue("@urun_alis", urun_alis_textbox_guncelle.Text);
                guncelle.Parameters.AddWithValue("@urun_satis", urun_satis_textbox_guncelle.Text);
                guncelle.ExecuteNonQuery();
                baglantı.Close();
                listeyiyenile();
            }
            }catch (Exception ex)
            {

            }
        }

        private void verileri_kayida_ekle_button_Click(object sender, EventArgs e)
        {
            kontrol kontrol = new kontrol();
            try { 
            if (this.Ekle_grup.Controls.OfType<TextBox>().Where(f => string.IsNullOrWhiteSpace(f.Text)).Count() > 0)
            {
                this.Ekle_grup.Controls.OfType<TextBox>().Where(f => string.IsNullOrWhiteSpace(f.Text)).ToList().ForEach(f => { MessageBox.Show(f.Name + " boş bırakılamaz"); });
            }
            else
            {
                SqlCommand ekle = new SqlCommand("insert into urun_tablosu " +
                     "(urun_kodu, urun_ismi, urun_miktari, urun_yorumu, urun_tarih, urun_alis, urun_satis) " +
                     "values " +
                     "(@urun_kodu, @urun_ismi, @urun_miktari, @urun_yorumu, @urun_tarih, @urun_alis, @urun_satis)", baglantı);
                baglantı.Open();
                ekle.Parameters.AddWithValue("@urun_kodu", kontrol.rakam_metin_kontrol(Convert.ToString(urun_kodu_textbox_ekle.Text)));
                ekle.Parameters.AddWithValue("@urun_ismi", urun_ismi_textbox_ekle.Text);
                ekle.Parameters.AddWithValue("@urun_miktari", kontrol.rakam_metin_kontrol(Convert.ToString(urun_miktar_textbox_ekle.Text)));
                ekle.Parameters.AddWithValue("@urun_yorumu", urun_yorum_textbox.Text);
                ekle.Parameters.AddWithValue("@urun_tarih", urun_tarihi_maskedTextBox_ekle.Text);
                ekle.Parameters.AddWithValue("@urun_alis", kontrol.rakam_metin_kontrol(Convert.ToString(urun_alis_textbox_ekle.Text)));
                ekle.Parameters.AddWithValue("@urun_satis", kontrol.rakam_metin_kontrol(Convert.ToString(urun_satis_textbox_ekle.Text)));
                ekle.ExecuteNonQuery();
                baglantı.Close();
                listeyiyenile();
            }
           }catch (Exception ex)
            {

            }
        }

        private void kayit_veriyi_sil_button_Click(object sender, EventArgs e)
        {
            kontrol kontrol = new kontrol();
            kontrol.rakam_metin_kontrol(Convert.ToString(id_numarasi_textbox.Text));
            try { 
            if (id_numarasi_textbox.Text == "")
            {
                MessageBox.Show("ID numarasını girmediniz. ID numarasını girin ve tekrar deneyin");
            }
            else
            {
                SqlCommand id_numarasiyla_sil = new SqlCommand("delete from  urun_tablosu  where id_numarasi=@id_numarasi", baglantı);
                baglantı.Open();
                id_numarasiyla_sil.Parameters.AddWithValue("@id_numarasi", id_numarasi_textbox.Text);

                id_numarasiyla_sil.ExecuteNonQuery();
                baglantı.Close();
                listeyiyenile();
            }
            }catch(Exception ex)
            {

            }
        }

        private void AnaSayfa_Load(object sender, EventArgs e)
        {
            ekle_radiobutton.Checked = true;
            listeyiyenile();
        }

        private void bugunun_tarihini_al_button_Click(object sender, EventArgs e)
        {
            kontrol duzenle = new kontrol();
            string tarih = Convert.ToString(DateTime.Now.Year.ToString() + "." +
                            duzenle.zaman_karekter(DateTime.Now.Month.ToString()) + "." +
                            duzenle.zaman_karekter(DateTime.Now.Day.ToString()));
            urun_tarihi_maskedTextBox_ekle.Text = tarih;
        }

        private void bugunun_tahini_al_button_guncelle_Click(object sender, EventArgs e)
        {
            kontrol duzenle = new kontrol();
            string tarih = Convert.ToString(DateTime.Now.Year.ToString() + "." +
                            duzenle.zaman_karekter(DateTime.Now.Month.ToString()) + "." +
                            duzenle.zaman_karekter(DateTime.Now.Day.ToString()));
            urun_tarih_maskedTextBox_guncelle.Text = tarih;
        }
    }
}