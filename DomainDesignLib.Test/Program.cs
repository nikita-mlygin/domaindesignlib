using DomainDesignLib.Abstractions.Result;

Result<Foo1> parameter1 = null!;
Result<Foo2> parameter2 = null!;

Console.WriteLine(
    await Result
        .Check(() => parameter1 = Foo1.Create(Guid.NewGuid()))
        .Check(() => parameter2 = Foo2.Create(parameter1.Value))
        .Success(CreateIntAsync)
        .After(AfterResult1)
        .Match(_ => "Complete", (error) => error.Message)
);

Result<Macronutrients> macronutrients = null!;
Result<Product> product = null!;

Task<Result<Guid>> res = Result
    .Check(() => macronutrients = Macronutrients.Create(12, 12, 12, 11))
    .Check(
        () =>
            product = Product.Create(
                Guid.NewGuid(),
                "12",
                macronutrients.Value,
                Guid.NewGuid(),
                DateTime.UtcNow
            )
    )
    .Success(() => OnSuccess(product)());

Console.WriteLine((await res).Value);

Console.WriteLine(await GetAboba().Match(s => s, fail => -1));

static async Task CreateResult()
{
    Console.WriteLine("Create");
    await Task.Delay(2000);
    Console.WriteLine("Create");
}

static async Task AfterResult1()
{
    Console.WriteLine("After1");
    await Task.Delay(1000);
    Console.WriteLine("After1");
}

static void CreateVoidResult()
{
    Console.WriteLine("Create");
}

static async Task<int> CreateIntAsync()
{
    Console.WriteLine("Create 288");

    return 228;
}

static Task<Result<int>> GetAboba()
{
    return Result
        .Check(0, (val1) => val1 > 10, new Error("2", "First"))
        .Check(10, (val2) => val2 > 10, new Error("2", "Second"))
        .Check("0", (val2) => val2 == "20", new Error("2", "Third"))
        .Success(CreateIntAsync)
        .After(async () => Console.WriteLine(888));
}

Func<Task<Guid>> OnSuccess(Result<Product> product)
{
    return async () =>
    {
        Console.WriteLine(product.Value.Name);

        return Guid.NewGuid();
    };
}

public class Foo1
{
    private Foo1(Guid id) { }

    public static Result<Foo1> Create(Guid id) => Result.Success(new Foo1(id));

    public Guid Id { get; private set; }
}

public class Foo2
{
    public Foo1 Id { get; private set; }

    private Foo2(Foo1 foo1) => Id = foo1;

    public static Result<Foo2> Create(Foo1 id) => Result.Success(new Foo2(id));
}

public class Product
{
    private Product(
        Guid id,
        string name,
        Macronutrients macronutrients,
        Guid owner,
        DateTime createdAt,
        DateTime? updatedAt
    )
    {
        Name = name;
        Macronutrients = macronutrients;
        Owner = owner;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public static Result<Product> Create(
        Guid id,
        string name,
        Macronutrients macronutrients,
        Guid creator,
        DateTime createdAt
    )
    {
        return Result.Success(new Product(id, name, macronutrients, creator, createdAt, null));
    }

    public string Name { get; private set; }

    public Macronutrients Macronutrients { get; private set; }

    public Guid Owner { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }
}

public record Macronutrients
{
    private Macronutrients(decimal calories, decimal proteins, decimal fats, decimal carbohydrates)
    {
        Calories = calories;
        Proteins = proteins;
        Fats = fats;
        Carbohydrates = carbohydrates;
    }

    public static Result<Macronutrients> Create(
        decimal calories,
        decimal proteins,
        decimal fats,
        decimal carbohydrates
    )
    {
        return Result.Success(new Macronutrients(calories, proteins, fats, carbohydrates));
    }

    private static bool CheckValueByNotLessZero(decimal value) => value < 0;

    public decimal Calories { get; init; }
    public decimal Proteins { get; init; }
    public decimal Fats { get; init; }
    public decimal Carbohydrates { get; init; }

    public static Macronutrients operator +(Macronutrients left, Macronutrients right)
    {
        return new(
            left.Calories + right.Calories,
            left.Proteins + right.Proteins,
            left.Fats + right.Fats,
            left.Carbohydrates + right.Carbohydrates
        );
    }

    public static Macronutrients operator -(Macronutrients left, Macronutrients right)
    {
        return new(
            left.Calories - right.Calories,
            left.Proteins - right.Proteins,
            left.Fats - right.Fats,
            left.Carbohydrates - right.Carbohydrates
        );
    }

    public static Macronutrients operator -(Macronutrients left, decimal right)
    {
        return new(
            left.Calories - right,
            left.Proteins - right,
            left.Fats - right,
            left.Carbohydrates - right
        );
    }

    public static Macronutrients operator +(Macronutrients left, decimal right)
    {
        return new(
            left.Calories + right,
            left.Proteins + right,
            left.Fats + right,
            left.Carbohydrates + right
        );
    }

    public static Macronutrients operator *(Macronutrients left, decimal right)
    {
        return new(
            left.Calories * right,
            left.Proteins * right,
            left.Fats * right,
            left.Carbohydrates * right
        );
    }

    public static Macronutrients operator /(Macronutrients left, decimal right)
    {
        return new(
            left.Calories / right,
            left.Proteins / right,
            left.Fats / right,
            left.Carbohydrates / right
        );
    }
}
