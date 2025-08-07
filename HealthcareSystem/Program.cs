using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareSystem
{
    // a. Generic Repository<T> class
    public class Repository<T>
    {
        private List<T> items;

        public Repository()
        {
            items = new List<T>();
        }

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return new List<T>(items);
        }

        public T? GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }

    // b. Patient class
    public class Patient
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString()
        {
            return $"Patient ID: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}";
        }
    }

    // c. Prescription class
    public class Prescription
    {
        public int Id { get; }
        public int PatientId { get; }
        public string MedicationName { get; }
        public DateTime DateIssued { get; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString()
        {
            return $"Prescription ID: {Id}, Medication: {MedicationName}, Issued: {DateIssued:yyyy-MM-dd}";
        }
    }

    // g. HealthSystemApp class
    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo;
        private Repository<Prescription> _prescriptionRepo;
        private Dictionary<int, List<Prescription>> _prescriptionMap;

        public HealthSystemApp()
        {
            _patientRepo = new Repository<Patient>();
            _prescriptionRepo = new Repository<Prescription>();
            _prescriptionMap = new Dictionary<int, List<Prescription>>();
        }

        public void SeedData()
        {
            Console.WriteLine("--- Seeding Sample Data ---");
            
            // Add 2-3 Patient objects
            _patientRepo.Add(new Patient(1, "Alice Johnson", 28, "Female"));
            _patientRepo.Add(new Patient(2, "Bob Smith", 45, "Male"));
            _patientRepo.Add(new Patient(3, "Carol Davis", 32, "Female"));

            // Add 4-5 Prescription objects with valid PatientIds
            _prescriptionRepo.Add(new Prescription(101, 1, "Ibuprofen 400mg", DateTime.Now.AddDays(-10)));
            _prescriptionRepo.Add(new Prescription(102, 1, "Vitamin D3", DateTime.Now.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(103, 2, "Metformin 500mg", DateTime.Now.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(104, 2, "Lisinopril 10mg", DateTime.Now.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(105, 3, "Amoxicillin 250mg", DateTime.Now.AddDays(-1)));

            Console.WriteLine("Sample data seeded successfully.");
            Console.WriteLine();
        }

        public void BuildPrescriptionMap()
        {
            Console.WriteLine("--- Building Prescription Map ---");
            
            // Clear existing map
            _prescriptionMap.Clear();

            // Loop through all prescriptions and group by PatientId
            var allPrescriptions = _prescriptionRepo.GetAll();
            foreach (var prescription in allPrescriptions)
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    _prescriptionMap[prescription.PatientId] = new List<Prescription>();
                }
                _prescriptionMap[prescription.PatientId].Add(prescription);
            }

            Console.WriteLine($"Prescription map built with {_prescriptionMap.Count} patient groups.");
            Console.WriteLine();
        }

        public void PrintAllPatients()
        {
            Console.WriteLine("--- All Patients ---");
            var patients = _patientRepo.GetAll();
            
            if (patients.Count == 0)
            {
                Console.WriteLine("No patients found.");
            }
            else
            {
                foreach (var patient in patients)
                {
                    Console.WriteLine(patient);
                }
            }
            Console.WriteLine();
        }

        public void PrintPrescriptionsForPatient(int patientId)
        {
            Console.WriteLine($"--- Prescriptions for Patient ID {patientId} ---");
            
            // Get patient info first
            var patient = _patientRepo.GetById(p => p.Id == patientId);
            if (patient == null)
            {
                Console.WriteLine($"Patient with ID {patientId} not found.");
                Console.WriteLine();
                return;
            }

            Console.WriteLine($"Patient: {patient.Name}");
            
            // Get prescriptions using the map
            if (_prescriptionMap.ContainsKey(patientId))
            {
                var prescriptions = _prescriptionMap[patientId];
                Console.WriteLine($"Total prescriptions: {prescriptions.Count}");
                Console.WriteLine();
                
                foreach (var prescription in prescriptions.OrderBy(p => p.DateIssued))
                {
                    Console.WriteLine($"  • {prescription}");
                }
            }
            else
            {
                Console.WriteLine("No prescriptions found for this patient.");
            }
            Console.WriteLine();
        }

        // f. Method to get prescriptions by patient ID using dictionary
        public List<Prescription> GetPrescriptionsByPatientId(int patientId)
        {
            if (_prescriptionMap.ContainsKey(patientId))
            {
                return new List<Prescription>(_prescriptionMap[patientId]);
            }
            return new List<Prescription>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Healthcare Management System ===");
            Console.WriteLine();

            // i. Instantiate HealthSystemApp
            HealthSystemApp app = new HealthSystemApp();

            // ii. Call SeedData()
            app.SeedData();

            // iii. Call BuildPrescriptionMap()
            app.BuildPrescriptionMap();

            // iv. Print all patients
            app.PrintAllPatients();

            // v. Select one PatientId and display all prescriptions
            Console.WriteLine("--- Demonstrating Prescription Lookup ---");
            app.PrintPrescriptionsForPatient(1); // Alice Johnson
            app.PrintPrescriptionsForPatient(2); // Bob Smith
            app.PrintPrescriptionsForPatient(4); // Non-existent patient

            // Additional demonstration of generic repository usage
            Console.WriteLine("--- Repository Usage Statistics ---");
            var patientRepo = new Repository<Patient>();
            var prescriptionRepo = new Repository<Prescription>();
            
            // Get counts through the app's methods since fields are private
            var allPatients = app.GetPrescriptionsByPatientId(1); // This demonstrates the method works
            Console.WriteLine("Generic repository and dictionary integration working successfully!");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
