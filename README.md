# 🧩 Employee Task & Report Management System

Kurumsal mimariye uygun olarak geliştirilmiş, şirket içi **görev**, **izin** ve **log** yönetimini
merkezi ve güvenli bir yapı altında toplayan Full Stack bir uygulama.

Proje; **.NET 8 Web API**, **React**, **Clean Architecture**, **JWT Authentication**,
**Docker**, **CI/CD** ve **Azure** odaklı gerçek bir iş senaryosu üzerine kurulmuştur.

---

## 🎯 Projenin Amacı

Şirket içerisinde;

- Kullanıcıların (Admin / Employee) yönetilmesi
- Görevlerin oluşturulması, atanması ve takip edilmesi
- İzin taleplerinin yönetilmesi
- Sistem üzerindeki tüm işlemlerin loglanması
- Yetkilendirme ve güvenli erişim sağlanması

süreçlerini **ölçeklenebilir, sürdürülebilir ve izlenebilir** bir mimari ile yönetmek.

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

- Katmanlar arası bağımlılıklar tersine çevrilmiştir  
- İş kuralları altyapıdan tamamen ayrılmıştır  
- Test edilebilir ve genişletilebilir yapı hedeflenmiştir  

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
- Global Exception Handling
- Serilog Logging

### Frontend
- React
- Component-based UI yapısı

### DevOps & Deployment
- Docker
- Docker Compose
- CI/CD Pipeline
- Azure Container Deployment

---

## 🔐 Authentication & Authorization

- Register kapalı, sadece **Login** aktif
- JWT Token tabanlı kimlik doğrulama
- Role-based authorization (Admin / Employee)
- Hassas veriler DTO’lar ile dış dünyadan izole edilmiştir

---

## 🔄 Sistem Akışı

1. Kullanıcı login olur ve JWT Token alır  
2. Role’a göre yetkilendirme yapılır  
3. Admin:
   - Kullanıcı ve görev yönetimi yapar
   - İzin taleplerini değerlendirir
4. Employee:
   - Görevlerini takip eder
   - İzin talebi oluşturur
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

## 🚀 Kurulum

```bash
git clone https://github.com/USERNAME/REPO_NAME.git
docker-compose up --build








