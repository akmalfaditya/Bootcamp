# NZWalks.UI - Web Application Frontend

## 🎯 Project Overview
Welcome to **NZWalks.UI** - the user-friendly web application frontend for the NZWalks system! This ASP.NET Core MVC project demonstrates how to build a modern web interface that seamlessly integrates with RESTful APIs, providing an intuitive experience for managing New Zealand's walking regions.

## 🚀 What You'll Learn
This UI project is your practical guide to mastering:
- **ASP.NET Core MVC** architecture and patterns
- **HTTP Client** integration for API consumption
- **Bootstrap** for responsive web design
- **Razor Views** with strongly-typed models
- **Form Handling** and data validation
- **RESTful API Integration** from a client perspective
- **Modern Web UI** patterns and best practices

## 🏗️ Architecture & Design

### 🗂️ Project Structure
```
NZWalks.UI/
├── Controllers/          # MVC Controllers for handling requests
│   ├── HomeController.cs # Landing page and general navigation
│   └── RegionsController.cs # Region management operations
├── Views/               # Razor view templates
│   ├── Home/           # Home page views
│   ├── Regions/        # Region management views
│   │   ├── Index.cshtml    # Region listing page
│   │   ├── Add.cshtml      # Create new region form
│   │   └── Edit.cshtml     # Edit existing region form
│   └── Shared/         # Shared layouts and partials
├── Models/             # ViewModels and DTOs
│   ├── DTO/           # Data Transfer Objects
│   └── ViewModels/    # Page-specific view models
└── wwwroot/           # Static files (CSS, JS, images)
```

### 🎨 MVC Pattern Implementation
- **Models** - Data structures and ViewModels for views
- **Views** - Razor templates for user interface
- **Controllers** - Handle user input and coordinate with API

## 🔧 Core Features

### 🏠 **Home Dashboard**
- **Welcome Page** - Application overview and navigation
- **Bootstrap Navigation** - Responsive menu system
- **Clean Layout** - Professional appearance with consistent styling

### 📍 **Region Management Interface**
- **📋 List Regions** - View all New Zealand regions in a table
- **➕ Add Region** - Create new regions with validation
- **✏️ Edit Region** - Update existing region information  
- **🗑️ Delete Region** - Remove regions with confirmation
- **🖼️ Image Support** - Display and manage region images

## 🌐 API Integration

### 🔌 **HTTP Client Configuration**
```csharp
// Program.cs - HTTP Client setup
builder.Services.AddHttpClient();

// Controller - API communication
public class RegionsController : Controller
{
    private readonly IHttpClientFactory httpClientFactory;
    
    public RegionsController(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }
}
```

### 📡 **RESTful API Calls**
```csharp
// GET - Fetch all regions
var client = httpClientFactory.CreateClient();
var response = await client.GetAsync("https://localhost:7081/api/regions");
var regions = await response.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>();

// POST - Create new region
var httpRequestMessage = new HttpRequestMessage()
{
    Method = HttpMethod.Post,
    RequestUri = new Uri("https://localhost:7081/api/regions"),
    Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
};
```

## 📱 User Interface Design

### 🎨 **Bootstrap Integration**
- **Responsive Design** - Works on desktop, tablet, and mobile
- **Professional Styling** - Clean, modern appearance
- **Form Validation** - Client-side and server-side validation
- **Interactive Elements** - Buttons, tables, and navigation

### 📋 **Region Management Views**

#### **Index View** - Region Listing
```html
<table class="table table-bordered">
    <thead>
        <tr>
            <th>Id</th>
            <th>Code</th>
            <th>Name</th>
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var region in Model)
        {
            <tr>
                <td>@region.Id</td>
                <td>@region.Code</td>
                <td>@region.Name</td>
                <td>
                    <a asp-action="Edit" asp-route-id="@region.Id" 
                       class="btn btn-light">Edit</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

#### **Add/Edit Forms** - Region Management
```html
<form method="post">
    <div class="mt-3">
        <label class="form-label">Code</label>
        <input type="text" class="form-control" asp-for="Code" />
    </div>
    <div class="mt-3">
        <label class="form-label">Name</label>
        <input type="text" class="form-control" asp-for="Name" />
    </div>
    <div class="mt-3">
        <label class="form-label">Image URL</label>
        <input type="text" class="form-control" asp-for="RegionImageUrl" />
    </div>
    <button type="submit" class="btn btn-primary">Save</button>
</form>
```

## 🗃️ Data Models

### 📊 **Data Transfer Objects (DTOs)**
```csharp
// RegionDto - API response model
public class RegionDto
{
    public Guid Id { get; set; }
    public string Code { get; set; }      // e.g., "AKL", "WGN"
    public string Name { get; set; }      // e.g., "Auckland", "Wellington"  
    public string? RegionImageUrl { get; set; }
}
```

### 📝 **ViewModels**
```csharp
// AddRegionViewModel - Form input model
public class AddRegionViewModel
{
    public string Code { get; set; }
    public string Name { get; set; }
    public string RegionImageUrl { get; set; }
}
```

## 🛠️ Key Dependencies

```xml
<!-- Minimal dependencies for focused learning -->
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

## 🚀 Getting Started

### 1️⃣ **Prerequisites**
- .NET 8.0 SDK
- Running NZWalks.API instance
- Modern web browser

### 2️⃣ **Configuration**
```csharp
// Program.cs - Basic setup
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();  // For API communication

var app = builder.Build();

// Configure pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### 3️⃣ **Run the Application**
```bash
# Ensure NZWalks.API is running first
dotnet run --project NZWalks.API

# Then start the UI application
dotnet run --project NZWalks.UI
# UI will be available at https://localhost:7123
```

## 🎓 **Learning Opportunities**

### 🔍 **MVC Pattern Deep Dive**
Explore how the application demonstrates:
- **Controller Actions** - Handling GET and POST requests
- **Model Binding** - Automatic form data mapping
- **View Rendering** - Razor syntax and HTML helpers
- **Routing** - URL patterns and route constraints

### 🌐 **API Integration Patterns**
Learn essential techniques:
- **HTTP Client Factory** - Proper HTTP client lifecycle management
- **JSON Serialization** - Converting between objects and JSON
- **Error Handling** - Graceful API error management
- **Async/Await** - Non-blocking API calls

### 🎨 **Frontend Development**
Master modern web UI:
- **Bootstrap Classes** - Responsive grid and components
- **Form Validation** - User input validation patterns
- **CRUD Operations** - Complete Create, Read, Update, Delete workflow
- **User Experience** - Intuitive navigation and feedback

## 🔧 **Hands-On Exercises**

### 🏋️‍♂️ **Try These Challenges:**
1. **Add Validation** - Implement client-side form validation
2. **Error Handling** - Add try-catch blocks for API failures
3. **Loading States** - Show spinners during API calls
4. **Confirmation Dialogs** - Add delete confirmation modals
5. **Pagination** - Implement page navigation for large datasets

### 🚀 **Extension Ideas:**
1. **Walk Management** - Create UI for managing walks
2. **Image Previews** - Show image previews in forms
3. **Search Functionality** - Add region search capabilities
4. **User Authentication** - Integrate login/logout features

## 🎯 **Key Takeaways**

This UI project demonstrates:
- **Clean Separation** - UI logic separate from API logic
- **RESTful Integration** - Proper API consumption patterns
- **Modern Web Standards** - Responsive, accessible design
- **Maintainable Code** - Clear structure and naming conventions

Perfect for developers learning how to build web applications that consume APIs effectively!

## 🔮 **Next Steps**
1. **Explore Controllers** - See how HTTP requests are handled
2. **Study Views** - Learn Razor syntax and HTML helpers
3. **Test API Integration** - Try different API scenarios
4. **Customize Styling** - Modify Bootstrap classes and CSS
5. **Add Features** - Implement additional CRUD operations

This frontend application showcases modern web development practices while maintaining simplicity and clarity - ideal for understanding how web applications interact with APIs!

---

*Build beautiful, functional web interfaces! 🌟 Master these patterns and create amazing user experiences.*

## 📚 **Step-by-Step Implementation Guide**

Ready to build this web application from scratch? Follow this comprehensive guide to create a modern MVC application that seamlessly integrates with RESTful APIs.

### 🎯 **Phase 1: Project Setup & Foundation**

#### **Step 1: Create MVC Project**
```bash
# Navigate to solution directory
cd NZWalks-Solution

# Create new MVC project
dotnet new mvc -n "NZWalks.UI" -f net8.0

# Add to solution
dotnet sln add NZWalks.UI/NZWalks.UI.csproj

# Navigate to UI project
cd NZWalks.UI
```

#### **Step 2: Project Structure Setup**
```bash
# Create folder structure
mkdir Models/DTO
mkdir Models/ViewModels

# Clean up default files (optional)
# Remove WeatherForecast.cs and default controller if not needed
```

#### **Step 3: Basic Project Configuration**

**Verify `NZWalks.UI.csproj`:**
```xml
<Project Sdk="Microsoft.NET.Sdk.Web">
  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
</Project>
```

### 🎯 **Phase 2: Data Models & DTOs**

#### **Step 4: Create Data Transfer Objects**

**Create `Models/DTO/RegionDto.cs`:**
```csharp
namespace NZWalks.UI.Models.DTO
{
    public class RegionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string? RegionImageUrl { get; set; }
    }
}
```

#### **Step 5: Create ViewModels**

**Create `Models/AddRegionViewModel.cs`:**
```csharp
namespace NZWalks.UI.Models
{
    public class AddRegionViewModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string RegionImageUrl { get; set; }
    }
}
```

### 🎯 **Phase 3: Program.cs Configuration**

#### **Step 6: Configure Services**

**Update `Program.cs`:**
```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add HTTP Client for API communication
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### 🎯 **Phase 4: Regions Controller Implementation**

#### **Step 7: Create Regions Controller**

**Create `Controllers/RegionsController.cs`:**
```csharp
using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDto> response = new List<RegionDto>();

            try
            {
                // Get All Regions from Web API
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.GetAsync("https://localhost:7081/api/regions");

                httpResponseMessage.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
            }
            catch (Exception ex)
            {
                // Log the exception
                // Handle error appropriately
            }

            return View(response);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://localhost:7081/api/regions"),
                Content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var client = httpClientFactory.CreateClient();

            var response = await client.GetFromJsonAsync<RegionDto>($"https://localhost:7081/api/regions/{id.ToString()}");

            if (response is not null)
            {
                return View(response);
            }

            return View(null);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(RegionDto request)
        {
            var client = httpClientFactory.CreateClient();

            var httpRequestMessage = new HttpRequestMessage()
            {
                Method = HttpMethod.Put,
                RequestUri = new Uri($"https://localhost:7081/api/regions/{request.Id}"),
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            httpResponseMessage.EnsureSuccessStatusCode();

            var response = await httpResponseMessage.Content.ReadFromJsonAsync<RegionDto>();

            if (response is not null)
            {
                return RedirectToAction("Index", "Regions");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(RegionDto request)
        {
            try
            {
                var client = httpClientFactory.CreateClient();

                var httpResponseMessage = await client.DeleteAsync($"https://localhost:7081/api/regions/{request.Id}");

                httpResponseMessage.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Regions");
            }
            catch (Exception ex)
            {
                // Handle error
            }

            return View("Edit");
        }
    }
}
```

### 🎯 **Phase 5: Razor Views Implementation**

#### **Step 8: Create Regions Index View**

**Create `Views/Regions/Index.cshtml`:**
```html
@model IEnumerable<NZWalks.UI.Models.DTO.RegionDto>

@{
    ViewData["Title"] = "Regions";
}

<h1 class="mt-3">Regions</h1>

<div class="d-flex justify-content-end mb-3">
    <a class="btn btn-secondary" asp-controller="Regions" asp-action="Add">Add Region</a>
</div>

<table class="table table-bordered">
    <thead>
        <tr>
            <th>Id</th>
            <th>Code</th>
            <th>Name</th>
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var region in Model)
        {
            <tr>
                <td>@region.Id</td>
                <td>@region.Code</td>
                <td>@region.Name</td>
                <td>
                    <a asp-controller="Regions" asp-action="Edit" asp-route-id="@region.Id"
                       class="btn btn-light">Edit</a>
                </td>
            </tr>
        }
    </tbody>
</table>
```

#### **Step 9: Create Add Region View**

**Create `Views/Regions/Add.cshtml`:**
```html
@model NZWalks.UI.Models.AddRegionViewModel

@{
    ViewData["Title"] = "Add Region";
}

<h1 class="mt-3">Add Region</h1>

<form method="post">
    <div class="mt-3">
        <label class="form-label">Code</label>
        <input type="text" class="form-control" asp-for="Code" />
    </div>

    <div class="mt-3">
        <label class="form-label">Name</label>
        <input type="text" class="form-control" asp-for="Name" />
    </div>

    <div class="mt-3">
        <label class="form-label">Image URL</label>
        <input type="text" class="form-control" asp-for="RegionImageUrl" />
    </div>

    <div class="mt-3">
        <button type="submit" class="btn btn-primary">Save</button>
        <a class="btn btn-secondary" asp-controller="Regions" asp-action="Index">Cancel</a>
    </div>
</form>
```

#### **Step 10: Create Edit Region View**

**Create `Views/Regions/Edit.cshtml`:**
```html
@model NZWalks.UI.Models.DTO.RegionDto

@{
    ViewData["Title"] = "Edit Region";
}

<h1 class="mt-3">Edit Region</h1>

@if (Model is not null)
{
    <form method="post">
        <div class="mt-3">
            <label class="form-label">Id</label>
            <input type="text" class="form-control" asp-for="Id" readonly />
        </div>

        <div class="mt-3">
            <label class="form-label">Code</label>
            <input type="text" class="form-control" asp-for="Code" />
        </div>

        <div class="mt-3">
            <label class="form-label">Name</label>
            <input type="text" class="form-control" asp-for="Name" />
        </div>

        <div class="mt-3">
            <label class="form-label">Image URL</label>
            <input type="text" class="form-control" asp-for="RegionImageUrl" />
        </div>

        <div class="mt-3 d-flex justify-content-between">
            <button type="submit" class="btn btn-primary">Save</button>

            <button type="submit" asp-controller="Regions"
                    asp-action="Delete"
                    class="btn btn-danger"
                    onclick="return confirm('Are you sure you want to delete this region?')">Delete</button>
        </div>

        <div class="mt-3">
            <a class="btn btn-secondary" asp-controller="Regions" asp-action="Index">Back to List</a>
        </div>
    </form>
}
else
{
    <p class="alert alert-danger">Region not found.</p>
    <a class="btn btn-secondary" asp-controller="Regions" asp-action="Index">Back to List</a>
}
```

### 🎯 **Phase 6: Navigation & Layout Updates**

#### **Step 11: Update Navigation**

**Update `Views/Shared/_Layout.cshtml` navigation section:**
```html
<div class="navbar-collapse collapse d-sm-inline-flex justify-content-between">
    <ul class="navbar-nav flex-grow-1">
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Index">Home</a>
        </li>
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-controller="Regions" asp-action="Index">Regions</a>
        </li>
        <li class="nav-item">
            <a class="nav-link text-dark" asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
        </li>
    </ul>
</div>
```

#### **Step 12: Update Home Page (Optional)**

**Update `Views/Home/Index.cshtml`:**
```html
@{
    ViewData["Title"] = "Home Page";
}

<div class="text-center">
    <h1 class="display-4">Welcome to NZ Walks</h1>
    <p class="lead">Discover and manage beautiful walking trails across New Zealand</p>
    
    <div class="mt-4">
        <a class="btn btn-primary btn-lg" asp-controller="Regions" asp-action="Index">
            Explore Regions
        </a>
    </div>
</div>

<div class="row mt-5">
    <div class="col-md-4">
        <h3>🏔️ Beautiful Regions</h3>
        <p>Explore New Zealand's stunning geographical regions from Auckland to Southland.</p>
    </div>
    <div class="col-md-4">
        <h3>🚶‍♂️ Walking Trails</h3>
        <p>Discover walking trails suited for all difficulty levels and preferences.</p>
    </div>
    <div class="col-md-4">
        <h3>📱 Easy Management</h3>
        <p>Simple and intuitive interface for managing regions and walking trails.</p>
    </div>
</div>
```

### 🎯 **Phase 7: Testing & Validation**

#### **Step 13: Configuration Verification**

**Ensure API URL consistency:**
- Verify that the API is running on `https://localhost:7081`
- Update API URLs in the controller if your API runs on a different port
- Test API connectivity before running the UI application

#### **Step 14: Build and Run**

```bash
# Build the project
dotnet build

# Run the application
dotnet run

# Application will be available at https://localhost:7123 (or your configured port)
```

#### **Step 15: Test CRUD Operations**

1. **Navigate to Home Page** - Verify layout and navigation
2. **View Regions List** - Test GET operation
3. **Add New Region** - Test POST operation
4. **Edit Existing Region** - Test PUT operation  
5. **Delete Region** - Test DELETE operation

### 🎯 **Phase 8: Enhanced Features (Optional)**

#### **Step 16: Add Error Handling**

**Enhance controller with better error handling:**
```csharp
[HttpGet]
public async Task<IActionResult> Index()
{
    List<RegionDto> response = new List<RegionDto>();

    try
    {
        var client = httpClientFactory.CreateClient();
        var httpResponseMessage = await client.GetAsync("https://localhost:7081/api/regions");

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            response.AddRange(await httpResponseMessage.Content.ReadFromJsonAsync<IEnumerable<RegionDto>>());
        }
        else
        {
            // Log error and show user-friendly message
            ViewBag.ErrorMessage = "Unable to load regions. Please try again later.";
        }
    }
    catch (HttpRequestException ex)
    {
        // Handle network errors
        ViewBag.ErrorMessage = "Unable to connect to the server. Please ensure the API is running.";
    }
    catch (Exception ex)
    {
        // Handle other errors
        ViewBag.ErrorMessage = "An unexpected error occurred.";
    }

    return View(response);
}
```

#### **Step 17: Add Loading States**

**Add loading indicators to views:**
```html
<!-- Add to forms for better UX -->
<button type="submit" class="btn btn-primary" id="submitBtn">
    <span class="spinner-border spinner-border-sm d-none" role="status"></span>
    Save
</button>

<script>
document.querySelector('form').addEventListener('submit', function() {
    const btn = document.getElementById('submitBtn');
    const spinner = btn.querySelector('.spinner-border');
    
    btn.disabled = true;
    spinner.classList.remove('d-none');
});
</script>
```

#### **Step 18: Add Client-Side Validation**

**Enhance forms with validation:**
```html
<div class="mt-3">
    <label class="form-label">Code</label>
    <input type="text" class="form-control" asp-for="Code" required maxlength="3" />
    <div class="invalid-feedback">
        Please provide a valid 3-character code.
    </div>
</div>
```

## 🎓 **Implementation Best Practices**

### ✅ **Patterns Demonstrated**
1. **MVC Pattern** - Clear separation of Model, View, Controller
2. **HTTP Client Pattern** - Proper API communication
3. **DTO Pattern** - Data transfer between API and UI
4. **Repository Pattern** - Clean data access (through API)
5. **Error Handling** - Graceful failure management

### 🔧 **Key Learning Points**
1. **API Integration** - How to consume RESTful APIs from MVC
2. **Form Handling** - POST/PUT operations with model binding
3. **Routing** - MVC routing conventions and custom routes
4. **Razor Syntax** - Server-side rendering with C# code
5. **Bootstrap Integration** - Responsive UI components

### 🚀 **Extension Opportunities**
1. **Authentication** - Add user login/logout functionality
2. **Validation** - Implement comprehensive form validation
3. **Pagination** - Handle large datasets efficiently
4. **Search & Filter** - Add search capabilities
5. **File Upload** - Implement image upload for regions

## 🎯 **Troubleshooting Guide**

### ❌ **Common Issues**
1. **API Connection Failed** - Ensure NZWalks.API is running on correct port
2. **CORS Errors** - Add CORS configuration to API if needed
3. **JSON Serialization Issues** - Verify DTO property names match API response
4. **Routing Problems** - Check controller and action names in views

### ✅ **Solutions**
1. **Test API First** - Use Swagger UI to verify API functionality
2. **Check Network Tab** - Use browser dev tools to inspect HTTP requests
3. **Add Logging** - Implement logging for debugging API calls
4. **Use Try-Catch** - Wrap API calls in proper exception handling

This comprehensive guide provides a solid foundation for building modern web applications that integrate with RESTful APIs. Each step builds upon the previous one, creating a complete understanding of MVC development patterns and API consumption techniques!

