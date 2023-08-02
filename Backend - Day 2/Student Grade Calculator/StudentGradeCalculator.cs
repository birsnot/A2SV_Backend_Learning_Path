public class StudentGradeCalculator
{
    public static void Main()
    {
        // title
        Console.WriteLine("\t\t\tSTUDENT GRADE CALCULATOR");
        Console.WriteLine();

        // take the name of the student
        Console.WriteLine("Enter the name of the student: ");
        string studentName = Console.ReadLine();
        while(studentName.Length == 0)
        {
            Console.WriteLine("Invalid input. Enter the name of the student: ");
            studentName = Console.ReadLine();
        }
        
        // take the number of subjects
        Console.WriteLine();
        Console.WriteLine($"Enter the number of subjects {studentName} is taking: ");
        int numOfSubjects;
        while (!int.TryParse(Console.ReadLine(), out numOfSubjects) || numOfSubjects <= 0)
        {
            Console.WriteLine($"Invalid input. Enter the number of subjects {studentName} is taking: ");
        }
        
        // take the name and grade of the subjects
        Dictionary<string, int> subjects = new Dictionary<string, int>();
        for(int i = 1; i <= numOfSubjects; i++)
        {
            // take the name a subject
            Console.WriteLine();
            Console.WriteLine($"Enter the name of subject #{i}: ");
            string subjectName = Console.ReadLine();
            while(subjectName.Length == 0 || subjects.ContainsKey(subjectName))
            {
                Console.WriteLine($"Invalid input. Enter the name of subject #{i}: ");
                subjectName = Console.ReadLine();
            }

            // take the grade of the subject
            Console.WriteLine() ;
            Console.WriteLine($"Enter the grade for {subjectName}: ");
            int grade;
            while (!int.TryParse(Console.ReadLine(), out grade) || grade < 0 || grade > 100)
            {
                Console.WriteLine($"Invalid input. Enter the grade for {subjectName}: ");
            }

            // store the subject name and grade
            subjects[subjectName] = grade;
        }

        // calculate the average
        double averageGrade = average(subjects);

        // print the final result
        Console.WriteLine();
        Console.WriteLine("============================================");
        Console.WriteLine($"Student's Name: {studentName}");
        Console.WriteLine();
        Console.WriteLine($"{"SUBJECT",-20}GRADE");
        foreach(KeyValuePair<string, int> subject in subjects)
        {
            Console.WriteLine($" {subject.Key,-20}{subject.Value}");
        }
        Console.WriteLine();
        Console.WriteLine($"Average Grade: {averageGrade, 0:F2}");
        Console.WriteLine("============================================");
    }

    static double average(Dictionary<string, int> subjects)
    {
        Dictionary<string, int>.ValueCollection grades = subjects.Values;
        return grades.Average();
    }
}