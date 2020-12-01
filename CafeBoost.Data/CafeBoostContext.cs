using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace CafeBoost.Data
{
    public class CafeBoostContext : DbContext
    {
        public CafeBoostContext() : base("name=CafeBoostContext")
        {
            //ouput penceresinde çalışan sorguları göster
            Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //fluent api
            //bir SiparisDetay ki
            //zorunlu bir Urun'u vardır
            //ki o Urun'un birden çok SiparisDetay'ı vardır
            //ki o SiparisDetay'lar UrunId alanı üzerinde foreign key ile Urun'le ilişki kurar
            //ki o Urun'u siler ise ona bağlı SiparisDetaylar da silinmesin  

            modelBuilder
                .Entity<SiparisDetay>()
                .HasRequired(x => x.Urun)
                .WithMany(x => x.SiparisDetaylar)
                //.HasForeignKey(x => x.UrunId)  // gerek yok (geleneğe uygun olarak tanımlama yaptığımız için)
                .WillCascadeOnDelete(false);
        }
        public int MasaAdet { get; set; } = 20;
        public DbSet<Urun> Urunler { get; set; }
        public DbSet<Siparis> Siparisler { get; set; }
        public DbSet<SiparisDetay> SiparisDetaylar { get; set; }


    }
}
