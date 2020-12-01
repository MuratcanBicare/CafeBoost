using CafeBoost.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CafeBoost.UI
{
    public partial class Gecmis_Siparisler : Form
    {
        private readonly CafeBoostContext db;

        public Gecmis_Siparisler(CafeBoostContext cafeBoostContext)
        {
            db = cafeBoostContext;
            InitializeComponent();
            dgvSiparisDetaylar.AutoGenerateColumns = false;
            dgvSiparis.AutoGenerateColumns = false;
            dgvSiparis.DataSource = db.Siparisler.Where(x=> x.Durum != SiparisDurum.Aktif).ToList();

        }

        private void dgvSiparis_SelectionChanged(object sender, EventArgs e)
        {
            //en az bir seçili satır varsa
            if (dgvSiparis.SelectedRows.Count > 0)
            {
                //seçili satırlarının ilkinin üzerindeki siparis nesnesi
                Siparis seciliSiparis = (Siparis)dgvSiparis.SelectedRows[0].DataBoundItem;
                dgvSiparisDetaylar.DataSource = seciliSiparis.SiparisDetaylar.ToList();

            }
        }
    }
}
