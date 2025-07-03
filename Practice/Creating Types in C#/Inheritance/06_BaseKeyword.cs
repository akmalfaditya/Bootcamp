using System;

namespace Inheritance
{
    /// <summary>
    /// This file demonstrates the 'base' keyword in C# inheritance.
    /// The 'base' keyword allows you to access members of the immediate base class
    /// from within a derived class. It's essential for calling base constructors
    /// and base implementations of overridden methods.
    /// 
    /// Key concepts:
    /// 1. Using 'base' to call base class constructors
    /// 2. Using 'base' to access overridden methods
    /// 3. Using 'base' to access base class properties and fields
    /// 4. Best practices for using 'base' keyword
    /// </summary>

    // Base class demonstrating various members that can be accessed via 'base'
    public class Employee
    {
        protected string _department = string.Empty; // Protected field accessible to derived classes
        
        public string Name { get; set; } = string.Empty;
        public int EmployeeId { get; set; }
        public DateTime HireDate { get; set; }
        public decimal BaseSalary { get; set; }
        
        // Base constructor with parameters
        public Employee(string name, int employeeId, string department, decimal baseSalary)
        {
            Name = name;
            EmployeeId = employeeId;
            _department = department;
            BaseSalary = baseSalary;
            HireDate = DateTime.Now;
            
            Console.WriteLine($"Employee base constructor called for {Name}");
        }
        
        // Parameterless constructor
        public Employee()
        {
            Console.WriteLine("Employee parameterless constructor called");
        }
        
        // Virtual method that can be overridden
        public virtual decimal CalculateAnnualSalary()
        {
            Console.WriteLine("Calculating base annual salary...");
            return BaseSalary * 12;
        }
        
        // Virtual method for bonus calculation
        public virtual decimal CalculateBonus()
        {
            Console.WriteLine("Calculating base bonus (5% of base salary)...");
            return BaseSalary * 0.05m;
        }
        
        // Virtual method for benefits
        public virtual decimal CalculateBenefits()
        {
            Console.WriteLine("Calculating base benefits (10% of base salary)...");
            return BaseSalary * 0.10m;
        }
        
        // Method to display basic employee info
        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Employee: {Name} (ID: {EmployeeId})");
            Console.WriteLine($"Department: {_department}");
            Console.WriteLine($"Hire Date: {HireDate:yyyy-MM-dd}");
            Console.WriteLine($"Base Salary: ${BaseSalary:F2}");
        }
        
        // Virtual method for performance review
        public virtual string GetPerformanceRating()
        {
            return "Satisfactory";
        }
        
        // Method that calls other virtual methods
        public virtual void GeneratePayrollReport()
        {
            Console.WriteLine($"\n=== Payroll Report for {Name} ===");
            Console.WriteLine($"Annual Salary: ${CalculateAnnualSalary():F2}");
            Console.WriteLine($"Annual Bonus: ${CalculateBonus():F2}");
            Console.WriteLine($"Annual Benefits: ${CalculateBenefits():F2}");
            Console.WriteLine($"Performance: {GetPerformanceRating()}");
            
            decimal totalCompensation = CalculateAnnualSalary() + CalculateBonus() + CalculateBenefits();
            Console.WriteLine($"Total Annual Compensation: ${totalCompensation:F2}");
        }
    }

    // First derived class - Manager
    public class Manager : Employee
    {
        public int TeamSize { get; set; }
        public decimal ManagementBonus { get; set; }
        
        // Constructor that calls base constructor using 'base'
        public Manager(string name, int employeeId, string department, decimal baseSalary, int teamSize, decimal managementBonus)
            : base(name, employeeId, department, baseSalary) // Calling base class constructor
        {
            TeamSize = teamSize;
            ManagementBonus = managementBonus;
            
            Console.WriteLine($"Manager constructor called. Managing {TeamSize} people.");
        }
        
        // Parameterless constructor that chains to base
        public Manager() : base() // Explicitly call base parameterless constructor
        {
            Console.WriteLine("Manager parameterless constructor called");
        }
        
        // Override method but call base implementation first
        public override decimal CalculateAnnualSalary()
        {
            Console.WriteLine("Manager salary calculation:");
            
            // Call the base implementation first
            decimal baseSalary = base.CalculateAnnualSalary();
            
            // Add manager-specific calculations
            decimal managementIncrease = baseSalary * 0.20m; // 20% increase for management
            Console.WriteLine($"Management increase: ${managementIncrease:F2}");
            
            return baseSalary + managementIncrease;
        }
        
        // Override bonus calculation, incorporating base calculation
        public override decimal CalculateBonus()
        {
            Console.WriteLine("Manager bonus calculation:");
            
            // Start with base bonus calculation
            decimal baseBonus = base.CalculateBonus();
            Console.WriteLine($"Base bonus: ${baseBonus:F2}");
            
            // Add management bonus
            Console.WriteLine($"Management bonus: ${ManagementBonus:F2}");
            
            // Add team size bonus
            decimal teamBonus = TeamSize * 500; // $500 per team member
            Console.WriteLine($"Team size bonus: ${teamBonus:F2}");
            
            return baseBonus + ManagementBonus + teamBonus;
        }
        
        // Override method and extend base functionality
        public override void DisplayInfo()
        {
            // Call base implementation first
            base.DisplayInfo();
            
            // Add manager-specific info
            Console.WriteLine($"Team Size: {TeamSize}");
            Console.WriteLine($"Management Bonus: ${ManagementBonus:F2}");
            Console.WriteLine("Role: Manager");
        }
        
        public override string GetPerformanceRating()
        {
            // Managers have enhanced performance evaluation
            string baseRating = base.GetPerformanceRating();
            return $"{baseRating} - Leadership Qualities Demonstrated";
        }
        
        // Manager-specific method that uses base class members
        public void ConductTeamMeeting()
        {
            // Access protected field from base class
            Console.WriteLine($"Manager {Name} is conducting a team meeting for {_department} department.");
            Console.WriteLine($"Team size: {TeamSize} members");
        }
    }

    // Second derived class - SalesRep
    public class SalesRep : Employee
    {
        public decimal CommissionRate { get; set; }
        public decimal MonthlyQuota { get; set; }
        public decimal CurrentMonthSales { get; set; }
        
        public SalesRep(string name, int employeeId, decimal baseSalary, decimal commissionRate, decimal monthlyQuota)
            : base(name, employeeId, "Sales", baseSalary) // Call base constructor with specific department
        {
            CommissionRate = commissionRate;
            MonthlyQuota = monthlyQuota;
            CurrentMonthSales = 0;
            
            Console.WriteLine($"SalesRep constructor called. Commission rate: {CommissionRate:P}");
        }
        
        // Override salary calculation to include commission
        public override decimal CalculateAnnualSalary()
        {
            Console.WriteLine("Sales representative salary calculation:");
            
            // Get base salary calculation
            decimal baseSalary = base.CalculateAnnualSalary();
            Console.WriteLine($"Base annual salary: ${baseSalary:F2}");
            
            // Calculate annual commission (assuming current month sales apply to all months)
            decimal annualCommission = (CurrentMonthSales * CommissionRate) * 12;
            Console.WriteLine($"Estimated annual commission: ${annualCommission:F2}");
            
            return baseSalary + annualCommission;
        }
        
        // Override bonus calculation based on quota achievement
        public override decimal CalculateBonus()
        {
            Console.WriteLine("Sales representative bonus calculation:");
            
            // Start with base bonus
            decimal baseBonus = base.CalculateBonus();
            
            // Add quota achievement bonus
            if (CurrentMonthSales >= MonthlyQuota)
            {
                decimal quotaBonus = MonthlyQuota * 0.10m; // 10% of quota as bonus
                Console.WriteLine($"Quota achievement bonus: ${quotaBonus:F2}");
                return baseBonus + quotaBonus;
            }
            else
            {
                Console.WriteLine("Quota not met - no additional bonus");
                return baseBonus;
            }
        }
        
        public override void DisplayInfo()
        {
            // Call base implementation
            base.DisplayInfo();
            
            // Add sales-specific information
            Console.WriteLine($"Commission Rate: {CommissionRate:P}");
            Console.WriteLine($"Monthly Quota: ${MonthlyQuota:F2}");
            Console.WriteLine($"Current Month Sales: ${CurrentMonthSales:F2}");
            Console.WriteLine("Role: Sales Representative");
        }
        
        public override string GetPerformanceRating()
        {
            string baseRating = base.GetPerformanceRating();
            
            if (CurrentMonthSales >= MonthlyQuota * 1.5m)
                return $"{baseRating} - Exceptional Sales Performance";
            else if (CurrentMonthSales >= MonthlyQuota)
                return $"{baseRating} - Quota Achieved";
            else
                return $"{baseRating} - Below Quota";
        }
        
        public void RecordSale(decimal saleAmount)
        {
            CurrentMonthSales += saleAmount;
            Console.WriteLine($"Sale recorded: ${saleAmount:F2}. Total this month: ${CurrentMonthSales:F2}");
            
            // Access base class properties
            Console.WriteLine($"Commission earned: ${saleAmount * CommissionRate:F2}");
        }
    }

    // Third derived class - Engineer  
    public class Engineer : Employee
    {
        public string Specialization { get; set; } = string.Empty;
        public int ProjectsCompleted { get; set; }
        public bool HasCertifications { get; set; }
        
        public Engineer(string name, int employeeId, string specialization, decimal baseSalary, bool hasCertifications)
            : base(name, employeeId, "Engineering", baseSalary)
        {
            Specialization = specialization;
            HasCertifications = hasCertifications;
            ProjectsCompleted = 0;
            
            Console.WriteLine($"Engineer constructor called. Specialization: {Specialization}");
        }
        
        public override decimal CalculateBonus()
        {
            Console.WriteLine("Engineer bonus calculation:");
            
            // Get base bonus
            decimal baseBonus = base.CalculateBonus();
            Console.WriteLine($"Base bonus: ${baseBonus:F2}");
            
            // Project completion bonus
            decimal projectBonus = ProjectsCompleted * 1000; // $1000 per project
            Console.WriteLine($"Project completion bonus: ${projectBonus:F2}");
            
            // Certification bonus
            decimal certificationBonus = HasCertifications ? 2000 : 0;
            if (HasCertifications)
                Console.WriteLine($"Certification bonus: ${certificationBonus:F2}");
            
            return baseBonus + projectBonus + certificationBonus;
        }
        
        public override decimal CalculateBenefits()
        {
            Console.WriteLine("Engineer benefits calculation:");
            
            // Engineers get enhanced benefits
            decimal baseBenefits = base.CalculateBenefits();
            Console.WriteLine($"Base benefits: ${baseBenefits:F2}");
            
            // Technical training allowance
            decimal trainingAllowance = 3000; // Annual training budget
            Console.WriteLine($"Training allowance: ${trainingAllowance:F2}");
            
            return baseBenefits + trainingAllowance;
        }
        
        public override void DisplayInfo()
        {
            base.DisplayInfo(); // Call base implementation
            
            Console.WriteLine($"Specialization: {Specialization}");
            Console.WriteLine($"Projects Completed: {ProjectsCompleted}");
            Console.WriteLine($"Has Certifications: {HasCertifications}");
            Console.WriteLine("Role: Engineer");
        }
        
        public void CompleteProject(string projectName)
        {
            ProjectsCompleted++;
            Console.WriteLine($"Engineer {Name} completed project: {projectName}");
            Console.WriteLine($"Total projects completed: {ProjectsCompleted}");
            
            // Access protected member from base class
            Console.WriteLine($"Project completed in {_department} department");
        }
    }

    /// <summary>
    /// Demonstration class for the 'base' keyword functionality
    /// </summary>
    public static class BaseKeywordDemo
    {
        public static void RunDemo()
        {
            Console.WriteLine("=== BASE KEYWORD DEMONSTRATION ===\n");
            
            // 1. DEMONSTRATE CONSTRUCTOR CHAINING WITH BASE
            Console.WriteLine("1. CONSTRUCTOR CHAINING:");
            Console.WriteLine("Notice how base constructors are called before derived constructors\n");
            
            var manager = new Manager("Alice Johnson", 1001, "IT", 8000, 10, 2000);
            Console.WriteLine();
            
            var salesRep = new SalesRep("Bob Smith", 1002, 5000, 0.05m, 50000);
            Console.WriteLine();
            
            var engineer = new Engineer("Carol Davis", 1003, "Software Development", 7500, true);
            Console.WriteLine();
            
            // 2. DEMONSTRATE BASE METHOD CALLS IN OVERRIDES
            Console.WriteLine("2. BASE METHOD CALLS IN OVERRIDDEN METHODS:");
            Console.WriteLine("Watch how derived classes extend base functionality\n");
            
            Console.WriteLine("--- Manager Annual Salary Calculation ---");
            decimal managerSalary = manager.CalculateAnnualSalary();
            Console.WriteLine($"Final manager salary: ${managerSalary:F2}\n");
            
            Console.WriteLine("--- SalesRep with Current Sales ---");
            salesRep.RecordSale(15000);
            salesRep.RecordSale(20000);
            salesRep.RecordSale(18000);
            decimal salesRepSalary = salesRep.CalculateAnnualSalary();
            Console.WriteLine($"Final sales rep salary: ${salesRepSalary:F2}\n");
            
            Console.WriteLine("--- Engineer Bonus Calculation ---");
            engineer.CompleteProject("Mobile App Redesign");
            engineer.CompleteProject("Database Optimization");
            engineer.CompleteProject("API Integration");
            decimal engineerBonus = engineer.CalculateBonus();
            Console.WriteLine($"Final engineer bonus: ${engineerBonus:F2}\n");
            
            // 3. DEMONSTRATE CALLING BASE DISPLAY METHODS
            Console.WriteLine("3. EXTENDING BASE DISPLAY FUNCTIONALITY:");
            Console.WriteLine("Each class calls base.DisplayInfo() then adds its own details\n");
            
            Console.WriteLine("--- Manager Info ---");
            manager.DisplayInfo();
            Console.WriteLine();
            
            Console.WriteLine("--- Sales Rep Info ---");
            salesRep.DisplayInfo();
            Console.WriteLine();
            
            Console.WriteLine("--- Engineer Info ---");
            engineer.DisplayInfo();
            Console.WriteLine();
            
            // 4. DEMONSTRATE PAYROLL REPORTS WITH BASE METHOD CALLS
            Console.WriteLine("4. PAYROLL REPORTS:");
            Console.WriteLine("GeneratePayrollReport calls multiple virtual methods\n");
            
            manager.GeneratePayrollReport();
            salesRep.GeneratePayrollReport();
            engineer.GeneratePayrollReport();
            
            // 5. DEMONSTRATE ACCESS TO PROTECTED MEMBERS
            Console.WriteLine("\n5. ACCESSING PROTECTED BASE MEMBERS:");
            Console.WriteLine("Derived classes can access protected members through methods\n");
            
            manager.ConductTeamMeeting();
            engineer.CompleteProject("Security Audit");
            
            // 6. POLYMORPHIC BEHAVIOR WITH BASE CALLS
            Console.WriteLine("\n6. POLYMORPHIC BEHAVIOR:");
            Console.WriteLine("Even through base references, overridden methods call derived implementations\n");
            
            Employee[] employees = { manager, salesRep, engineer };
            
            foreach (Employee emp in employees)
            {
                Console.WriteLine($"--- {emp.Name} Performance ---");
                Console.WriteLine($"Rating: {emp.GetPerformanceRating()}");
                Console.WriteLine($"Annual Bonus: ${emp.CalculateBonus():F2}");
                Console.WriteLine();
            }
        }
    }
}
