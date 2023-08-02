public class Shape
{
    public string Name { get; set; }

    public virtual double CalculateArea()
    {
        return 0.0;
    }
}

public class Circle : Shape
{
    public double Radius { get; set; }

    public override double CalculateArea()
    {
        return Math.PI * Radius * Radius;
    }
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public override double CalculateArea()
    {
        return Width * Height;
    }
}

public class Triangle : Shape
{
    public double Base { get; set; }
    public double Height { get; set; }

    public override double CalculateArea()
    {
        return (Base * Height) / 2.0;
    }
}

public class Program
{
    public static void Main()
    {
        Circle circle = new Circle { Name = "Circle", Radius = 10.0 };
        Rectangle rectangle = new Rectangle { Name = "Rectangle", Width = 4.0, Height = 5.0 };
        Triangle triangle = new Triangle { Name = "Triangle", Base = 4.0, Height = 5.0 };
        
        Console.WriteLine($"{"SHAPE NAME",-20}AREA");

        // polymorphism
        PrintShapeArea(circle);
        PrintShapeArea(rectangle);
        PrintShapeArea(triangle);
    }

    private static void PrintShapeArea(Shape shape)
    {
        Console.WriteLine($" {shape.Name, -20}{shape.CalculateArea(), 0:F2}");
    }
}