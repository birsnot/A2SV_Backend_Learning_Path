class Program
{
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        TaskManager taskManager = new TaskManager();
        await taskManager.LoadTasksAsync();

        Console.WriteLine("\n\t\t\tSIMPLE TASK MANAGER");

        while (true)
        {
            Console.WriteLine("\n'v' to view all tasks");
            Console.WriteLine("'c' to view tasks by category");
            Console.WriteLine("'a' to add a task");
            Console.WriteLine("'q' to exit");
            Console.Write("Enter your choice: ");
            var choice = Console.ReadLine().ToLower();

            switch (choice)
            {
                case "v":
                    taskManager.PrintAllTasks();
                    break;
                
                case "c":
                    Console.WriteLine("Select task category to view:");
                    Console.WriteLine("'p' for Personal");
                    Console.WriteLine("'w' for Work");
                    Console.WriteLine("'e' for Errands");
                    Console.WriteLine("'u' for Unspecified");
                    Console.Write("Enter your choice: ");
                    var filterChoice = Console.ReadLine().ToLower();
                    TaskCategory filterCategory;
                    switch (filterChoice)
                    {
                        case "p":
                            filterCategory = TaskCategory.Personal;
                            break;
                        case "w":
                            filterCategory = TaskCategory.Work;
                            break;
                        case "e":
                            filterCategory = TaskCategory.Errands;
                            break;
                        default:
                            filterCategory = TaskCategory.Unspecified;
                            break;
                    }
                    taskManager.FilterTasks(filterCategory);
                    break;
                
                case "a":
                    Console.Write("Enter task name: ");
                    var name = Console.ReadLine();
                    Console.Write("Enter task description: ");
                    var description = Console.ReadLine();
                    Console.WriteLine("Select task category:");
                    Console.WriteLine("'p' for Personal");
                    Console.WriteLine("'w' for Work");
                    Console.WriteLine("'e' for Errands");
                    Console.WriteLine("'u' for Unspecified");
                    Console.Write("Enter your choice: ");
                    var categoryChoice = Console.ReadLine().ToLower();
                    TaskCategory category;
                    switch (categoryChoice)
                    {
                        case "p":
                            category = TaskCategory.Personal;
                            break;
                        case "w":
                            category = TaskCategory.Work;
                            break;
                        case "e":
                            category = TaskCategory.Errands;
                            break;
                        default:
                            category = TaskCategory.Unspecified;
                            break;
                    }
                    taskManager.AddTask(name, description, category);
                    Console.WriteLine("Task added successfully.");
                    break;

                case "q":
                    await taskManager.SaveTasksAsync();
                    Console.WriteLine("Tasks saved. Exiting...");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}

public enum TaskCategory
{
    Personal,
    Work,
    Errands,
    Unspecified
}

public class Task
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public TaskCategory Category { get; set; }
    public bool IsCompleted { get; set; }

    public override string ToString()
    {
        return $"\nTask Name: {this.Name}\nDescription: {this.Description}\nCategory: {this.Category}\nCompleted: {this.IsCompleted}";
    }
}

public class TaskManager
{
    private List<Task> _tasks;
    private string _filePath = "./tasks.csv";

    public TaskManager()
    {
        _tasks = new List<Task>();
    }

    public void AddTask(string name, string description, TaskCategory category = TaskCategory.Unspecified)
    {
        _tasks.Add(new Task { Name = name, Description = description, Category = category, IsCompleted = false });
    }

    public void PrintAllTasks()
    {
        PrintTasks(_tasks);
    }
    public void FilterTasks(TaskCategory category)
    {
        PrintTasks(_tasks.Where(task => task.Category == category));
    }

    private void PrintTasks(IEnumerable<Task> tasks)
    {
        Console.WriteLine("\t\t\tTASKS");
        foreach (var task in tasks)
        {
            Console.WriteLine(task);
        }
    }

    public async System.Threading.Tasks.Task SaveTasksAsync()
    {
        try
        {
            using (var fileWriter = new StreamWriter(_filePath))
            {
                foreach (var task in _tasks)
                {
                    await fileWriter.WriteLineAsync($"{task.Name},{task.Description},{task.Category},{task.IsCompleted}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while saving tasks: {ex.Message}");
        }
    }

    public async System.Threading.Tasks.Task LoadTasksAsync()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                using (var fileReader = new StreamReader(_filePath))
                {
                    string line;
                    while ((line = await fileReader.ReadLineAsync()) != null)
                    {
                        string[] taskData = line.Split(',');
                        string name = taskData[0];
                        string description = taskData[1];
                        TaskCategory category = (TaskCategory)Enum.Parse(typeof(TaskCategory), taskData[2]);
                        bool isCompleted = bool.Parse(taskData[3]);
                        _tasks.Add(new Task { Name = name, Description = description, Category = category, IsCompleted = isCompleted });
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading tasks: {ex.Message}");
        }
    }
}