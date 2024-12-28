using System;
using System.Collections.Generic;

namespace OtelResepsiyonSistemi
{
    class Program
    {
        static decimal kasa = 0; // Kasa miktarını tutar

        static void Main(string[] args)
        {
            List<Oda> odalar = new List<Oda>(); // Oteldeki odaları temsil eden bir liste

            // Odaları başlatır (10 adet oda oluşturulur)
            for (int i = 1; i <= 10; i++)
            {
                if (i <= 5)
                {
                    odalar.Add(new Oda { OdaNumarasi = i, Durum = "Boş", Kapasite = 2, Fiyat = 100, TemizlikDurumu = true });
                }
                else
                {
                    odalar.Add(new Oda { OdaNumarasi = i, Durum = "Boş", Kapasite = 4, Fiyat = 200, TemizlikDurumu = true });
                }
            }

            // Ana menü döngüsü
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Otel Resepsiyon Sistemi ===");
                Console.WriteLine("1. Mevcut Müşterileri Listele");
                Console.WriteLine("2. Odaları Listele");
                Console.WriteLine("3. Odaya Müşteri Yerleştir");
                Console.WriteLine("4. Odayı Boşalt");
                Console.WriteLine("5. Kasayı Görüntüle");
                Console.WriteLine("6. Odayı Temizle");
                Console.WriteLine("7. Çıkış");
                Console.Write("Seçiminiz: ");

                string secim = Console.ReadLine(); // Kullanıcıdan seçim alır

                // Kullanıcı seçimine göre ilgili metodu çağırır
                switch (secim)
                {
                    case "1":
                        MusterileriListele(odalar); // Mevcut müşterileri listeler
                        break;
                    case "2":
                        OdalariListele(odalar); // Odaların durumunu listeler
                        break;
                    case "3":
                        OdayaMusteriYerlestir(odalar); // Odaya müşteri yerleştirir
                        break;
                    case "4":
                        OdayiBosalt(odalar); // Odayı boşaltır
                        break;
                    case "5":
                        KasayiGoruntule(); // Kasayı görüntüler
                        break;
                    case "6":
                        OdayiTemizle(odalar); // Odayı temizler
                        break;
                    case "7":
                        return; // Programı sonlandırır
                    default:
                        Console.WriteLine("Geçersiz seçim. Lütfen tekrar deneyin.");
                        break;
                }

                Console.WriteLine("Devam etmek için bir tuşa basın...");
                Console.ReadKey();
            }
        }

        // Mevcut müşterileri listeleyen metot
        static void MusterileriListele(List<Oda> odalar)
        {
            Console.WriteLine("=== Mevcut Müşteriler ===");
            bool musteriVar = false; // Müşteri var mı kontrolü

            foreach (var oda in odalar)
            {
                if (oda.Durum != "Boş" && oda.Musteriler.Count > 0)
                {
                    musteriVar = true;
                    Console.WriteLine($"Oda {oda.OdaNumarasi}:");
                    foreach (var musteri in oda.Musteriler)
                    {
                        Console.WriteLine($"- {musteri.Isim}, Telefon: {musteri.Telefon}");
                    }
                }
            }

            if (!musteriVar)
            {
                Console.WriteLine("Hiç müşteri bulunamadı.");
            }
        }

        // Odaların durumunu listeleyen metot
        static void OdalariListele(List<Oda> odalar)
        {
            Console.WriteLine("=== Odaların Durumu ===");
            foreach (var oda in odalar)
            {
                string temizlikDurumu = oda.TemizlikDurumu ? "Temiz" : "Temizlenmesi Gerekiyor";
                Console.WriteLine($"Oda {oda.OdaNumarasi}: {oda.Durum}, Kapasite: {oda.Kapasite}, Fiyat: {oda.Fiyat} TL, Temizlik: {temizlikDurumu}");
            }
        }

        // Odaya müşteri yerleştiren metot
        static void OdayaMusteriYerlestir(List<Oda> odalar)
        {
            Console.Write("Hangi odaya müşteri yerleştirmek istiyorsunuz? (Oda No): ");
            int odaNo;
            if (int.TryParse(Console.ReadLine(), out odaNo))
            {
                var oda = odalar.Find(o => o.OdaNumarasi == odaNo);
                if (oda != null && oda.Durum == "Boş" && oda.TemizlikDurumu)
                {
                    Console.Write($"Kaç kişi yerleştirilecek? (1-{oda.Kapasite}): ");
                    int kisiSayisi;
                    if (int.TryParse(Console.ReadLine(), out kisiSayisi) && kisiSayisi > 0 && kisiSayisi <= oda.Kapasite)
                    {
                        Console.Write("Kaç gece kalacaklar? ");
                        int geceSayisi;
                        if (int.TryParse(Console.ReadLine(), out geceSayisi) && geceSayisi > 0)
                        {
                            decimal kazanc = kisiSayisi * oda.Fiyat * geceSayisi; // Toplam kazancı hesaplar
                            for (int i = 1; i <= kisiSayisi; i++)
                            {
                                Console.Write($"{i}. Müşteri İsmi: ");
                                string isim = Console.ReadLine();
                                Console.Write($"{i}. Telefon Numarası: ");
                                string telefon = Console.ReadLine();
                                oda.Musteriler.Add(new Musteri { Isim = isim, Telefon = telefon });
                            }

                            oda.Durum = $"Dolu ({kisiSayisi} kişi)";
                            kasa += kazanc; // Kazancı kasaya ekler
                            oda.TemizlikDurumu = false; // Oda kiralandıktan sonra temizlenmesi gerekir

                            Console.WriteLine($"Müşteriler odaya yerleştirildi. Kazanç: {kazanc} TL");
                        }
                        else
                        {
                            Console.WriteLine("Geçersiz gece sayısı.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz kişi sayısı.");
                    }
                }
                else
                {
                    Console.WriteLine("Bu oda mevcut değil, dolu ya da temizlenmesi gerekiyor.");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz oda numarası.");
            }
        }

        // Odayı boşaltan metot
        static void OdayiBosalt(List<Oda> odalar)
        {
            Console.Write("Hangi odayı boşaltmak istiyorsunuz? (Oda No): ");
            int odaNo;
            if (int.TryParse(Console.ReadLine(), out odaNo))
            {
                var oda = odalar.Find(o => o.OdaNumarasi == odaNo);
                if (oda != null && oda.Durum != "Boş")
                {
                    oda.Durum = "Boş"; // Oda durumunu "Boş" olarak değiştirir
                    oda.Musteriler.Clear(); // Odaya bağlı müşterileri temizler
                    Console.WriteLine("Oda başarıyla boşaltıldı.");
                }
                else
                {
                    Console.WriteLine("Bu oda zaten boş ya da mevcut değil.");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz oda numarası.");
            }
        }

        // Odayı temizleyen metot
        static void OdayiTemizle(List<Oda> odalar)
        {
            Console.Write("Hangi odayı temizlemek istiyorsunuz? (Oda No): ");
            int odaNo;
            if (int.TryParse(Console.ReadLine(), out odaNo))
            {
                var oda = odalar.Find(o => o.OdaNumarasi == odaNo);
                if (oda != null && !oda.TemizlikDurumu)
                {
                    oda.TemizlikDurumu = true; // Oda temiz olarak işaretlenir
                    Console.WriteLine("Oda başarıyla temizlendi.");
                }
                else if (oda != null && oda.TemizlikDurumu)
                {
                    Console.WriteLine("Oda zaten temiz.");
                }
                else
                {
                    Console.WriteLine("Bu oda mevcut değil.");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz oda numarası.");
            }
        }

        // Kasayı görüntüleyen metot
        static void KasayiGoruntule()
        {
            Console.WriteLine($"Kasa Miktarı: {kasa} TL");
        }
    }

    // Oda sınıfı: Oteldeki odaların özelliklerini temsil eder
    class Oda
    {
        public int OdaNumarasi { get; set; } // Odanın numarası
        public string Durum { get; set; } // Odanın doluluk durumu ("Boş", "Dolu")
        public int Kapasite { get; set; } // Odanın kapasitesi
        public decimal Fiyat { get; set; } // Odanın gecelik fiyatı
        public bool TemizlikDurumu { get; set; } // Oda temiz mi?
        public List<Musteri> Musteriler { get; set; } = new List<Musteri>(); // Odaya yerleşen müşteri bilgileri
    }

    // Musteri sınıfı: Otelde konaklayan müşterilerin bilgilerini temsil eder
    class Musteri
    {
        public string Isim { get; set; } // Müşteri adı
        public string Telefon { get; set; } // Müşteri telefon numarası
    }
}
