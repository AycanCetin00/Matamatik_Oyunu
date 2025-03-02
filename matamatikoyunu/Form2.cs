using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace matamatikoyunu
{

    
    public partial class Form2 : Form
    {

        private Form1 anaForm; // Form1'e referans

        private int seviye; // Seviye numarası
        private int dogruSayisi = 0; // Doğru cevap sayısı
        private int pasSayisi = 0; // Pas geçme sayısı
                                   // private List<Tuple<string, int>> sorular = new List<Tuple<string, int>>(); // Soruları ve doğru cevapları tutar
        List<Tuple<string, int, int>> sorular = new List<Tuple<string, int, int>>();

        private int soruSayaci = 0; // Soruların hangi bölümde olduğunu tutar
        private int toplamSoru = 20; // Her seviyede toplam 20 soru
        private HashSet<string> sorularSeti = new HashSet<string>(); // Tekrarlanan soruları engellemek için
        int[] kullaniciCevaplar = new int[5];
        private int sure = 120;

        //dosya yolları 
        private string kayitFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "kayit.txt");
        private string skorFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "skor.txt");
        private int mevcutSeviye = 1;  // Örnek başlangıç seviyesi
        private int mevcutSkor = 0;    // Örnek başlangıç skoru
        private int mevcutPuan = 0; // Mevcut puanı tutar


        public Form2(int seviye, Form1 form1) // Form1 referansını alıyoruz
        {

            InitializeComponent();
            this.anaForm = form1; // Referansı saklıyoruz

            this.seviye = seviye; // Seviye bilgisini alıyoruz
            SoruOlustur();
            // Seviye bazlı süre ataması
            switch (seviye)
            {
                case 1:
                    sure = 80; // Seviye 1 için 120 saniye
                    break;
                case 2:
                    sure = 100; // Seviye 2 için 100 saniye
                    break;
                case 3:
                    sure = 140; // Seviye 3 için 80 saniye
                    break;
                case 4:
                    sure = 180; // Seviye 4 için 60 saniye
                    break;
                case 5:
                    sure = 220; // Seviye 5 için 45 saniye
                    break;
                default:
                    sure = 270; // Varsayılan süre
                    break;
            }
            lblpuan.Text = $" {mevcutPuan}"; // Mevcut puanı labelda göster



        }

        // Soru bitişi ve seviye tamamlama
        private void SoruTamamlandı()
        {
            dogruSayisi++; // Doğru sayısını artır
            if (dogruSayisi >= 10) // Örnek olarak 10 doğru cevap gerektiği varsayalım
            {
                timer1.Stop();
                anaForm.SeviyeTamamlandı(seviye); // Form1'e seviye tamamlandığını bildir
                this.Close(); // Formu kapat
            }
        }

        // 20 soru oluşturma
        private void SoruOlustur()
        {
            Random rnd = new Random();

            for (int i = 0; i < toplamSoru; i++)
            {
                int sayi1 = 0, sayi2 = 0;

                // Seviye bazlı sayı aralıkları
                switch (seviye)
                {
                    case 1: // Tek basamaklı sayılar
                        sayi1 = rnd.Next(1, 10);
                        sayi2 = rnd.Next(1, 10);
                        break;

                    case 2: // İki basamaklı ve bir basamaklı sayılar
                        sayi1 = rnd.Next(10, 100); // İki basamaklı
                        sayi2 = rnd.Next(1, 10); // Tek basamaklı
                        break;

                    case 3: // İki basamaklı sayılar
                        sayi1 = rnd.Next(10, 100);
                        sayi2 = rnd.Next(10, 100);
                        break;

                    case 4: // Üç basamaklı ve bir basamaklı sayılar
                        sayi1 = rnd.Next(100, 1000); // Üç basamaklı
                        sayi2 = rnd.Next(1, 10); // Tek basamaklı
                        break;

                    case 5: // Üç basamaklı sayılar
                        sayi1 = rnd.Next(100, 1000);
                        sayi2 = rnd.Next(10, 100); // İki basamaklı
                        break;

                    default:
                        // Geçersiz seviye
                        MessageBox.Show("Geçersiz seviye!");
                        return;
                }

                string islem = "";
                int sonuc = 0;
                int zorluk = rnd.Next(1, 4); // 1 = kolay, 2 = orta, 3 = zor

                // İşlem türünü rastgele belirleme
                switch (rnd.Next(1, 5))
                {
                    case 1: // Toplama işlemi
                        islem = "+";
                        sonuc = sayi1 + sayi2;
                        break;

                    case 2: // Çıkarma işlemi
                        islem = "-";
                        if (sayi1 < sayi2) // Negatif sonuçlardan kaçınma
                        {
                            int temp = sayi1;
                            sayi1 = sayi2;
                            sayi2 = temp;
                        }
                        sonuc = sayi1 - sayi2;
                        break;

                    case 3: // Çarpma işlemi
                        islem = "*";
                        sonuc = sayi1 * sayi2;
                        break;

                    case 4: // Bölme işlemi
                        islem = "/";
                        if (sayi2 == 0) sayi2 = 1; // Bölme hatalarını önleme
                        while (sayi1 % sayi2 != 0) // Tam bölünmeyen sonuçlardan kaçınma
                        {
                            sayi2 = rnd.Next(1, 10 * seviye);
                        }
                        sonuc = sayi1 / sayi2;
                        break;

                }
                // Soruyu string formatında oluştur
                string soruMetni = $"{sayi1} {islem} {sayi2}";

                // Eğer aynı soru daha önce oluşmuşsa tekrar et
                if (!sorularSeti.Contains(soruMetni))
                {
                    sorularSeti.Add(soruMetni);
                    sorular.Add(new Tuple<string, int, int>($"{soruMetni} = ?", sonuc, zorluk)); // Zorluk derecesi eklendi
                }
            }
        }


        // Soruları 5'er 5'er gösterme
        private void SorulariGoster()
        {
            // Eğer 20 soruyu tamamladıysak
            if (soruSayaci >= toplamSoru)
            {
               // MessageBox.Show($"Tebrikler! Toplam doğru sayınız: {dogruSayisi}");
                SeviyeSonucuGoster(); // Burada sonuçları gösteriyoruz

                return;
            }

            // 5 soru göstereceğiz, o yüzden soruları 5'er 5'er alalım
            lblsoru1.Text = sorular[soruSayaci].Item1;
            lblsoru2.Text = sorular[soruSayaci + 1].Item1;
            lblsoru3.Text = sorular[soruSayaci + 2].Item1;
            lblsoru4.Text = sorular[soruSayaci + 3].Item1;
            lblsoru5.Text = sorular[soruSayaci + 4].Item1;

            // Cevap kutularını temizleyelim
            txtCevap1.Clear();
            txtCevap2.Clear();
            txtCevap3.Clear();
            txtCevap4.Clear();
            txtCevap5.Clear();


        }

        private void SeviyeSonucuGoster()
        {
            int dogruCevapSayisi = dogruSayisi; // Toplam doğru cevap sayısı

            string yildizlar = "";

            if (dogruCevapSayisi >= 19)
                yildizlar = "★★★";
            else if (dogruCevapSayisi >= 16)
                yildizlar = "★★";
            else if (dogruCevapSayisi >= 11)
                yildizlar = "★";
            else
                yildizlar = "Maalesef yıldız yok, tekrar deneyin.";

            MessageBox.Show($"Tebrikler! Doğru Sayısı: {dogruCevapSayisi}\nYıldızlar: {yildizlar}");

            // Seviye geçme kontrolü
            if (dogruCevapSayisi >= 11)
            {
                MessageBox.Show("Sonraki seviyeye geçiyorsunuz!");
                SoruTamamlandı();
                // Bir sonraki seviyeye geçiş işlemi burada yapılabilir
            }
            else
            {
                MessageBox.Show("Maalesef seviyeyi geçemediniz.");
            }
            timer1.Stop(); // Timer'ı durdur

            // Oyun bitince formu kapat ve ana menüyü göster
            this.Close(); // Form2'yi kapat

        }
        int toplamPuan = 0;
        // Cevapları kontrol ederken zorluk derecesine göre puan verelim
        private void CevaplariKontrolEt()
        {

            for (int i = 0; i < 5; i++)
            {
                if (kullaniciCevaplar[i] == sorular[soruSayaci + i].Item2)
                {
                    dogruSayisi++;
                }
            }

            // Burada doğru sayısını kontrol et
            if (dogruSayisi >= 10)
            {
                timer1.Stop(); // Timer'ı durdur

                SoruTamamlandı(); // Seviye tamamlandıysa metodu çağır

            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            OyunuYukle();

            //SoruOlustur(); // 20 soruyu oluştur
            SorulariGoster(); // İlk 5 soruyu göster

            Timer timer = new Timer();
            timer.Interval = 1000; // Her saniye bir defa çalışacak
            timer.Tick += timer1_Tick;
            timer.Start();
        }

        private void btnDevam_Click(object sender, EventArgs e)
        {
            // Kullanıcı cevaplarını alalım ve kontrol edelim
            if (!int.TryParse(txtCevap1.Text, out kullaniciCevaplar[0]) ||
                !int.TryParse(txtCevap2.Text, out kullaniciCevaplar[1]) ||
                !int.TryParse(txtCevap3.Text, out kullaniciCevaplar[2]) ||
                !int.TryParse(txtCevap4.Text, out kullaniciCevaplar[3]) ||
                !int.TryParse(txtCevap5.Text, out kullaniciCevaplar[4]))
            {
                MessageBox.Show("Lütfen tüm kutulara geçerli sayılar girin!");
                return;
            }

            // Cevapları kontrol edelim ve puanı hesaplayalım
            for (int i = 0; i < 5; i++)
            {

                if (kullaniciCevaplar[i] == sorular[soruSayaci + i].Item2)
                {
                    dogruSayisi++; // Doğru cevap sayısını artır
                    toplamPuan = dogruSayisi * seviye;
                }
            }

            // Mevcut puanı güncelle
            mevcutPuan = toplamPuan;

            // Puanı etiket üzerinde göster
            lblToplamPuan.Text = $"Puan: {mevcutPuan}";
            lblpuan.Text = $"{dogruSayisi}";
            // Soru sayacını 5 artır
            soruSayaci += 5;

            


            // Yeni soruları göster
            SorulariGoster();




        }

        private void btnPas_Click(object sender, EventArgs e)
        {
            pasSayisi++;
            if (pasSayisi >= 2)
            {
                MessageBox.Show("İkinci pas hakkınız yanlıştır!");
            }

            // Soru sayacını 5 artır
            soruSayaci += 5;

            // Yeni soruları göster
            SorulariGoster();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void SkoruDosyayaKaydet()
        {
            using (StreamWriter sw = new StreamWriter(skorFilePath, false)) // "false" ekledim çünkü eski bilgileri yenisiyle değiştirmek istiyoruz
            {
                sw.WriteLine($"Seviye: {seviye}");
                sw.WriteLine($"Puan: {toplamPuan}");
                sw.WriteLine($"Süre: {sure}");
                sw.WriteLine($"Doğru Cevap: {dogruSayisi}");
                sw.WriteLine($"Tarih: {DateTime.Now}");
            }
        }


        private void OyunuKaydet()
        {
            using (StreamWriter sw = new StreamWriter(kayitFilePath))
            {
                // Kayıt dosyasına seviye bilgisi yazma
                sw.WriteLine(mevcutSeviye.ToString()); // Mevcut seviyeyi yaz

                // Skor bilgisi yazma
                sw.WriteLine(mevcutSkor.ToString()); // Mevcut skoru yaz
            timer1.Stop();
            }



        }

        private void OyunuYukle()
        {
            // Eğer kayıt dosyası mevcutsa, seviye ve skoru dosyadan yükle
            if (File.Exists(kayitFilePath))
            {
                using (StreamReader sr = new StreamReader(kayitFilePath))
                {
                    string seviyeStr = sr.ReadLine(); // İlk satır seviye
                    string skorStr = sr.ReadLine();   // İkinci satır skor

                    int seviye;
                    if (int.TryParse(seviyeStr, out seviye))
                    {
                        mevcutSeviye = seviye;  // Seviye bilgisini mevcut seviye değişkenine ata
                    }

                    int skor;
                    if (int.TryParse(skorStr, out skor))
                    {
                        mevcutSkor = skor;  // Skor bilgisini mevcut skor değişkenine ata
                    }
                }

            }

            if (File.Exists(skorFilePath))
            {
                using (StreamReader sr = new StreamReader(skorFilePath))
                {
                    string skorStr = sr.ReadLine();
                    int skor;
                    if (int.TryParse(skorStr, out skor))
                    {
                        mevcutSkor = skor;  // Skor bilgisini mevcut skor değişkenine ata
                    }
                }
            }




        }

        private bool sureDoldu = false; // Süre doldu mu kontrolü için
        private void timer1_Tick(object sender, EventArgs e)
        {
            // Süreyi her tickte bir azalt
            sure--;

            // Kalan süreyi ekrandaki label'a yaz
            lblsure.Text = $" {sure} saniye";

            // Eğer süre 0'a ulaştıysa işlemleri durdur
            if (sure == 0)
            {
                timer1.Stop(); // Timer'ı durdur
                lblsure.Text = "Süre doldu!"; // Label'a süre doldu yazısı yaz
                MessageBox.Show("Süre doldu!"); // Mesaj kutusu göster
                SeviyeSonucuGoster(); // Süre dolunca seviye sonuçlarını göster
            }
        }

        private void btncıkıs_Click(object sender, EventArgs e)
        {
            // Oyunu kaydedelim
            OyunuKaydet();
            // Uygulamadan çıkalım
            this.Close();
        }

        private void txtCevap2_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSureUzat_Click(object sender, EventArgs e)
        {
            

        }
    }
}
