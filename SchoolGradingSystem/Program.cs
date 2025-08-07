using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SchoolGradingSystem
{
    // a. Student Class
    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public int Score { get; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80 && Score <= 100)
                return "A";
            else if (Score >= 70 && Score <= 79)
                return "B";
            else if (Score >= 60 && Score <= 69)
                return "C";
            else if (Score >= 50 && Score <= 59)
                return "D";
            else
                return "F";
        }

        public override string ToString()
        {
            return $"{FullName} (ID: {Id}): Score = {Score}, Grade = {GetGrade()}";
        }
    }

    // b. InvalidScoreFormatException - Custom Exception
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    // c. MissingFieldException - Custom Exception
    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // d. StudentResultProcessor Class
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            List<Student> students = new List<Student>();

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                string line;
                int lineNumber = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    lineNumber++;
                    
                    // Skip empty lines
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    try
                    {
                        // Split by comma and validate number of fields
                        string[] fields = line.Split(',');
                        
                        if (fields.Length != 3)
                        {
                            throw new MissingFieldException($"Line {lineNumber}: Expected 3 fields (ID, Name, Score) but found {fields.Length}. Line content: '{line}'");
                        }

                        // Trim whitespace from fields
                        for (int i = 0; i < fields.Length; i++)
                        {
                            fields[i] = fields[i].Trim();
                        }

                        // Validate that none of the fields are empty after trimming
                        if (string.IsNullOrEmpty(fields[0]) || string.IsNullOrEmpty(fields[1]) || string.IsNullOrEmpty(fields[2]))
                        {
                            throw new MissingFieldException($"Line {lineNumber}: One or more fields are empty. Line content: '{line}'");
                        }

                        // Try converting ID and score to integers
                        int id, score;
                        
                        if (!int.TryParse(fields[0], out id))
                        {
                            throw new InvalidScoreFormatException($"Line {lineNumber}: Student ID '{fields[0]}' is not a valid integer.");
                        }

                        if (!int.TryParse(fields[2], out score))
                        {
                            throw new InvalidScoreFormatException($"Line {lineNumber}: Score '{fields[2]}' is not a valid integer.");
                        }

                        // Validate score range (optional, but good practice)
                        if (score < 0 || score > 100)
                        {
                            Console.WriteLine($"Warning - Line {lineNumber}: Score {score} is outside typical range (0-100)");
                        }

                        // Create student object
                        Student student = new Student(id, fields[1], score);
                        students.Add(student);
                    }
                    catch (InvalidScoreFormatException)
                    {
                        throw; // Re-throw custom exceptions
                    }
                    catch (MissingFieldException)
                    {
                        throw; // Re-throw custom exceptions
                    }
                    catch (Exception ex)
                    {
                        throw new Exception($"Line {lineNumber}: Unexpected error processing line '{line}': {ex.Message}");
                    }
                }
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                // Write header
                writer.WriteLine("=== STUDENT GRADE REPORT ===");
                writer.WriteLine($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine($"Total Students: {students.Count}");
                writer.WriteLine();

                // Write student details
                writer.WriteLine("STUDENT RESULTS:");
                writer.WriteLine(new string('=', 50));

                foreach (Student student in students)
                {
                    writer.WriteLine(student.ToString());
                }

                // Write summary statistics
                writer.WriteLine();
                writer.WriteLine("GRADE DISTRIBUTION:");
                writer.WriteLine(new string('=', 20));

                var gradeStats = CalculateGradeDistribution(students);
                foreach (var grade in gradeStats)
                {
                    writer.WriteLine($"Grade {grade.Key}: {grade.Value} students");
                }

                // Write average score
                double averageScore = students.Count > 0 ? students.Average(s => s.Score) : 0;
                writer.WriteLine();
                writer.WriteLine($"Average Score: {averageScore:F2}");
            }
        }

        private Dictionary<string, int> CalculateGradeDistribution(List<Student> students)
        {
            var distribution = new Dictionary<string, int>
            {
                {"A", 0}, {"B", 0}, {"C", 0}, {"D", 0}, {"F", 0}
            };

            foreach (Student student in students)
            {
                string grade = student.GetGrade();
                distribution[grade]++;
            }

            return distribution;
        }

        // Helper method to create sample data file for testing
        public void CreateSampleDataFile(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("101, Alice Smith, 84");
                writer.WriteLine("102, Bob Johnson, 72");
                writer.WriteLine("103, Carol Davis, 91");
                writer.WriteLine("104, David Wilson, 58");
                writer.WriteLine("105, Eva Brown, 45");
                writer.WriteLine("106, Frank Miller, 77");
                writer.WriteLine("107, Grace Taylor, 89");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== School Grading System ===");
            Console.WriteLine();

            string inputFile = "students_input.txt";
            string outputFile = "grade_report.txt";

            try
            {
                StudentResultProcessor processor = new StudentResultProcessor();

                // Create sample data file for demonstration
                Console.WriteLine("Creating sample student data file...");
                processor.CreateSampleDataFile(inputFile);
                Console.WriteLine($"Sample data created in: {inputFile}");
                Console.WriteLine();

                // ii. Call ReadStudentsFromFile(...) and pass the input file path
                Console.WriteLine($"Reading students from file: {inputFile}");
                List<Student> students = processor.ReadStudentsFromFile(inputFile);
                Console.WriteLine($"Successfully read {students.Count} students.");
                Console.WriteLine();

                // Display students in console
                Console.WriteLine("Students loaded:");
                foreach (Student student in students)
                {
                    Console.WriteLine($"  • {student}");
                }
                Console.WriteLine();

                // iii. Call WriteReportToFile(...) and pass the student list and output file path
                Console.WriteLine($"Writing report to file: {outputFile}");
                processor.WriteReportToFile(students, outputFile);
                Console.WriteLine("Report generated successfully!");
                Console.WriteLine();

                Console.WriteLine($"Check the following files:");
                Console.WriteLine($"  • Input file: {Path.GetFullPath(inputFile)}");
                Console.WriteLine($"  • Output file: {Path.GetFullPath(outputFile)}");
            }
            // iv. Catch and display specific exceptions
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File Error: {ex.Message}");
                Console.WriteLine("Please ensure the input file exists and try again.");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"Score Format Error: {ex.Message}");
                Console.WriteLine("Please check that all scores are valid integers.");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"Missing Field Error: {ex.Message}");
                Console.WriteLine("Please ensure each line has exactly 3 fields: ID, Name, Score");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
                Console.WriteLine("Please check your input file and try again.");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
