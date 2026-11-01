# MyAcademy Blog Projesi - Geliştirme Caseleri & Yol Haritası

Bu doküman, projede uygulanacak olan eğitmenlerin (Erhan Hoca ve Murat Hoca) özel isterlerini, kabul kriterlerini ve uygulama adımlarını içerir.

---

## 📌 1. Erhan Hocanın Caseleri

### 1.1. Giriş & Yetkilendirme (Authentication & Authorization)
- [ ] **Ana Sayfa Giriş Butonu:** Ana sayfa header (menü) kısmında dinamik "Giriş Yap" butonu yer alacak. Kullanıcı giriş yapmışsa kullanıcı adı/profil ve çıkış butonları gösterilecek.
- [ ] **Giriş Sayfası:** Şık ve işlevsel bir giriş yap (`/Login/Index`) sayfası oluşturulacak.
- [ ] **Varsayılan Rol Ataması:** Veritabanına `User` rolü eklenecek ve yeni kayıt (`/Register/Index`) olan her kullanıcıya otomatik olarak `User` rolü atanacak.
- [ ] **Rol Koruması ile Yorum:** Giriş yapmayan kullanıcılar bloglara yorum yapamayacak (yorum alanı gizlenecek veya "Giriş Yap" uyarısı gösterilecek).
- [ ] **Blog Detayında Yorum:** Blog detay sayfasından giriş yapmış kullanıcı doğrudan ilgili bloga yorum gönderebilecek.

### 1.2. Yeni Area Mimarisi (Rol Bazlı Paneller)
- [ ] **User Area (`/User`):**
  - Sadece `User` rolüne sahip kullanıcılar erişebilecek.
  - Kendi yaptığı yorumları listeleme, güncelleme ve silme.
  - Profil bilgilerini (ad, soyad, şifre, avatar) güncelleme.
- [ ] **Writer Area (`/Writer`):**
  - Sadece `Writer` rolüne sahip kullanıcılar erişebilecek.
  - Blog oluşturma, düzenleme, silme ve kendi bloglarına gelen yorumları yönetme.
  - Profil güncelleme.
- [ ] **Area İzolasyonu:** Her Area (`Admin`, `User`, `Writer`) birbirinden tamamen bağımsız layout ve yetkilendirme kontrollerine sahip olacak.

### 1.3. Dinamik UI & İstatistikler
- [ ] **İletişim & Hakkımda:** Ana sayfadaki ve ilgili sayfalardaki iletişim ile hakkımda bilgileri veritabanından dinamik olarak çekilecek.
- [ ] **Sosyal Medya:** Sosyal medya butonları, ikonları ve yönlendirme linkleri veritabanından dinamik olarak yüklenecek.
- [ ] **Kategori Blog Sayıları:** Blog sayfasında kategoriler listelenirken, her kategorinin yanında o kategoriye ait blog sayısı gösterilecek (Örn: `Teknoloji (12)`).

---

## 📌 2. Murat Hocanın Caseleri

### 2.1. Yapay Zeka (AI) Entegrasyonları
- [ ] **AI Makale Oluşturucu (Admin):** 
  - Admin panelinde form üzerinden anahtar kelimeler ve kısa bir özet cümle girilecek.
  - Yapay zeka servisi (Gemini / OpenAI API) tetiklenerek en az 1000 karakter uzunluğunda zengin bir blog makalesi otomatik üretilecek.
- [ ] **AI Otomatik İletişim Yanıtlayıcı:**
  - "Bize Yazın" / İletişim formundan mesaj gönderildiği anda yapay zeka mesajı analiz edecek.
  - Gelen mesaj hangi dilde yazılmışsa (Türkçe, İngilizce, Korece, Japonca vb.) o dilde otomatik kurumsal bir yanıt taslağı oluşturup yanıt dönecek.
- [ ] **Dinamik AI About Bölümü:**
  - Sitenin alt (footer) veya hakkımızda bölümündeki tanıtım metni AI servisinden dinamik olarak oluşturulup sunulacak.

### 2.2. Doğal Dil İşleme (Hugging Face NLP & Toksik Yorum Tespiti)
- [ ] **Helsinki-NLP Çeviri Hattı:**
  - Türkçe yorumlar doğrudan analiz edilmeyecek (doğruluk düşüklüğü sebebiyle).
  - Yorum metni önce `Helsinki-NLP/opus-mt-tr-en` modeli ile İngilizceye çevrilecek.
- [ ] **Toxic Bert Toksiklik Analizi:**
  - İngilizceye çevrilen metin `unitary/toxic-bert` modeline aktarılacak.
  - Toksiklik oranı hesaplanacak; eğer oran **> 0.50** ise yorum otomatik olarak toksik olarak işaretlenecek / onay sürecine düşürülecek.

### 2.3. Dashboard, İstatistikler & Filtreleme
- [ ] **Dinamik Grafikler (Charts):** Dashboard üzerinde en az 2 adet dinamik grafik (Chart.js / ApexCharts) yer alacak (Örn: Kategori dağılımı, aylık yorum/yazı sayıları).
- [ ] **İstatistik Kartları & Widgetlar:** Dashboard üzerinde toplam blog, toplam yorum, toplam yazar, aktif kullanıcı gibi widget kartları bulunacak.
- [ ] **Kategoriye Göre Filtreleme:** Blog sayfasının sağ tarafındaki kategorilere tıklandığında yalnızca seçilen kategoriye ait yazılar listelenecek ve sayfalanabilecek.

---

## 🚀 Uygulama Fazları

| Faz | Kapsam | Durum |
| :--- | :--- | :--- |
| **Faz 1** | Identity, Otomatik User Rolü, User/Writer Area Kurulumu | ⏳ Sıradaki |
| **Faz 2** | Dinamik UI (Hakkımda, İletişim, Sosyal Medya, Kategori Sayıları) | ⏹️ Beklemede |
| **Faz 3** | Blog Detay Yorum Sistemi, Giriş Kontrolü, Kategori Filtreleme | ⏹️ Beklemede |
| **Faz 4** | Yapay Zeka Servisleri (1000 Karakter Makale, Çok Dilli İletişim, AI About) | ⏹️ Beklemede |
| **Faz 5** | Hugging Face (Toxic Bert + Helsinki) & Dashboard Grafikleri | ⏹️ Beklemede |
