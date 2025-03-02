using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace matamatikoyunu
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
 /* // Komut satırından gelen argümanları kontrol et
            int seviye = 1; // Varsayılan seviye

            if (args.Length > 2 && args[1] == "open" && int.TryParse(args[2], out int tempSeviye))
            {
                seviye = tempSeviye; // Komut satırından gelen seviye
            }
            else
            {
                Console.WriteLine("Geçersiz argümanlar veya seviye bilgisi alınamadı.");
            }

            // Seviye bilgisini konsola yazdır
            Console.WriteLine($"Açılan seviye: {seviye}");

            Application.Run(new Form1(seviye)); // Form1'i başlat*/

