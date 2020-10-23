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
    public partial class SiparisForm : Form
    {
        public event EventHandler<MasaTasimaEventArgs> MasaTasindi;
        private readonly KafeVeri db;
        private readonly Siparis siparis;
        private readonly BindingList<SiparisDetay> blSiparisDetaylar;
        public SiparisForm(KafeVeri kafeVeri, Siparis siparis)
        {
            db = kafeVeri;
            this.siparis = siparis;
            InitializeComponent();
            dgvSiparisDetaylar.AutoGenerateColumns = false;
            MasalariListele();
            UrunleriListeler();
            MasaNoGuncelle();
            OdemeTutariGuncelle();
            //Text = siparis.MasaNo.ToString();

            blSiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);
            blSiparisDetaylar.ListChanged += BlSiparisDetaylar_ListChanged;
            dgvSiparisDetaylar.DataSource = blSiparisDetaylar;

            #region Sütunları Elle İsimlendirme
            //dgvSiparisDetaylar.Columns[0].HeaderText = "Ürün Adı";
            //dgvSiparisDetaylar.Columns[1].HeaderText = "Birim Fiyat";
            //dgvSiparisDetaylar.Columns[2].HeaderText = "Adet";
            //dgvSiparisDetaylar.Columns[3].HeaderText = "Tutar TL"; 
            #endregion
        }

        private void MasalariListele()
        {
            cboMasalar.Items.Clear();

            for (int i = 1; i <= db.MasaAdet; i++)
            {
                if (!db.AktifSiparisler.Any(x => x.MasaNo == i))
                {
                    cboMasalar.Items.Add(i);
                }
            }
        }

        private void BlSiparisDetaylar_ListChanged(object sender, ListChangedEventArgs e)
        {
            OdemeTutariGuncelle();
        }

        private void OdemeTutariGuncelle()
        {
            lblOdemeTutari.Text = siparis.ToplamTutarTL;
        }

        private void UrunleriListeler()
        {
            cboUrun.DataSource = db.Urunler;
        }

        private void MasaNoGuncelle()
        {
            Text = $"Masa {siparis.MasaNo:00} - Sipariş Detayları (Açılış: {siparis.AcilisiZamani.Value.ToShortTimeString()})";
            lblMasaNo.Text = siparis.MasaNo.ToString("00");
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Urun secilen = (Urun)cboUrun.SelectedItem;
            int adet = (int)nudAdet.Value;

            SiparisDetay detay = new SiparisDetay()
            {
                UrunAd = secilen.UrunAd,
                BirimFiyat = secilen.BirimFiyat,
                Adet = adet
            };
            blSiparisDetaylar.Add(detay);
            //OdemeTutariGuncelle();

            #region Adı aynı olan ürünleri adede ekleme
            //SiparisDetay det = blSiparisDetaylar.FirstOrDefault(x => x.UrunAd == secilen.UrunAd);

            //if (det != null)
            //{
            //    det.Adet += adet;
            //    blSiparisDetaylar.ResetBindings();
            //}
            //else
            //{
            //    det = new SiparisDetay()
            //    {
            //        UrunAd = secilen.UrunAd,
            //        BirimFiyat = secilen.BirimFiyat,
            //        Adet = adet
            //    };
            //    blSiparisDetaylar.Add(det);
            //} 
            #endregion


        }

        private void dgvSiparisDetaylar_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Seçili detyaları silmek istediğinize emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (dr != DialogResult.Yes)
            {
                e.Cancel = true;
            }
        }

        private void btnAnasayfa_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Sipariş iptal edilerekk kapatılacaktır. Emin Misiniz?", "Ödeme Onayı", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                SiparisKapat(SiparisDurum.Iptal);
            }
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Ödeme alındıysa sipariş kapatılacaktır. Emin Misiniz?", "Ödeme Onayı", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                SiparisKapat(SiparisDurum.Odendi, siparis.ToplamTutar());

            }
        }

        private void SiparisKapat(SiparisDurum siparisDurum, decimal odenenTutar = 0)
        {
            siparis.OdenenTutar = odenenTutar;
            siparis.KapanisZamani = DateTime.Now;
            siparis.Durum = siparisDurum;
            db.AktifSiparisler.Remove(siparis);
            db.GecmisSiparisler.Add(siparis);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnMasaTasi_Click(object sender, EventArgs e)
        {
            if (cboMasalar.SelectedIndex < 0) return;
            int kaynak = siparis.MasaNo;
            int hedef = (int)cboMasalar.SelectedItem;
            siparis.MasaNo = hedef;
            MasaNoGuncelle();
            MasalariListele();

            MasaTasimaEventArgs args = new MasaTasimaEventArgs()
            {
                EskiMasaNo = kaynak,
                YeniMasaNo = hedef
            };
            MasaTasindiginda(args);



        }

        protected virtual void MasaTasindiginda(MasaTasimaEventArgs args)
        {
            if (MasaTasindi != null)
            {
                MasaTasindi(this, args);
            }
            //MasaTasindi?.Invoke(this, args);
        }

        //private void dgvSiparisDetaylar_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        //{
        //    OdemeTutariGuncelle();
        //}
    }
}
