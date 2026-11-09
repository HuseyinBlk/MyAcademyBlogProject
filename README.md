# Blogy - ASP.NET Core 10 N-Tier Blog Platformu & Yapay Zekâ Stüdyosu

Bu proje, yazarların ve yöneticilerin zengin içerikler üretebildiği, okuyucuların yorum ve etkileşimde bulunabildiği, modern **N-Tier (Çok Katmanlı)** mimari prensiplerine uygun olarak geliştirilmiş ve **Google Gemini** ile **Hugging Face (Helsinki-NLP & Toxic-BERT)** yapay zekâ moderasyon teknolojileriyle güçlendirilmiş gelişmiş bir **ASP.NET Core 10 MVC** web uygulamasıdır.

Uygulamanın ziyaretçi (UI) tarafında **Blogy Bootstrap Şablonu**, yönetim ve yazar (Admin / Writer / User) tarafında ise modern **Admin Dashboard Şablonu** kullanılarak hem görsel hem de işlevsel açıdan kurumsal ve temiz bir kullanıcı deneyimi hedeflenmiştir.

---

## Teknolojik Altyapı ve Mimari

* **Backend Framework:** ASP.NET Core 10.0 MVC (C# 13)
* **Mimari Yaklaşım:** N-Tier Architecture (Entity, DataAccess, Business, WebUI)
* **Veritabanı Erişimi (ORM):** Entity Framework Core (Code First Yaklaşımı)
* **Veritabanı:** MS SQL Server
* **Kimlik Doğrulama ve Güvenlik:** ASP.NET Core Identity & Cookie Authentication, Rol Bazlı Yetkilendirme (`[Authorize(Roles = "...")]`)
* **Çoklu Rol & Alan (Area) Mimarisi:** 3 Farklı Bağımsız Area Yapısı (`Admin`, `Writer`, `User`)
* **Yapay Zekâ & Doğal Dil İşleme (NLP):**
  * **Hugging Face Inference API:** `unitary/toxic-bert` (toksisite ve hakaret tespiti) & `Helsinki-NLP/opus-mt-tr-en` (otomatik çeviri köprüsü)
  * **Google Gemini API:** Kesinti veya kota durumunda otomatik devreye giren Fallback motoru ve sistem komutlu toksisite denetimi
* **Dependency Injection & Servis Yaşam Döngüsü:** Scrutor (Otomatik Servis & Generic Repository Taraması)
* **E-Posta Servisi:** MailKit / MimeKit & Brevo SMTP Entegrasyonu
* **Sayfalama Motoru:** PagedList.Core.Mvc
* **Arayüz Teknolojileri:** Bootstrap 5, Bootstrap Icons, JavaScript (ES6), HTML5, CSS3
* **Grafik Motoru:** ApexCharts / Chart.js
* **Bildirimler:** SweetAlert2 & Bootstrap Modalları
* **Dosya & Medya Yaşam Döngüsü:** Sunucu tabanlı benzersiz dosya saklama (GUID) ve güncelleme/silme işlemlerinde otomatik disk temizleme (`IFileService.DeleteFile`)

---

## Veritabanı Entity Yapısı (Veri Modelleri)

Projenin veritabanı şeması modüler, ilişkisel ve veri bütünlüğünü garanti altına alan bir yapıda tasarlanmıştır:

1. **`AppUser` (Kullanıcı / Yazar):** ASP.NET Core Identity altyapısını genişletir (`FirstName`, `LastName`, `ProfilImageUrl`, `Description`, `Blogs`, `Comments`).
2. **`AppRole` (Roller):** Sistemdeki yetki seviyelerini (`Admin`, `Writer`, `User`) temsil eder.
3. **`Blog` (Blog Yazıları):** Blog içeriklerini ve çoklu görsel bağlantılarını barındırır (`Title`, `Description`, `CoverImage`, `BlogImage`, `BlogImage2`, `CategoryId`, `WriterId`, `Status`, `CreatedOn`).
4. **`Category` (Kategoriler):** Blog yazılarını sınıflandırmak için kullanılır (`Name`, `CategoryStatus`, `Blogs`).
5. **`Comment` (Yorumlar):** Ziyaretçi veya kayıtlı kullanıcı yorumlarını ve AI toksisite skorunu saklar (`NameSurname`, `Email`, `Content`, `CommentDate`, `Status` [Pending/Approved/Rejected], `ToxicityScore`, `BlogId`, `UserId`).
6. **`Tag` (Etiketler):** İçerikleri konu başlıklarına göre etiketler (`Name`).
7. **`BlogTag` (Çoktan Çoka İlişki):** Bloglar ile etiketler arasındaki Many-to-Many ilişkisini yönetir (`BlogId`, `TagId`).
8. **`About` (Hakkımızda):** Kurumsal tanıtım metinlerini, öne çıkan maddeleri ve görselleri saklar (`Title`, `Description1`, `Description2`, `FeatureTitle1..3`, `FeatureDescription1..3`, `ImageUrl`).
9. **`Contact` (İletişim & Mesajlar):** Ziyaretçilerin gönderdiği iletişim mesajlarını ve okunma durumlarını tutar (`FullName`, `Email`, `Subject`, `Message`, `SendDate`, `IsRead`).
10. **`Social` (Sosyal Medya):** Sosyal medya hesaplarının ikon ve yönlendirme bağlantılarını tutar (`Title`, `Icon`, `Url`).

---

## Sistem Özellikleri ve Modüller

### 1. Çift Motorlu AI Moderasyon & Toksisite Analizi (Dual-Engine AI)
* **Hugging Face Router & Fallback:** Gelen yorumlar ilk aşamada Helsinki-NLP ile çevrilip Toxic-BERT modeli üzerinden toksisite analizine tabi tutulur. Model kota aşımı veya servis kesintisi yaşadığında sistem kesintisiz olarak Google Gemini API'ye fallback yapar.
* **Yönetici Re-Analyze Araçları:** Admin panelinde analizi yapılamamış veya şüpheli yorumlar için tek tıkla hem **Hugging Face** hem de **Google Gemini** ile anlık yeniden analiz etme butonları yer alır.
* **Toksisiteye Göre Otomatik Karar:** Skorlama sistemine göre güvenli yorumlar doğrudan onaylanırken, yüksek riskli yorumlar admin onayına düşürülür veya engellenir.

### 2. 3 Katmanlı Identity & Rol Mimarisi (Areas)
* **Admin Area (`/Admin`):** Blog, kategori, etiket, kullanıcı, rol ve yorum moderasyonunun yapıldığı, sistem genelini yöneten tam yetkili ana merkez.
* **Writer Area (`/Writer`):** Yazarların sadece kendi yazılarını oluşturup düzenleyebildiği, istatistiklerini görebildiği ve kendi yazılarına gelen yorumları yönetebildiği stüdyo.
* **User Area (`/User`):** Okuyucuların profil bilgilerini düzenleyebildiği ve bloglara yaptıkları yorumların durumunu (Onaylandı/Beklemede) takip edebildiği panel.

### 3. Dinamik Analitik Dashboard & Grafikler
* **ApexCharts / Chart.js:** Kategori bazında blog dağılımları ve haftalık yayın trendleri dinamik olarak görselleştirilir.
* **Canlı Sayaçlar:** Toplam blog, aktif yazar, incelenen yorum, moderasyon başarı oranı ve sistem durumunu özetleyen dinamik bilgi kartları.

### 4. Standart Görsel & Metin Deneyimi (Harmonized UI)
* **Standart Kart Oranları:** Tüm blog listeleme sayfalarında (Ana Sayfa, Bloglar, Kategori, Etiket) görseller `width: 230px; height: 150px` sabit kutuda `object-fit: cover` ile taşma yapmadan eşit oranda sunulur.
* **Akıllı Metin Özetleme (Regex Truncation):** Blog özetleri HTML etiketlerinden arındırılarak kelime bütünlüğü korunur ve 140 karakter sonunda `...` ile tamamlanır.
* **Sunucu Dosya Temizliği:** Blog görselleri (Kapak, Detay 1, Detay 2) veya profil fotoğrafları güncellendiğinde ya da silindiğinde eski dosyalar diskten (`IFileService.DeleteFile`) otomatik olarak temizlenir.

### 5. Sayfalama, Arama ve Etiket Gezintisi
* **PagedList Entegrasyonu:** Büyük veri setlerinde performans kaybını önleyen dinamik sayfalama yapısı.
* **Filtreleme & Arama:** Blog arama kutusu ve tek tıkla "Aramayı Temizle" seçeneği; kategori ve ilişkisel etiket bulutu ile akıcı navigasyon.

---

## Proje Ekran Görüntüleri

Projenin tüm arayüzlerinin ekran görüntüleri aşağıda listelenmiştir. Ekran görüntülerini `Blogy.WebUI/wwwroot/images/screenshots/` klasörüne eklediğinizde burada otomatik olarak görüntülenecektir.

### Kimlik Doğrulama Ekranları (Auth UI)

#### 1. Giriş Yap Ekranı (Login)
![Giriş Ekranı](Blogy.WebUI/wwwroot/images/screenshots/login.png)

#### 2. Kayıt Ol Ekranı (Register)
![Kayıt Ekranı](Blogy.WebUI/wwwroot/images/screenshots/register.png)

---

### Ziyaretçi Arayüzü (Visitor UI - Blogy)

#### 1. Ana Sayfa (Vitrin & Öne Çıkan Bloglar)
![Ziyaretçi Ana Sayfa](Blogy.WebUI/wwwroot/images/screenshots/visitor_home.png)

#### 2. Blog Listesi & PagedList Sayfalama
![Blog Listesi](Blogy.WebUI/wwwroot/images/screenshots/visitor_blogs.png)

#### 3. Blog Detay & Yorum Alanı
![Blog Detay](Blogy.WebUI/wwwroot/images/screenshots/visitor_blog_details.png)

#### 4. Kategoriye Göre Filtrelenen Bloglar
![Kategori Blogları](Blogy.WebUI/wwwroot/images/screenshots/visitor_category.png)

#### 5. Etikete Göre Filtrelenen Bloglar
![Etiket Blogları](Blogy.WebUI/wwwroot/images/screenshots/visitor_tag.png)

#### 6. Hakkımızda Sayfası
![Hakkımızda](Blogy.WebUI/wwwroot/images/screenshots/visitor_about.png)

#### 7. İletişim ve Mesaj Gönderme Formu
![İletişim](Blogy.WebUI/wwwroot/images/screenshots/visitor_contact.png)

---

### Yönetici Kontrol Paneli (Admin Area UI)

#### 1. Yönetici Dashboard (Grafikler ve Canlı Sayaçlar)
![Admin Dashboard](Blogy.WebUI/wwwroot/images/screenshots/admin_dashboard.png)

#### 2. Blog Yönetimi & Çoklu Görsel Ekleme/Düzenleme
![Admin Blog Yönetimi](Blogy.WebUI/wwwroot/images/screenshots/admin_blogs.png)

#### 3. Kategori Yönetim Sayfası (CRUD)
![Admin Kategori Yönetimi](Blogy.WebUI/wwwroot/images/screenshots/admin_categories.png)

#### 4. Etiket Yönetim Sayfası (CRUD)
![Admin Etiket Yönetimi](Blogy.WebUI/wwwroot/images/screenshots/admin_tags.png)

#### 5. AI Destekli Yorum Moderasyon Paneli (HF & Gemini Re-Analyze)
![Admin Yorum Moderasyonu](Blogy.WebUI/wwwroot/images/screenshots/admin_comments.png)

#### 6. Kullanıcı & Rol Yetkilendirme Yönetimi
![Admin Kullanıcı Yönetimi](Blogy.WebUI/wwwroot/images/screenshots/admin_users.png)

#### 7. Profil ve Şifre Güncelleme
![Admin Profil](Blogy.WebUI/wwwroot/images/screenshots/admin_profile.png)

---

### Yazar Paneli Arayüzü (Writer Area UI)

#### 1. Yazar Dashboard & Blog İstatistikleri
![Yazar Dashboard](Blogy.WebUI/wwwroot/images/screenshots/writer_dashboard.png)

#### 2. Yazar Blog Yazma ve Görsel Yükleme Stüdyosu
![Yazar Blog Stüdyosu](Blogy.WebUI/wwwroot/images/screenshots/writer_blogs.png)

#### 3. Yazara Ait Bloglara Gelen Yorumlar
![Yazar Yorumları](Blogy.WebUI/wwwroot/images/screenshots/writer_comments.png)

#### 4. Yazar Profil Düzenleme
![Yazar Profil](Blogy.WebUI/wwwroot/images/screenshots/writer_profile.png)

---

### Okuyucu Paneli Arayüzü (User Area UI)

#### 1. Okuyucu Profil ve Bilgi Güncelleme
![Kullanıcı Profil](Blogy.WebUI/wwwroot/images/screenshots/user_profile.png)

#### 2. Kullanıcı Yorum Geçmişi ve Durum Takibi
![Kullanıcı Yorumları](Blogy.WebUI/wwwroot/images/screenshots/user_comments.png)

---

## Kurulum ve Çalıştırma

1. **Projeyi Bilgisayarınıza Klonlayın:**
   ```bash
   git clone https://github.com/HuseyinBlk/MyAcademyBlogProject.git
   cd MyAcademyBlogProject
   ```

2. **Veritabanı Bağlantısı ve API Anahtarları:**
   `Blogy.WebUI/appsettings.json` dosyasını kendi ortamınıza göre yapılandırın:
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

3. **Veritabanı Migration'larını Uygulayın:**
   ```bash
   dotnet ef database update --project Blogy.DataAccess --startup-project Blogy.WebUI
   ```

4. **Projeyi Başlatın:**
   ```bash
   dotnet run --project Blogy.WebUI
   ```

5. **Varsayılan Yönetici Girişi:**
   Uygulama ilk kez çalıştırıldığında sistem otomatik olarak rolleri (`Admin`, `Writer`, `User`) ve varsayılan yönetici hesabını oluşturur:
   * **Kullanıcı Adı:** `admin`
   * **E-Posta:** `admin@blogy.com`
   * **Şifre:** `Password12*`

6. **Rol Atama ve Kullanıcı Yönetimi:**
   Sisteme Kayıt Ol ekranından (`/Register`) kaydolan tüm kullanıcılar varsayılan olarak `User` rolünü alır. Kullanıcılara `Admin` veya `Writer` rolü yetkilendirmesi yapmak için yönetici hesabıyla giriş yapıp **Admin Paneli -> Kullanıcılar & Roller (`/Admin/Users`)** ekranından dilediğiniz rolleri atayabilirsiniz.

---

## Geliştirici

**Hüseyin BALIK**
* GitHub: [@HuseyinBlk](https://github.com/HuseyinBlk)
* LinkedIn: [Hüseyin Balık](https://linkedin.com/in/huseyinbalik)
