# 🧩 Employee Task & Report Management System

Kurumsal mimari prensipler gözetilerek geliştirilmiş, şirket içi **görev**, **izin**, **raporlama** ve  
**sistem loglama** süreçlerini merkezi ve güvenli bir yapı altında yöneten **production-ready Full Stack** bir uygulama.

Bu proje basit bir CRUD uygulaması değil, **gerçek bir şirket içi operasyonel sistem** senaryosu üzerine inşa edilmiştir.  
Geliştirme sürecinde **Clean Architecture**, **güvenli kimlik doğrulama**, **Docker tabanlı containerization** ve  
**CI/CD pipeline** yaklaşımları esas alınmıştır.

---

## 🎯 Projenin Amacı

Şirket içerisinde;

- Kullanıcıların (Admin / Employee) güvenli şekilde yönetilmesi  
- Görevlerin oluşturulması, atanması ve yaşam döngüsünün takip edilmesi  
- İzin taleplerinin merkezi olarak yönetilmesi  
- Sistem üzerindeki tüm kritik işlemlerin loglanması  
- Yetkilendirme, izlenebilirlik ve sürdürülebilirlik sağlanması  

süreçlerini **ölçeklenebilir**, **bakımı kolay** ve **kurumsal standartlara uygun** bir mimariyle yönetmek.

---

## 🏗️ Mimari Yaklaşım

Proje **Clean Architecture** prensiplerine uygun olarak geliştirilmiştir.

- Katmanlar arası bağımlılıklar tersine çevrilmiştir  
- İş kuralları altyapıdan tamamen ayrılmıştır  
- UI, Application ve Infrastructure katmanları izole edilmiştir  
- Test edilebilir ve genişletilebilir yapı hedeflenmiştir  

**Katmanlar:**
- Domain  
- Application  
- Infrastructure  
- API  

---

## 👥 Roller ve Yetkiler

### 🔐 Admin
- Kullanıcı oluşturma, güncelleme ve silme  
- Görev oluşturma ve çalışanlara atama  
- İzin taleplerini onaylama / reddetme  
- Sistem loglarını görüntüleme  

### 👤 Employee
- Kendisine atanmış görevleri görüntüleme  
- Görev durumlarını güncelleme  
- İzin talebi oluşturma  

---

## ⚙️ Kullanılan Teknolojiler

### Backend
- .NET 8 Web API  
- Entity Framework Core  
- Clean Architecture  
- Generic Repository Pattern  
- AutoMapper  
- JWT Authentication & Role-Based Authorization  
- FluentValidation  
- Global Exception Handling Middleware  
- Serilog ile merkezi loglama  

### Frontend
- React  
- Component-based UI mimarisi  
- API tabanlı veri yönetimi  

### DevOps & Deployment
- Docker (Multi-Stage Dockerfile)  
- Docker Compose  
- Nginx (Frontend Production Build)  
- GitHub Actions (CI Pipelines)  
- Cloud-ready yapı (Azure uyumlu)  

---

## 🔐 Authentication & Authorization

- Register endpoint’i kapalıdır  
- Sadece **Login** üzerinden erişim sağlanır  
- JWT Token tabanlı kimlik doğrulama  
- Role-based authorization (Admin / Employee)  
- Hassas veriler DTO’lar aracılığıyla izole edilmiştir  

---

## 🔄 Sistem Akışı

1. Kullanıcı login olur ve JWT Token alır  
2. Token üzerinden rol bazlı yetkilendirme yapılır  
3. Admin kullanıcı ve görev yönetimi yapar  
4. Employee görevlerini takip eder ve izin talebi oluşturur  
5. Tüm işlemler **SystemLog** tablosuna kaydedilir  

---

## 🖥️ Uygulama Ekran Görüntüleri

### 👤 Kullanıcı Yönetimi
![Kullanıcı Yönetimi](https://github.com/user-attachments/assets/a16cbbbe-b7d6-4c0e-b0b0-517cb55ec2b1)

### 📋 Görev Yönetimi
![Görev Yönetimi](https://github.com/user-attachments/assets/b9b69161-f218-45f1-a27e-eadc0728b8a8)

### 🏖️ İzin Talepleri
![İzin Talepleri](https://github.com/user-attachments/assets/ac183bb5-b62f-406b-85bb-d5bf19c06a6d)

### 🧾 Sistem Logları
![Log Yönetimi](https://github.com/user-attachments/assets/6c9ae6c5-532f-4265-bd45-4d5bcfcc0760)

### 👨‍💼 Admin Paneli
![Admin Paneli](https://github.com/user-attachments/assets/df419457-e269-4236-bb01-5586192103e4)

### 👨‍💻 Kullanıcı Paneli
![Kullanıcı Paneli](https://github.com/user-attachments/assets/61aee22c-4a77-4873-ae18-ed360ef94d8e)

### 🗂️ Kart Bazlı Görev Sistemi
![Kart Sistemi](https://github.com/user-attachments/assets/dee42aa7-a10a-4ca3-8d49-e319c29a0b1a)

### 📝 İzin Talep Ekranı
![İzin Talebi](https://github.com/user-attachments/assets/26092ebe-5993-4389-81ec-7cbbec41364f)

---

## 🐳 Docker & Containerization

Uygulama tamamen **container-based** olarak çalışacak şekilde yapılandırılmıştır.

- Backend ve Frontend için **multi-stage Dockerfile**
- Frontend production build’i **Nginx** ile sunulur  
- MSSQL ayrı bir container olarak çalışır  
- Veritabanı verileri **Docker volume** ile kalıcıdır  
- Container’lar arası iletişim **Docker network** üzerinden sağlanır  

**Çalışan Container’lar:**
- `employee-api` → ASP.NET Core Web API  
- `employee-frontend` → React + Nginx  
- `employee-mssql` → SQL Server  

---

## 🚀 Kurulum (Docker)

Projeyi local ortamda çalıştırmak için:

```bash
git clone https://github.com/USERNAME/REPO_NAME.git
cd REPO_NAME
docker compose up -d --build
