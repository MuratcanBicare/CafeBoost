using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace CafeBoost.Data
{
    [Table("Siparisler")]
    public class Siparis
    {
        public Siparis()
        {
            SiparisDetaylar = new HashSet<SiparisDetay>();
            AcilisiZamani = DateTime.Now;
        }

        public int Id { get; set; }
        public int MasaNo { get; set; }
        public DateTime? AcilisiZamani { get; set; }
        public DateTime? KapanisZamani { get; set; }
        public SiparisDurum Durum { get; set; }
        public decimal OdenenTutar { get; set; }

        public string ToplamTutarTL => $"{ToplamTutar():0.00}₺";
        public virtual ICollection<SiparisDetay> SiparisDetaylar { get; set; }

        public decimal ToplamTutar()
        {

            return SiparisDetaylar.Sum(x => x.Tutar());

            #region oldSchool
            //decimal sonuc = 0m;
            //foreach (var s in SiparisDetaylar)
            //{
            //    sonuc += s.Adet * s.BirimFiyat;
            //}

            //return sonuc; 
            #endregion
        }
    }
}
