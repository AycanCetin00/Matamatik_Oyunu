using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace matamatikoyunu
{

    public partial class Form1 : Form
    {
        private int tamamlananSeviye = 1; // Başlangıçta 1. seviye tamamlanmış sayılır.

        public Form1()
        {
            InitializeComponent();
            GüncelleButonlar(); // Form yüklendiğinde butonları güncelle
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 seviyeForm = new Form2(1, this); // Seviye 1
            seviyeForm.Show(); // Yeni formu aç
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tamamlananSeviye >= 2)
            {
                Form2 seviyeForm = new Form2(2, this); // Seviye 2
                seviyeForm.Show(); // Yeni formu aç
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (tamamlananSeviye >= 3)
            {
                Form2 seviyeForm = new Form2(3, this); // Seviye 3
                seviyeForm.Show(); // Yeni formu aç
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (tamamlananSeviye >= 4)
            {
                Form2 seviyeForm = new Form2(4, this); // Seviye 4
                seviyeForm.Show(); // Yeni formu aç
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (tamamlananSeviye >= 5)
            {
                Form2 seviyeForm = new Form2(5, this); // Seviye 5
                seviyeForm.Show(); // Yeni formu aç
            }
        }

        public void SeviyeTamamlandı(int seviye)
        {
            if (seviye == tamamlananSeviye)
            {
                MessageBox.Show($"Seviye {seviye} tamamlandı!"); // Debug için

                tamamlananSeviye++;
                GüncelleButonlar(); // Butonları güncelle
            }
        }

        private void GüncelleButonlar()
        {
            button2.Enabled = tamamlananSeviye >= 2; // 2. seviye butonunu güncelle
            button3.Enabled = tamamlananSeviye >= 3; // 3. seviye butonunu güncelle
            button4.Enabled = tamamlananSeviye >= 4; // 4. seviye butonunu güncelle
            button5.Enabled = tamamlananSeviye >= 5; // 5. seviye butonunu güncelle
        }
    }
}
