<div align="center">

<img src="https://capsule-render.vercel.app/api?type=waving&color=0:667eea,100:764ba2&height=200&section=header&text=EduBook%20Platform&fontSize=60&fontColor=ffffff&animation=fadeIn&fontAlignY=35&desc=The%20Next%20Generation%20Digital%20Learning%20Ecosystem&descAlignY=55&descSize=18" width="100%"/>

<br/>

**Kindle × Google Books × LMS — Built for South Asia**

*A production-grade educational eBook platform with DRM protection, local payments, and offline reading.*

<br/>

![ASP.NET Core](https://img.shields.io/badge/ASP.NET_Core-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Flutter](https://img.shields.io/badge/Flutter-3.x-02569B?style=for-the-badge&logo=flutter&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)
![Redis](https://img.shields.io/badge/Redis-7-DC382D?style=for-the-badge&logo=redis&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Ready-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![AWS S3](https://img.shields.io/badge/AWS_S3-Storage-FF9900?style=for-the-badge&logo=amazons3&logoColor=white)

<br/>

![Build](https://img.shields.io/badge/Build-Passing-brightgreen?style=flat-square)
![Architecture](https://img.shields.io/badge/Architecture-Clean-blueviolet?style=flat-square)
![Pattern](https://img.shields.io/badge/Pattern-CQRS-blue?style=flat-square)
![License](https://img.shields.io/badge/License-Private-red?style=flat-square)
![Status](https://img.shields.io/badge/Status-In_Development-orange?style=flat-square)

</div>

---

## ✨ Platform features

| 👤 Identity | 📚 Library | 💳 Commerce | 📖 Reading | 🔔 Engagement |
|:---:|:---:|:---:|:---:|:---:|
| JWT Auth | PDF Reader | One-time Purchase | Progress Sync | Push Notifications |
| OTP Verification | EPUB Reader | Monthly Subscription | Bookmarks | SignalR Real-time |
| RBAC (6 roles) | Offline Reading | bKash Integration | Highlights & Notes | Email Alerts |
| Refresh Tokens | Book Search | SSLCommerz | DRM Protection | Promo Campaigns |
| Device Management | Advanced Filters | Coupon System | Signed URLs | Subscription Reminders |

---

## 🏗️ System architecture

```mermaid
graph TB
    subgraph Clients["🖥️ Client layer"]
        A[📱 Flutter Mobile]
        B[🌐 Web Browser]
        C[⚙️ Admin Panel]
    end

    subgraph Gateway["🔀 Gateway layer"]
        D[Nginx · Rate Limiting · SSL · Load Balancer]
    end

    subgraph API["⚡ ASP.NET Core 10 — Backend services"]
        E[🔐 Auth Service]
        F[📚 Book Service]
        G[💳 Payment Service]
        H[📖 Reading & DRM]
        I[🔔 Notification Service]
        J[📊 Analytics Service]
    end

    subgraph Data["🗄️ Data layer"]
        K[(🐘 PostgreSQL)]
        L[(⚡ Redis)]
        M[(☁️ AWS S3 / R2)]
        N[(🔍 Elasticsearch)]
    end

    A & B & C --> D --> E & F & G & H & I & J
    E & F & G & H & I & J --> K & L & M & N
```

---

## 🧅 Clean architecture

<img width="1472" height="1240" alt="image" src="https://github.com/user-attachments/assets/de5a0a47-0ade-410c-bbe4-1e64e6f17365" />


---

## 🔐 Authentication flow

```mermaid
sequenceDiagram
    autonumber
    actor User
    participant API as ASP.NET Core API
    participant DB as PostgreSQL
    participant JWT as JWT Service

    User->>API: POST /api/v1/auth/register
    API->>DB: Check email/phone exists
    DB-->>API: Not found
    API->>API: BCrypt.Hash(password)
    API->>DB: Save User entity
    DB-->>API: User saved
    API->>JWT: GenerateAccessToken(user)
    JWT-->>API: JWT (60 min)
    API->>JWT: GenerateRefreshToken()
    JWT-->>API: Refresh token (7 days)
    API->>DB: Save RefreshToken
    API-->>User: { accessToken, refreshToken, userId }

    User->>API: GET /api/v1/books
    Note over User,API: Authorization: Bearer {token}
    API->>JWT: ValidateToken()
    JWT-->>API: Claims extracted
    API-->>User: { books data }
```

---

## ⚡ CQRS pattern

```mermaid
flowchart LR
    C[🌐 Controller] --> M{MediatR}

    M --> CMD[📝 Command\nWrite operation]
    M --> QRY[🔍 Query\nRead operation]

    CMD --> CH[Command Handler\n• Validate input\n• Business logic\n• Save to DB]
    QRY --> QH[Query Handler\n• Fetch from DB\n• Map to DTO\n• Return result]

    CH --> DB[(PostgreSQL)]
    QH --> DB

    style CMD fill:#7c3aed,color:#fff
    style QRY fill:#0369a1,color:#fff
    style CH  fill:#6d28d9,color:#fff
    style QH  fill:#0284c7,color:#fff
```

---

## 🔄 Request pipeline

```mermaid
flowchart TD
    A[📡 HTTP Request] --> B[🔀 Middleware\nLogging · CORS · Auth]
    B --> C[🛡️ JWT Validation]
    C --> D[📋 FluentValidation\nInput validation]
    D --> E{Valid?}
    E -- No  --> F[❌ 400 Bad Request]
    E -- Yes --> G[⚡ MediatR Dispatch]
    G --> H[🔧 Command / Query Handler]
    H --> I[🗄️ Repository / DbContext]
    I --> J[(PostgreSQL)]
    J --> K[✅ Response DTO]
    K --> L[📤 HTTP Response]

    style F fill:#dc2626,color:#fff
    style K fill:#16a34a,color:#fff
```

---

## 👥 User roles

```mermaid
flowchart LR
    S["👤 Student\nRead · Purchase\nTrack progress"]
    P["👨‍👩‍👦 Parent\nMonitor child\nManage subscription"]
    T["👨‍🏫 Teacher\nAssign books\nCreate reading lists"]
    I["🏫 Institution\nBulk license\nManage teachers"]
    A["🔧 Admin\nManage users & books\nFinancial reports"]
    SA["👑 Super Admin\nFull platform access\nSystem configuration"]

    S --> P --> T --> I --> A --> SA

    style S  fill:#E1F5EE,stroke:#1D9E75,color:#085041
    style P  fill:#E6F1FB,stroke:#378ADD,color:#0C447C
    style T  fill:#FAEEDA,stroke:#EF9F27,color:#633806
    style I  fill:#FAECE7,stroke:#D85A30,color:#712B13
    style A  fill:#EEEDFE,stroke:#7F77DD,color:#3C3489
    style SA fill:#085041,stroke:#1D9E75,color:#9FE1CB
```

| Role | Access level | Key permissions |
|:---|:---:|:---|
| 👤 Student | 20% | Browse, purchase, read, bookmark, track progress |
| 👨‍👩‍👦 Parent | 28% | All student perms + monitor child + manage child subscription |
| 👨‍🏫 Teacher | 38% | All parent perms + assign books + create reading lists |
| 🏫 Institution | 58% | All teacher perms + bulk license + upload books + org reports |
| 🔧 Admin | 80% | All institution perms + manage all users + financial management |
| 👑 Super Admin | 100% | Full access + system config + audit logs + ban/restore users |

---

## 🗄️ Database schema

```mermaid
erDiagram
    users {
        uuid id PK
        string full_name
        string email
        string phone_number
        string password_hash
        int role
        int status
        timestamp created_at
        timestamp deleted_at
    }

    refresh_tokens {
        uuid id PK
        uuid user_id FK
        string token
        timestamp expires_at
        timestamp revoked_at
        string device_info
        string ip_address
    }

    books {
        uuid id PK
        string title
        string description
        decimal price
        int status
        int format
        uuid publisher_id FK
        int grade_level
    }

    book_files {
        uuid id PK
        uuid book_id FK
        string storage_key
        int format
        bool is_encrypted
    }

    purchases {
        uuid id PK
        uuid user_id FK
        uuid book_id FK
        decimal price_paid
        int status
    }

    subscriptions {
        uuid id PK
        uuid user_id FK
        int plan
        int status
        timestamp start_date
        timestamp end_date
        bool auto_renew
    }

    reading_progress {
        uuid id PK
        uuid user_id FK
        uuid book_id FK
        int current_page
        int total_pages
        timestamp last_read_at
    }

    payment_transactions {
        uuid id PK
        uuid user_id FK
        decimal amount
        int gateway
        int status
        string gateway_transaction_id
        jsonb gateway_response
    }

    users ||--o{ refresh_tokens : has
    users ||--o{ purchases : makes
    users ||--o{ subscriptions : holds
    users ||--o{ reading_progress : tracks
    books ||--o{ book_files : contains
    books ||--o{ purchases : sold_in
    books ||--o{ reading_progress : tracked_in
    purchases ||--|| payment_transactions : paid_via
    subscriptions ||--|| payment_transactions : paid_via
```

---

## 🌐 API endpoints

### Authentication
| Method | Endpoint | Description | Auth |
|:---|:---|:---|:---:|
| POST | `/api/v1/auth/register` | Register new user | No |
| POST | `/api/v1/auth/login` | Login user | No |
| POST | `/api/v1/auth/refresh` | Refresh access token | No |
| POST | `/api/v1/auth/logout` | Logout user | Yes |
| POST | `/api/v1/auth/verify-otp` | Verify OTP code | No |

### Books
| Method | Endpoint | Description | Auth |
|:---|:---|:---|:---:|
| GET | `/api/v1/books` | Get all books | No |
| GET | `/api/v1/books/{id}` | Get book detail | No |
| POST | `/api/v1/books` | Upload book | Admin |
| PUT | `/api/v1/books/{id}` | Update book metadata | Admin |
| DELETE | `/api/v1/books/{id}` | Delete book | Admin |

### Reading
| Method | Endpoint | Description | Auth |
|:---|:---|:---|:---:|
| GET | `/api/v1/reading/{bookId}/url` | Get signed read URL | Yes |
| POST | `/api/v1/reading/{bookId}/progress` | Sync reading progress | Yes |
| GET | `/api/v1/reading/{bookId}/progress` | Get reading progress | Yes |
| POST | `/api/v1/reading/{bookId}/bookmarks` | Add bookmark | Yes |
| POST | `/api/v1/reading/{bookId}/highlights` | Add highlight | Yes |

### Commerce
| Method | Endpoint | Description | Auth |
|:---|:---|:---|:---:|
| POST | `/api/v1/purchases` | Purchase a book | Yes |
| GET | `/api/v1/purchases` | Get purchase history | Yes |
| POST | `/api/v1/subscriptions` | Create subscription | Yes |
| GET | `/api/v1/subscriptions` | Get active subscription | Yes |

---

## 🛠️ Technology stack

<details>
<summary><b>View full stack details</b></summary>

<br/>

**Backend**
| Technology | Version | Purpose |
|:---|:---:|:---|
| ASP.NET Core | 10.0 | Web API framework |
| MediatR | Latest | CQRS dispatcher |
| Entity Framework Core | Latest | ORM |
| FluentValidation | Latest | Input validation |
| BCrypt.Net | Latest | Password hashing |
| Serilog | Latest | Structured logging |
| SignalR | Built-in | Real-time communication |
| Swashbuckle | Latest | Swagger / OpenAPI |

**Mobile**
| Technology | Version | Purpose |
|:---|:---:|:---|
| Flutter | 3.x | Cross-platform mobile |
| Riverpod | Latest | State management |
| Dio | Latest | HTTP client |
| Go Router | Latest | Navigation |

**Database & cache**
| Technology | Version | Purpose |
|:---|:---:|:---|
| PostgreSQL | 16 | Primary datastore |
| Redis | 7 | Cache & sessions |
| Elasticsearch | 8.x | Full-text search (Phase 2) |

**Security**
| Technology | Purpose |
|:---|:---|
| JWT Bearer | Access token authentication |
| BCrypt | Password hashing (cost 12) |
| Signed URLs | DRM file access control |
| RBAC | Role-based access control |
| Refresh tokens | Secure session rotation |

**Infrastructure**
| Technology | Purpose |
|:---|:---|
| Docker | Containerization |
| GitHub Actions | CI/CD pipeline |
| AWS S3 / Cloudflare R2 | Book file storage |
| Nginx | Reverse proxy & load balancer |
| Kubernetes | Orchestration (Phase 2) |

</details>

---

## 📁 Project structure

<details>
<summary><b>View solution structure</b></summary>

```
edubook-platform/
│
├── 📁 backend/
│   ├── 📁 EduBook.API/                    ← Entry point
│   │   ├── Controllers/V1/
│   │   │   └── AuthController.cs
│   │   ├── Middleware/
│   │   ├── Extensions/
│   │   ├── Filters/
│   │   └── Program.cs
│   │
│   ├── 📁 EduBook.Application/            ← Use cases
│   │   ├── Common/
│   │   │   └── JwtSettings.cs
│   │   ├── Features/
│   │   │   ├── Auth/
│   │   │   │   ├── Commands/
│   │   │   │   │   ├── RegisterCommand.cs
│   │   │   │   │   └── RegisterCommandHandler.cs
│   │   │   │   ├── Queries/
│   │   │   │   └── DTOs/
│   │   │   ├── Books/Commands & Queries
│   │   │   ├── Reading/Commands & Queries
│   │   │   └── Purchases/Commands & Queries
│   │   ├── Interfaces/
│   │   │   ├── IApplicationDbContext.cs
│   │   │   ├── IJwtService.cs
│   │   │   └── IPasswordHasher.cs
│   │   └── Behaviors/
│   │
│   ├── 📁 EduBook.Domain/                 ← Business core
│   │   ├── Common/
│   │   │   └── BaseEntity.cs
│   │   ├── Entities/
│   │   │   ├── User.cs
│   │   │   ├── RefreshToken.cs
│   │   │   └── OtpCode.cs
│   │   ├── Enums/
│   │   │   ├── UserRole.cs
│   │   │   ├── UserStatus.cs
│   │   │   └── OtpType.cs
│   │   └── Events/
│   │
│   └── 📁 EduBook.Infrastructure/         ← Technical details
│       ├── Persistence/
│       │   └── AppDbContext.cs
│       ├── Configurations/
│       │   ├── UserConfiguration.cs
│       │   ├── RefreshTokenConfiguration.cs
│       │   └── OtpCodeConfiguration.cs
│       └── Services/
│           └── Auth/
│               ├── JwtService.cs
│               └── PasswordHasher.cs
│
├── 📁 mobile/                             ← Flutter app (Phase 2)
├── 📁 infrastructure/                     ← Docker & K8s configs
└── 📁 docs/                               ← Documentation
```

</details>

---

## 🗺️ Roadmap

```mermaid
gantt
    title EduBook Platform — Development roadmap
    dateFormat  YYYY-MM-DD
    section Phase 1 — Foundation (75%)
    Solution scaffold        :done,    p1a, 2026-01-01, 7d
    Domain entities          :done,    p1b, after p1a, 7d
    PostgreSQL + EF Core     :done,    p1c, after p1b, 7d
    JWT + Register API       :done,    p1d, after p1c, 7d
    Login & OTP API          :active,  p1e, after p1d, 7d
    Refresh token rotation   :         p1f, after p1e, 7d
    section Phase 2 — Content (0%)
    Book upload to S3        :         p2a, after p1f, 14d
    Search & filters         :         p2b, after p2a, 7d
    Admin panel MVC          :         p2c, after p2b, 14d
    section Phase 3 — Commerce (0%)
    PDF/EPUB reader APIs     :         p3a, after p2c, 14d
    DRM signed URLs          :         p3b, after p3a, 7d
    bKash + SSLCommerz       :         p3c, after p3b, 14d
    section Phase 4 — Mobile & Scale (0%)
    Flutter mobile app       :         p4a, after p3c, 28d
    Offline reading          :         p4b, after p4a, 14d
    Kubernetes deployment    :         p4c, after p4b, 14d
```

### Phase details

**Phase 1 — Foundation (75% complete)**
- [x] Clean Architecture solution scaffold
- [x] Domain entities (User, Book, Purchase, Subscription)
- [x] PostgreSQL + EF Core migrations
- [x] JWT authentication & BCrypt password hashing
- [x] User registration API (tested with Postman)
- [ ] Login API & OTP verification
- [ ] Refresh token rotation
- [ ] Device management

**Phase 2 — Content (0%)**
- [ ] Book upload (PDF & EPUB) to AWS S3
- [ ] Book metadata management
- [ ] Full-text search (PostgreSQL tsvector)
- [ ] Admin panel (ASP.NET MVC)
- [ ] Book approval workflow

**Phase 3 — Commerce & Reading (0%)**
- [ ] PDF & EPUB reader APIs
- [ ] Reading progress sync
- [ ] Bookmarks, highlights & notes
- [ ] DRM signed URL access
- [ ] One-time purchase flow
- [ ] Monthly subscription
- [ ] bKash payment integration
- [ ] SSLCommerz integration

**Phase 4 — Mobile & Scale (0%)**
- [ ] Flutter mobile app
- [ ] Offline reading (encrypted local storage)
- [ ] Elasticsearch integration
- [ ] Recommendation engine
- [ ] Kubernetes deployment
- [ ] Google & Facebook OAuth

---

## ⚙️ Getting started

### Prerequisites
- .NET 10 SDK
- PostgreSQL 16
- Redis (Memurai for Windows)
- Visual Studio 2022

### Quick start

```bash
git clone https://github.com/YOUR_USERNAME/edubook-platform.git
cd edubook-platform/backend
```

Update `EduBook.API/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=edubook_dev;Username=postgres;Password=YOUR_PASSWORD"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyHereMakeItLongAtLeast32Chars",
    "Issuer": "EduBook",
    "Audience": "EduBookUsers",
    "ExpiryMinutes": 60
  }
}
```

Run migrations (Package Manager Console in Visual Studio):
```powershell
Update-Database -StartupProject EduBook.API
```

Run the API:
```bash
cd EduBook.API
dotnet run
```

Open Swagger UI: `https://localhost:7001/swagger`

Test registration:
```bash
curl -X POST https://localhost:7001/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "fullName": "Test User",
    "email": "test@edubook.com",
    "phoneNumber": "01712345678",
    "password": "Test@123",
    "role": "Student"
  }'
```

---

## 🔒 Security

- Passwords hashed with **BCrypt** (cost factor 12)
- JWT tokens signed with **HMAC-SHA256**
- Refresh token rotation on every use
- Soft deletes — data never permanently removed
- Role-Based Access Control (RBAC) with 6 roles
- Signed URLs for DRM book file access
- Rate limiting on auth endpoints (coming soon)
- Full audit log trail (coming soon)

---

## 🤝 Contributing

```mermaid
gitGraph
    commit id: "main"
    branch dev
    checkout dev
    commit id: "dev base"
    branch feature/auth
    checkout feature/auth
    commit id: "add login"
    commit id: "add OTP"
    checkout dev
    merge feature/auth id: "merge auth"
    branch feature/books
    checkout feature/books
    commit id: "add upload"
    checkout dev
    merge feature/books id: "merge books"
    checkout main
    merge dev id: "release v1.0"
```

1. Fork the repository
2. Create your branch: `git checkout -b feature/your-feature`
3. Commit changes: `git commit -m "feat: add your feature"`
4. Push to branch: `git push origin feature/your-feature`
5. Open a Pull Request to `dev` branch

---

<div align="center">

**Built with ❤️ by Parvez**

*EduBook Platform — Empowering Education Across South Asia*

<img src="https://capsule-render.vercel.app/api?type=waving&color=0:667eea,100:764ba2&height=100&section=footer" width="100%"/>

</div>
