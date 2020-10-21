using System;
using System.Collections.Generic;
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

        public string ToplamTutarTL { get { return ToplamTutar().ToString(); } }

        private decimal ToplamTutar()
        {
            return 0;
        }
    }
}
