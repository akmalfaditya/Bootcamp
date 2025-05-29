using System;
using System.Collections.Generic;

namespace NestedTypes
{
    /// <summary>
    /// Protected nested types with inheritance demonstration
    /// This shows how nested types participate in inheritance hierarchies
    /// Think of it like family traditions - some things are passed down to children!
    /// </summary>
    public class Employee
    {
        protected string employeeId;
        protected string name;
        protected decimal baseSalary;
        protected DateTime hireDate;
        private string socialSecurityNumber; // This stays private!

        public Employee(string employeeId, string name, decimal baseSalary)
        {
            this.employeeId = employeeId;
            this.name = name;
            this.baseSalary = baseSalary;
            this.hireDate = DateTime.Now;
            this.socialSecurityNumber = "***-**-****"; // Simulated
            
            Console.WriteLine($"Employee {name} hired with ID {employeeId}");
        }

        // Public properties for controlled access
        public string EmployeeId => employeeId;
        public string Name => name;
        public virtual decimal Salary => baseSalary;

        /// <summary>
        /// Protected nested class - available to Employee and its derived classes
        /// This represents sensitive HR operations that only family members should access
        /// </summary>
        protected class HRRecord
        {
            private Employee employee;
            private List<string> performanceNotes;
            private List<decimal> salaryHistory;

            public HRRecord(Employee employee)
            {
                this.employee = employee;
                this.performanceNotes = new List<string>();
                this.salaryHistory = new List<decimal> { employee.baseSalary };
                
                Console.WriteLine($"HR Record created for {employee.name}");
            }

            /// <summary>
            /// This method can access protected members of the outer class
            /// Perfect for HR operations that need employee details
            /// </summary>
            public void AddPerformanceNote(string note)
            {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd");
                string fullNote = $"[{timestamp}] {note} - Employee: {employee.name} (ID: {employee.employeeId})";
                
                performanceNotes.Add(fullNote);
                Console.WriteLine($"Performance note added for {employee.name}");
            }

            public void UpdateSalary(decimal newSalary, string reason)
            {
                decimal oldSalary = employee.baseSalary;
                employee.baseSalary = newSalary; // Direct access to protected field!
                salaryHistory.Add(newSalary);
                
                Console.WriteLine($"Salary updated for {employee.name}: ${oldSalary:C} → ${newSalary:C}");
                Console.WriteLine($"Reason: {reason}");
                
                AddPerformanceNote($"Salary adjustment: {reason}");
            }

            public void ShowHRSummary()
            {
                Console.WriteLine($"\n=== HR Summary for {employee.name} ===");
                Console.WriteLine($"Employee ID: {employee.employeeId}");
                Console.WriteLine($"Hire Date: {employee.hireDate:yyyy-MM-dd}");
                Console.WriteLine($"Current Salary: ${employee.baseSalary:C}");
                Console.WriteLine($"Salary History: {string.Join(" → ", salaryHistory.ConvertAll(s => $"${s:C}"))}");
                Console.WriteLine($"Performance Notes: {performanceNotes.Count}");
                
                foreach (string note in performanceNotes)
                {
                    Console.WriteLine($"  • {note}");
                }
            }
        }

        /// <summary>
        /// Protected method to access HR functionality
        /// Only this class and its derived classes can use this
        /// </summary>
        protected HRRecord GetHRRecord()
        {
            return new HRRecord(this);
        }

        public virtual void ShowEmployeeInfo()
        {
            Console.WriteLine($"Employee: {name} (ID: {employeeId})");
            Console.WriteLine($"Salary: ${Salary:C}");
            Console.WriteLine($"Hire Date: {hireDate:yyyy-MM-dd}");
        }
    }

    /// <summary>
    /// Manager class inherits from Employee and can access protected nested types
    /// This shows how inheritance gives access to family secrets (protected members)
    /// </summary>
    public class Manager : Employee
    {
        private List<Employee> directReports;
        private decimal bonusPercentage;

        public Manager(string employeeId, string name, decimal baseSalary, decimal bonusPercentage) 
            : base(employeeId, name, baseSalary)
        {
            this.directReports = new List<Employee>();
            this.bonusPercentage = bonusPercentage;
            
            Console.WriteLine($"{name} promoted to Manager with {bonusPercentage:P0} bonus eligibility");
        }

        public override decimal Salary => baseSalary * (1 + bonusPercentage);        /// <summary>
        /// Manager-specific nested class that extends HR functionality
        /// This inherits access to protected nested types from the base class
        /// </summary>
        protected class ManagerialHRRecord : HRRecord
        {
            private Manager manager;
            private List<string> teamNotes;

            public ManagerialHRRecord(Manager manager) : base(manager)
            {
                this.manager = manager;
                this.teamNotes = new List<string>();
                
                Console.WriteLine($"Managerial HR Record created for {manager.name}");
            }

            /// <summary>
            /// Manager-specific HR functionality
            /// Can access both Employee's protected members AND Manager's members
            /// </summary>
            public void AddTeamNote(string note)
            {
                string fullNote = $"[{DateTime.Now:yyyy-MM-dd}] TEAM: {note} - Manager: {manager.name}";
                teamNotes.Add(fullNote);
                
                // We can also use inherited functionality
                AddPerformanceNote($"Team leadership: {note}");
            }            public void ProcessTeamSalaryReview()
            {
                Console.WriteLine($"\n=== Team Salary Review by {manager.name} ===");
                
                foreach (Employee employee in manager.directReports)
                {
                    // Since we can't access protected methods on other Employee instances,
                    // we'll demonstrate the manager's own HR functionality instead
                    Console.WriteLine($"Reviewing {employee.Name} (ID: {employee.EmployeeId})");
                    
                    // Simulate performance-based salary adjustment - managers would typically
                    // work through an HR system or have delegation capabilities
                    decimal currentSalary = employee.Salary;
                    decimal adjustment = currentSalary * 0.05m; // 5% raise
                    
                    // In a real system, this would be done through proper HR delegation
                    Console.WriteLine($"  Current Salary: ${currentSalary:C}");
                    Console.WriteLine($"  Recommended Adjustment: +${adjustment:C}");
                    Console.WriteLine($"  New Salary: ${currentSalary + adjustment:C}");
                    
                    AddTeamNote($"Processed salary review for {employee.Name} - recommended ${adjustment:C} increase");
                }
                
                // Show that the manager can access their own HR record
                Console.WriteLine("\nManager's own HR record access:");
                HRRecord managerHR = new HRRecord(manager);
                managerHR.AddPerformanceNote("Completed team salary review process");
                managerHR.ShowHRSummary();
            }

            public void ShowManagerialSummary()
            {
                // Call inherited method
                ShowHRSummary();
                
                Console.WriteLine($"\n=== Managerial Responsibilities ===");
                Console.WriteLine($"Team Size: {manager.directReports.Count}");
                Console.WriteLine($"Bonus Percentage: {manager.bonusPercentage:P0}");
                Console.WriteLine($"Team Notes: {teamNotes.Count}");
                
                foreach (string note in teamNotes)
                {
                    Console.WriteLine($"  • {note}");
                }
            }
        }

        // Manager can use both Employee's protected nested type AND its own
        public void ManageTeam()
        {
            Console.WriteLine($"\n=== {name} Managing Team ===");
            
            // Use inherited protected nested type
            HRRecord myHR = GetHRRecord();
            myHR.AddPerformanceNote("Leading team effectively");
            
            // Use Manager-specific nested type
            ManagerialHRRecord managerialHR = new ManagerialHRRecord(this);
            managerialHR.AddTeamNote("Conducting quarterly team meeting");
            managerialHR.ShowManagerialSummary();
        }

        public void AddDirectReport(Employee employee)
        {
            directReports.Add(employee);
            Console.WriteLine($"{employee.Name} now reports to {name}");
        }

        public void ConductTeamReview()
        {
            ManagerialHRRecord managerialHR = new ManagerialHRRecord(this);
            managerialHR.ProcessTeamSalaryReview();
        }

        public override void ShowEmployeeInfo()
        {
            base.ShowEmployeeInfo();
            Console.WriteLine($"Team Size: {directReports.Count}");
            Console.WriteLine($"Total Compensation: ${Salary:C} (includes {bonusPercentage:P0} bonus)");
        }
    }

    /// <summary>
    /// Executive class - further inheritance demonstrating nested type access
    /// Shows how nested types flow through inheritance hierarchies
    /// </summary>
    public class Executive : Manager
    {
        private decimal stockOptions;
        private string division;

        public Executive(string employeeId, string name, decimal baseSalary, decimal bonusPercentage, 
                        decimal stockOptions, string division) 
            : base(employeeId, name, baseSalary, bonusPercentage)
        {
            this.stockOptions = stockOptions;
            this.division = division;
            
            Console.WriteLine($"{name} appointed as Executive of {division} division");
        }

        public override decimal Salary => base.Salary + stockOptions;        /// <summary>
        /// Executive-specific nested class that builds on inherited functionality
        /// This demonstrates multi-level inheritance with nested types
        /// </summary>
        protected class ExecutiveHRRecord : ManagerialHRRecord
        {
            private Executive executive;
            private List<string> boardNotes;

            public ExecutiveHRRecord(Executive executive) : base(executive)
            {
                this.executive = executive;
                this.boardNotes = new List<string>();
                
                Console.WriteLine($"Executive HR Record created for {executive.name}");
            }

            public void AddBoardNote(string note)
            {
                string fullNote = $"[{DateTime.Now:yyyy-MM-dd}] BOARD: {note} - Executive: {executive.name}";
                boardNotes.Add(fullNote);
                
                // Use inherited methods from multiple levels
                AddTeamNote($"Board-level decision: {note}");
                AddPerformanceNote($"Executive leadership: {note}");
            }

            public void ShowExecutiveSummary()
            {
                // Call inherited method (which calls its inherited method)
                ShowManagerialSummary();
                
                Console.WriteLine($"\n=== Executive Responsibilities ===");
                Console.WriteLine($"Division: {executive.division}");
                Console.WriteLine($"Stock Options: ${executive.stockOptions:C}");
                Console.WriteLine($"Board Notes: {boardNotes.Count}");
                
                foreach (string note in boardNotes)
                {
                    Console.WriteLine($"  • {note}");
                }
            }
        }

        public void HandleExecutiveResponsibilities()
        {
            Console.WriteLine($"\n=== {name} Executive Duties ===");
            
            ExecutiveHRRecord executiveHR = new ExecutiveHRRecord(this);
            executiveHR.AddBoardNote("Strategic planning session completed");
            executiveHR.AddBoardNote("Q4 budget approved");
            executiveHR.ShowExecutiveSummary();
        }

        public override void ShowEmployeeInfo()
        {
            base.ShowEmployeeInfo();
            Console.WriteLine($"Division: {division}");
            Console.WriteLine($"Stock Options: ${stockOptions:C}");
            Console.WriteLine($"Total Compensation: ${Salary:C}");
        }
    }

    /// <summary>
    /// External class to demonstrate what's NOT accessible
    /// This shows the boundaries of protected access
    /// </summary>
    public class ExternalHRSystem
    {
        public void TryToAccessProtectedTypes()
        {
            Console.WriteLine("\n=== External HR System Access Test ===");
            
            Employee emp = new Employee("EMP001", "John Doe", 50000);
            emp.ShowEmployeeInfo();
            
            // We CANNOT access protected nested types from outside the inheritance hierarchy
            // Employee.HRRecord hrRecord = emp.GetHRRecord(); // This would cause compile error!
            
            Console.WriteLine("❌ External systems cannot access protected nested types");
            Console.WriteLine("✓ This maintains encapsulation and security");
        }
    }
}
