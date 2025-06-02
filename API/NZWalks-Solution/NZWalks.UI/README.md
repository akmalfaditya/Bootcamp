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

