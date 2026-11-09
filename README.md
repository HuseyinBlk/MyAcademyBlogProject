# ✍️ Blogy - Modern N-Tier Blog Platformu & Yapay Zeka Stüdyosu

**Blogy**, ASP.NET Core 10 ile **N-Tier (Çok Katmanlı)** mimari prensiplerine uygun olarak geliştirilmiş; **Google Gemini** ve **Hugging Face (Helsinki-NLP & Toxic-BERT)** yapay zekâ entegrasyonlarına, çoklu rol (Admin, Writer, User) mimarisine ve dinamik analitik dashboard panellerine sahip yeni nesil bir blog ve dijital yayıncılık platformudur.

---

## 🚀 Öne Çıkan Özellikler

### 1. 🤖 Çift Motorlu AI Moderasyon & Toksisite Analizi
* **Google Gemini & Hugging Face:** Gelen kullanıcı yorumları ilk aşamada Helsinki-NLP ve Toxic-BERT modelleriyle analiz edilir; servis kesintisi veya kota sınırında sistem otomatik olarak Google Gemini motoruna fallback yapar.
* **Akıllı Moderasyon Paneli:** Toksisite skoruna göre yorumlar otomatik onaylanır, incelemeye alınır veya engellenir. Adminler tek tıkla hem **HF** hem de **Gemini** ile yorumları anlık olarak yeniden analiz edebilir.

### 2. 🔐 Çok Katmanlı Identity & Rol Yönetimi (Areas)
* **3 Farklı Area Mimarisi:**
  * **Admin Area:** Kategori, etiket, yazar, kullanıcı, blog, yorum ve sistem loglarının tam yetkili kontrol merkezi.
  * **Writer Area:** Yazarların kendi blog yazılarını, çoklu görsellerini, taslaklarını ve gelen yorumlarını yönetebildiği özel stüdyo.
  * **User Area:** Okuyucuların profil bilgilerini güncelleyebildiği ve kendi yorum geçmişini takip edebildiği kullanıcı paneli.

### 3. 📊 Dinamik Analitik Dashboard & Canlı Grafikler
* **Kategori & Blog Dağılım Grafikleri:** Blogların kategorilere göre dağılımını ve okuma/yorum trendlerini görselleştiren interaktif grafikler.
* **Sayaç Kartları & Metrikler:** Toplam blog, aktif yazar, incelenen yorum, moderasyon başarı oranı ve sistem durumunu anlık özetleyen dinamik kartlar.

### 4. 🎨 Kusursuz Görsel & Metin Deneyimi
* **Görsel Optimizasyonu:** Sabit en-boy oranı (`object-fit: cover`) ile kartlarda görsel taşması veya orantısızlık engellenmiştir.
* **Akıllı Metin Özetleme (Truncation):** Blog listelemelerinde HTML etiketleri arındırılarak (`Regex Strip`) kelime bütünlüğü korunur ve estetik `...` bitişiyle kart düzeni standartlaştırılmıştır.
* **Görsel Yaşam Döngüsü:** Düzenleme veya silme işlemlerinde eski dosyalar sunucu diskinden (`IFileService.DeleteFile`) otomatik temizlenir.

### 5. 🔍 Sayfalama, Arama & Etiket Filtreleme
* **PagedList Entegrasyonu:** Performans dostu sayfalama mekanizması.
* **Anlık Arama & Temizleme:** Başlık ve içerik bazlı arama filtresi ve tek tıkla arama temizleme desteği.
* **Etiket & Kategori Gezintisi:** İlişkisel etiket bulutu ve kategori filtreleme.

---

## 🛠️ Teknolojik Altyapı ve Mimarî

* **Framework:** ASP.NET Core 10.0 MVC (C# 13)
* **Mimari:** N-Tier Architecture (Entity, DataAccess, Business, WebUI)
* **Veritabanı & ORM:** MS SQL Server & Entity Framework Core (Code-First)
* **Güvenlik & Kimlik:** ASP.NET Core Identity & Cookie Authentication
* **Dependency Injection & Servis Tarama:** Scrutor
* **Yapay Zeka (AI/NLP):** Google Gemini API, Hugging Face Inference API (`toxic-bert`, `opus-mt-tr-en`)
* **E-Posta Servisi:** MailKit / MimeKit & Brevo SMTP
* **Frontend & UI:** Bootstrap 5, Bootstrap Icons, SweetAlert2, ApexCharts / Chart.js
* **Sayfalama:** PagedList.Core.Mvc

---

## 📁 Proje Katman Yapısı

```text
MyAcademyBlogProject/
├── Blogy.Entity/        # Veritabanı varlıkları (Blog, Category, Comment, Tag, AppUser vb.)
├── Blogy.DataAccess/    # EF Core DbContext, Migrations, Repository implementasyonları
├── Blogy.Business/      # İş mantığı, DTO'lar, AI servisleri (Gemini/HF), Validasyonlar
└── Blogy.WebUI/         # UI Katmanı (Controllers, Views, Areas [Admin/Writer/User], ViewComponents)
```

---

## ⚙️ Kurulum & Çalıştırma

1. **Projeyi Klonlayın:**
   ```bash
   git clone https://github.com/HuseyinBlk/MyAcademyBlogProject.git
   cd MyAcademyBlogProject
   ```

2. **Veritabanı Bağlantısı ve API Anahtarları:**
   `Blogy.WebUI/appsettings.json` dosyasını yapılandırın:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=localhost;Database=BlogyDb;Trusted_Connection=True;TrustServerCertificate=True;"
     },
     "Gemini": {
       "ApiKey": "YOUR_GEMINI_API_KEY"
     },
     "HuggingFace": {
       "ApiKey": "YOUR_HF_API_KEY"
     }
   }
   ```

3. **Veritabanı Migration:**
   ```bash
   dotnet ef database update --project Blogy.DataAccess --startup-project Blogy.WebUI
   ```

4. **Projeyi Başlatın:**
   ```bash
   dotnet run --project Blogy.WebUI
   ```

---

## 👨‍💻 Geliştirici

**Hüseyin BALIK**
* GitHub: [@HuseyinBlk](https://github.com/HuseyinBlk)
* LinkedIn: [Hüseyin Balık](https://linkedin.com/in/huseyinbalik)
