using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CafeBoost.Data
{
   public class Siparis
    {
        public int MasaNo { get; set; }
        public List<SiparisDetay> SiparisDetaylar { get; set; }
        public DateTime? AcilisiZamani { get; set; }
        public DateTime? KapanisZamani { get; set; }
        public SiparisDurum Durum { get; set; }

        public string ToplamTutarTL => $"{ToplamTutar():0.00}₺";

        public Siparis()
        {
            SiparisDetaylar = new List<SiparisDetay>();
            AcilisiZamani = DateTime.Now;
        }

        public decimal ToplamTutar()
        {

            return SiparisDetaylar.Sum(x => x.Tutar());
            
            //decimal sonuc = 0m;
            //foreach (var s in SiparisDetaylar)
            //{
            //    sonuc += s.Adet * s.BirimFiyat;
            //}

            //return sonuc;
        }
    }
}
