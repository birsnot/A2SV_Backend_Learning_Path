using System.Text.Json;

class Student
{
    public string Name { get; set; }
    public int Age { get; set; }
    private readonly int _rollNumber;
    public int RollNumber
    {
        get { return _rollNumber; }
    }
    public string Grade { get; set; }

    public Student(string name, int age, int rollNumber, string grade)
    {
        Name = name;
        Age = age;
        _rollNumber = rollNumber;
        Grade = grade;
    }

    public override string ToString()
    {
        return $"Name: {this.Name}\nAge: {this.Age}\nRoll Number: {this.RollNumber}\nGrade: {this.Grade}\n";
    }
}

class StudentList<T> where T: Student
{
    private readonly List<T> _students;

    public StudentList()
    {
        _students = new List<T>();
    }

    public void Add(T student)
    {
        _students.Add(student);
    }

    public void Remove(T student)
    {
        _students.Remove(student);
    }

    public List<T> GetList()
    {
        return _students;
    }
    
    public void DisplayAll()
    {
        if (_students.Count == 0)
        {
            Console.WriteLine("No student to display.");
            return;
        }
        Console.WriteLine("\t\t\tSTUDENTS");
        foreach (var student in _students)
        {
            Console.WriteLine(student);
        }
    }

    public void SortByAge()
    {
        _students.Sort((x, y) => x.Age.CompareTo(y.Age));
    }

    public void SortByName()
    {
        _students.Sort((x, y) => string.Compare(x.Name, y.Name));
    }

    public IEnumerable<T> Search(string query)
    {
        IEnumerable<T> searchResult;
        if (Int32.TryParse(query, out var numQuery))
        { 
            searchResult =
                from student in _students
                where student.RollNumber.Equals(numQuery)
                select student;
            if(searchResult.Any())
                return searchResult;
        }
        searchResult =
            from student in _students
            where student.Name.Contains(query)
            select student;
        return searchResult;
    }

    public List<T>.Enumerator GetEnumerator()
    {
        return _students.GetEnumerator();
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("\t\t\tSTUDENT MANAGEMENT SYSTEM");
        var students = new StudentList<Student>();
        Deserialize(students);
        bool unsaved = false;

        while (true)
        {
            Console.WriteLine("1. Display all students");
            Console.WriteLine("2. Sort the student list");
            Console.WriteLine("3. Add new student");
            Console.WriteLine("4. Update student's information");
            Console.WriteLine("5. Search for a student");
            Console.WriteLine("6. Delete a student");
            Console.WriteLine("7. Save data");
            Console.WriteLine("8. Exit");
            Console.WriteLine("Enter your selection: ");
    
            var mainSelection = Console.ReadLine();
            switch (mainSelection)
            {
                case "1":
                    students?.DisplayAll();
                    break;
                case "2":
                    unsaved = true;
                    Console.WriteLine("Do you want to sort by name/age?");
                    Console.WriteLine("1. By name");
                    Console.WriteLine("2. By age");
                    Console.WriteLine("Enter your selection: ");
                    var sortSelection = Console.ReadLine();
                    if (sortSelection == "2")
                    {
                        students?.SortByAge();
                        students?.DisplayAll();
                    }
                    else
                    {
                        students?.SortByName();
                        students?.DisplayAll();
                    }

                    break;
                case "3":
                    string? name, grade;
                    int rollNumber, age;
                    while (true)
                    {
                        Console.WriteLine("Enter the name of the student you want to add:");
                        name = Console.ReadLine();
                        if (name is not { Length: 0 }) break;
                        Console.WriteLine("Invalid input!");
                    }

                    while (true)
                    {
                        Console.WriteLine("Enter the age of the student:");
                        var input = Console.ReadLine();
                        if (Int32.TryParse(input, out age)) break;
                        Console.WriteLine("Invalid input!");
                    }

                    while (true)
                    {
                        Console.WriteLine("Enter the student's roll number:");
                        var input = Console.ReadLine();
                        if (Int32.TryParse(input, out rollNumber)) break;
                        Console.WriteLine("Invalid input!");
                    }

                    while (true)
                    {
                        Console.WriteLine("Enter the student's grade:");
                        grade = Console.ReadLine();
                        if (grade is not { Length: 0 }) break;
                        Console.WriteLine("Invalid input!");
                    }

                    students?.Add(new Student(name ?? "Unknown", age, rollNumber, grade ?? "N/A"));
                    Console.WriteLine($"{name} is added to students list.");
                    unsaved = true;
                    break;
                case "4":
                    string? query;
                    while (true)
                    {
                        Console.WriteLine("Enter name/roll number of the student you want to update:");
                        query = Console.ReadLine();
                        if (query is not { Length: 0 }) break;
                        Console.WriteLine("Invalid input!");
                    }

                    var studentsToUpdate = students.Search(query).ToList();
                    if (!studentsToUpdate.Any())
                    {
                        Console.WriteLine("No student with the given name/roll number is found.");
                        break;
                    }

                    Console.WriteLine("Choose the student you want to update:");
                    for (int i = 0; i < studentsToUpdate.Count; ++i)
                    {
                        Console.WriteLine($"{i + 1}.\n{studentsToUpdate[i]}");
                    }

                    int idx;
                    if (Int32.TryParse(Console.ReadLine(), out idx) && idx > 0 && idx <= studentsToUpdate.Count)
                    {
                        idx--;
                        Console.WriteLine($"You are going to update student\n{studentsToUpdate[idx]}");
                        Console.WriteLine("Enter student name to update or press enter to skip:");
                        var newName = Console.ReadLine();
                        if (newName is not { Length: 0 }) studentsToUpdate[idx].Name = newName;
                        Console.WriteLine("Enter student age to update or press enter to skip: ");
                        var input = Console.ReadLine();
                        if (Int32.TryParse(input, out var newAge)) studentsToUpdate[idx].Age = newAge;
                        Console.WriteLine("Enter student grade to update or press enter to skip: ");
                        var newGrade = Console.ReadLine();
                        if (newGrade is not { Length: 0 }) studentsToUpdate[idx].Grade = newGrade;
                        Console.WriteLine($"Student information is updated to: \n{studentsToUpdate[idx]}");
                        unsaved = true;
                    }

                    break;
                case "5":
                    Console.WriteLine("Enter name/roll number of the student you want to find:");
                    var searchQuery = Console.ReadLine();
                    var searchResult = students.Search(searchQuery ?? "");
                    if (!searchResult.Any())
                    {
                        Console.WriteLine("No student with the given name/roll number is found.");
                        break;
                    }

                    Console.WriteLine($"Search result for '{searchQuery}':");
                    foreach (var student in searchResult)
                    {
                        Console.WriteLine(student);
                    }

                    break;
                case "6":
                    string? deleteQuery;
                    while (true)
                    {
                        Console.WriteLine("Enter name/roll number of the student you want to delete:");
                        deleteQuery = Console.ReadLine();
                        if (deleteQuery is not { Length: 0 }) break;
                        Console.WriteLine("Invalid input!");
                    }

                    var studentsToDelete = students.Search(deleteQuery).ToList();
                    if (!studentsToDelete.Any())
                    {
                        Console.WriteLine("No student with the given name/roll number is found.");
                        break;
                    }

                    Console.WriteLine("Choose the student you want to delete:");
                    for (int i = 0; i < studentsToDelete.Count; ++i)
                    {
                        Console.WriteLine($"{i + 1}.\n{studentsToDelete[i]}");
                    }

                    if (Int32.TryParse(Console.ReadLine(), out idx) && idx > 0 && idx <= studentsToDelete.Count)
                    {
                        idx--;
                        Console.WriteLine(
                            $"Are you sure you want to delete student\n{studentsToDelete[idx]}\nEnter 'Y' for yes or 'N' for no:");
                        var chioce = Console.ReadLine();
                        if (chioce?.ToLower() == "y")
                        {
                            students.Remove(studentsToDelete[idx]);
                            Console.WriteLine("Student deleted successfully!");
                            unsaved = true;
                        }

                    }

                    break;
                case "7":
                    Serialize(students);
                    Console.WriteLine("Saved successfully!");
                    unsaved = false;
                    break;
                case "8":
                    if (!unsaved) return;
                    Console.WriteLine("Do you want to save the changes and exit? Enter Y/N:");
                    var choice = Console.ReadLine(); 
                    if(choice?.ToLower() == "n") return;
                    Serialize(students);
                    Console.WriteLine("Saved successfully!");
                    return;
                default:
                    Console.WriteLine("Invalid input!");
                    break;
            }
        }
    }

    static void Serialize(StudentList<Student> students)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        var serializedText = JsonSerializer.Serialize(students.GetList(), options);
        File.WriteAllText("students.json", serializedText);
    }
        
    static void Deserialize(StudentList<Student> students)
    {
        if (File.Exists("students.json"))
        {
            var allText = File.ReadAllText("students.json");
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var data = JsonSerializer.Deserialize<List<Student>>(allText, options);
            foreach (var student in data)
            {
                students.Add(student);
            }
        }
    }
}