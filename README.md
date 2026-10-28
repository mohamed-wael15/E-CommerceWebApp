# E-Commerce Web App — ASP.NET Core MVC

An online store built with **ASP.NET Core 8 MVC** as a **training project**, still a work in progress.
It covers a product catalog with paging and search, a shopping cart, and ASP.NET Core Identity
with Google sign-in.

`C#` · `ASP.NET Core 8 MVC` · `Razor` · `EF Core` · `SQL Server` · `ASP.NET Core Identity` · `Google authentication`

---

## Features

- **Product catalog** — paginated product list and search, product details
- **Product management** — create (with an image upload), update and delete products
- **Categories** — create, update and delete
- **Shopping cart** — add items, change quantities, remove items (logged-in users)
- **Accounts** — register, log in, log out, forgot / reset password by email, and sign in with Google (ASP.NET Core Identity UI)
- **Seeding** — a default admin account with an `IsAdmin` claim is created on startup

---

## Project structure

```
E-CommerceWebApp/
├── Controllers/              Home, Product, Category, Cart
├── Areas/Identity/           scaffolded Identity pages (login, register, external login, password reset)
├── Models/                   Product, ProductImage, Category, Cart, CartItem, Order, OrderItem, ApplicationUser
├── Services/                 repositories, image upload, email sender, Google login setup, seed data
├── Authorization/            custom claim and policy names
└── Data/                     ApplicationDbContext and EF Core migrations
```

---

## Running locally

**Prerequisites:** .NET 8 SDK and SQL Server (or LocalDB).

```bash
git clone https://github.com/mohamed-wael15/E-CommerceWebApp.git
cd E-CommerceWebApp
```

1. Check the `DefaultConnection` string in `E-CommerceWebApp/appsettings.json`.
2. Set the Google and email settings with user secrets, since they are empty in `appsettings.json`.
   The Google sign-in settings are required for the app to run.

   ```bash
   dotnet user-secrets init --project E-CommerceWebApp
   dotnet user-secrets set "Authentication:Google:ClientId" "<client id>" --project E-CommerceWebApp
   dotnet user-secrets set "Authentication:Google:ClientSecret" "<client secret>" --project E-CommerceWebApp
   dotnet user-secrets set "EmailServerSettings:Host" "<smtp host>" --project E-CommerceWebApp
   dotnet user-secrets set "EmailServerSettings:Username" "<smtp user>" --project E-CommerceWebApp
   dotnet user-secrets set "EmailServerSettings:Password" "<smtp password>" --project E-CommerceWebApp
   dotnet user-secrets set "EmailServerSettings:SenderEmail" "<sender email>" --project E-CommerceWebApp
   ```

3. Create the database and run the app:

   ```bash
   dotnet ef database update --project E-CommerceWebApp
   dotnet run --project E-CommerceWebApp
   ```

---

## Status and known limitations

This project is a work in progress. Not implemented yet:

- **Checkout and orders** — the `Order` and `OrderItem` models and an order repository exist, but there is no checkout flow or order pages
- **Admin-only access** — an `IsAdmin` policy is defined, but product and category management are not restricted to it yet
- Wishlist, reviews and ratings, and inventory management
- Automated tests
