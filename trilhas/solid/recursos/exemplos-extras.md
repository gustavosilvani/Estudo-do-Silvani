# 💡 Exemplos Extras - Trilha SOLID

Exemplos adicionais e casos de uso avançados dos princípios SOLID.

## 📋 Exemplos Práticos (C#)

### SRP - Exemplo 1: BookingService (Orquestração)

**❌ Violação do SRP:**

```csharp
public class BookingService
{
    public void ProcessBooking(BookingDetails bookingDetails)
    {
        // Responsabilidade 1: Validação das datas
        if (bookingDetails.StartDate >= bookingDetails.EndDate)
        {
            throw new Exception("Data de check-out deve ser após a data de check-in");
        }

        // Responsabilidade 2: Cálculo do preço total
        var durationInDays = Math.Ceiling(
            (bookingDetails.EndDate - bookingDetails.StartDate).TotalDays
        );
        var totalPrice = bookingDetails.DailyRate * durationInDays;
        Console.WriteLine($"Preço total calculado: R${totalPrice}");

        // Responsabilidade 3: Envio de confirmação por e-mail
        Console.WriteLine($"Enviando e-mail de confirmação para {bookingDetails.Email}");
    }
}
```

**✅ Solução Aplicando SRP:**

```csharp
// Responsabilidade única: Validação
public class BookingValidator
{
    public void ValidateDates(DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
        {
            throw new Exception("Data de check-out deve ser após a data de check-in");
        }
    }
}

// Responsabilidade única: Cálculo de preços
public class BookingPriceCalculator
{
    public double CalculatePrice(double dailyRate, DateTime startDate, DateTime endDate)
    {
        var durationInDays = Math.Ceiling((endDate - startDate).TotalDays);
        return dailyRate * durationInDays;
    }
}

// Responsabilidade única: Envio de emails
public class EmailService
{
    public void SendConfirmation(string email)
    {
        Console.WriteLine($"Enviando e-mail de confirmação para {email}");
    }
}

// Orquestração: Coordena as operações
public class BookingService
{
    private readonly BookingValidator _validator;
    private readonly BookingPriceCalculator _priceCalculator;
    private readonly EmailService _emailService;

    public BookingService(
        BookingValidator validator,
        BookingPriceCalculator priceCalculator,
        EmailService emailService)
    {
        _validator = validator;
        _priceCalculator = priceCalculator;
        _emailService = emailService;
    }

    public void ProcessBooking(BookingDetails bookingDetails)
    {
        _validator.ValidateDates(bookingDetails.StartDate, bookingDetails.EndDate);
        
        var totalPrice = _priceCalculator.CalculatePrice(
            bookingDetails.DailyRate,
            bookingDetails.StartDate,
            bookingDetails.EndDate
        );
        
        Console.WriteLine($"Preço total calculado: R${totalPrice}");
        _emailService.SendConfirmation(bookingDetails.Email);
    }
}
```

### SRP - Exemplo 2: CheckoutService (Orquestração)

**❌ Violação do SRP:**

```csharp
public class CheckoutService
{
    public void ProcessCheckout(Cart cart, string userId)
    {
        // Responsabilidade 1: Validação de estoque
        foreach (var item in cart.Items)
        {
            if (item.Stock < item.Quantity)
            {
                throw new Exception($"Produto {item.Name} sem estoque suficiente.");
            }
        }

        // Responsabilidade 2: Cálculo de impostos e total
        double total = 0;
        foreach (var item in cart.Items)
        {
            total += item.Price * item.Quantity;
        }
        var tax = total * 0.1;
        total += tax;
        Console.WriteLine($"Total com impostos: R${total}");

        // Responsabilidade 3: Processamento de pagamento
        Console.WriteLine($"Processando pagamento para o usuário {userId}");
    }
}
```

**✅ Solução Aplicando SRP:**

```csharp
// Responsabilidade única: Validação de estoque
public class StockValidator
{
    public void Validate(Cart cart)
    {
        foreach (var item in cart.Items)
        {
            if (item.Stock < item.Quantity)
            {
                throw new Exception($"Produto {item.Name} sem estoque suficiente.");
            }
        }
    }
}

// Responsabilidade única: Cálculo de impostos
public class TaxCalculator
{
    public double Calculate(Cart cart)
    {
        double total = 0;
        foreach (var item in cart.Items)
        {
            total += item.Price * item.Quantity;
        }
        var tax = total * 0.1;
        return total + tax;
    }
}

// Responsabilidade única: Processamento de pagamento
public class PaymentProcessor
{
    public void ProcessPayment(string userId, double amount)
    {
        Console.WriteLine($"Processando pagamento de R${amount} para o usuário {userId}");
    }
}

// Orquestração: Coordena as operações
public class CheckoutService
{
    private readonly StockValidator _stockValidator;
    private readonly TaxCalculator _taxCalculator;
    private readonly PaymentProcessor _paymentProcessor;

    public CheckoutService(
        StockValidator stockValidator,
        TaxCalculator taxCalculator,
        PaymentProcessor paymentProcessor)
    {
        _stockValidator = stockValidator;
        _taxCalculator = taxCalculator;
        _paymentProcessor = paymentProcessor;
    }

    public void ProcessCheckout(Cart cart, string userId)
    {
        _stockValidator.Validate(cart);
        var totalWithTaxes = _taxCalculator.Calculate(cart);
        _paymentProcessor.ProcessPayment(userId, totalWithTaxes);
    }
}
```

### SRP - Exemplo 3: FileUploadService (Orquestração)

**❌ Violação do SRP:**

```csharp
public class FileUploadService
{
    public void UploadFile(byte[] file, string destination)
    {
        // Responsabilidade 1: Compressão do arquivo
        var compressedFile = CompressFile(file);

        // Responsabilidade 2: Envio do arquivo
        Console.WriteLine($"Enviando arquivo para {destination}");
    }

    private byte[] CompressFile(byte[] file)
    {
        Console.WriteLine("Comprimindo arquivo...");
        // Exemplo de compressão simplificada
        return file.Take(file.Length / 2).ToArray();
    }
}
```

**✅ Solução Aplicando SRP:**

```csharp
// Responsabilidade única: Compressão
public class FileCompressor
{
    public byte[] Compress(byte[] file)
    {
        Console.WriteLine("Comprimindo arquivo...");
        // Lógica de compressão
        return file.Take(file.Length / 2).ToArray();
    }
}

// Responsabilidade única: Upload para nuvem
public class CloudUploader
{
    public void Upload(byte[] file, string destination)
    {
        Console.WriteLine($"Enviando arquivo para {destination}");
    }
}

// Orquestração: Coordena compressão e upload
public class FileUploadService
{
    private readonly FileCompressor _compressor;
    private readonly CloudUploader _uploader;

    public FileUploadService(FileCompressor compressor, CloudUploader uploader)
    {
        _compressor = compressor;
        _uploader = uploader;
    }

    public void UploadFile(byte[] file, string destination)
    {
        var compressedFile = _compressor.Compress(file);
        _uploader.Upload(compressedFile, destination);
    }
}
```

### OCP - Exemplo 1: ReportProcessor

**❌ Violação do OCP:**

```csharp
public class ReportProcessor
{
    public void Process(string reportType)
    {
        if (reportType == "PDF")
        {
            Console.WriteLine("Processing PDF report...");
        }
        else if (reportType == "CSV")
        {
            Console.WriteLine("Processing CSV report...");
        }
        else
        {
            Console.WriteLine("Unknown report type!");
        }
    }
}
```

**Problema**: Para adicionar um novo tipo de relatório (ex: Excel), precisamos MODIFICAR a classe.

**✅ Solução Aplicando OCP:**

```csharp
// Interface para extensão
public interface IReport
{
    void Process();
}

// Implementações específicas (extensões)
public class PDFReport : IReport
{
    public void Process()
    {
        Console.WriteLine("Processing PDF report...");
    }
}

public class CSVReport : IReport
{
    public void Process()
    {
        Console.WriteLine("Processing CSV report...");
    }
}

// Classe fechada para modificação, aberta para extensão
public class ReportProcessor
{
    public void Process(IReport report)
    {
        report.Process();
    }
}

// Uso: Novos tipos podem ser adicionados sem modificar ReportProcessor
public class ExcelReport : IReport
{
    public void Process()
    {
        Console.WriteLine("Processing Excel report...");
    }
}
```

### LSP - Exemplo 1: Rectangle e Square

**❌ Violação do LSP:**

```csharp
public class Rectangle
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public virtual void SetDimensions(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public double GetArea()
    {
        return Width * Height;
    }
}

public class Square : Rectangle
{
    public Square(double size) : base(size, size) { }

    // Violação do LSP: Fortalece pré-condições
    public override void SetDimensions(double width, double height)
    {
        if (width != height)
        {
            throw new Exception("Para um quadrado, largura e altura devem ser iguais!");
        }
        base.SetDimensions(width, height);
    }
}

// Função que espera um Rectangle
public void ResizeRectangle(Rectangle rectangle)
{
    rectangle.SetDimensions(4, 5); // Funciona para Rectangle, mas quebra para Square!
    Console.WriteLine($"Área ajustada: {rectangle.GetArea()}");
}
```

**Problema**: Square não pode substituir Rectangle sem quebrar o comportamento esperado.

**✅ Solução Aplicando LSP:**

```csharp
// Classe base abstrata
public abstract class Shape
{
    public abstract double GetArea();
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public void SetDimensions(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public override double GetArea()
    {
        return Width * Height;
    }
}

public class Square : Shape
{
    public double Side { get; set; }

    public Square(double side)
    {
        Side = side;
    }

    public void SetSide(double side)
    {
        Side = side;
    }

    public override double GetArea()
    {
        return Side * Side;
    }
}

// Agora ambas podem ser usadas onde Shape é esperado
public double CalculateTotalArea(List<Shape> shapes)
{
    return shapes.Sum(s => s.GetArea());
}
```

### ISP-DIP - Exemplo: Dependency Injection com Interfaces

**❌ Violação do DIP:**

```csharp
// Alto nível depende de baixo nível
public class UserService
{
    private MySQLDatabase _database; // Dependência concreta!

    public UserService()
    {
        _database = new MySQLDatabase(); // Acoplamento forte
    }

    public void CreateUser(string name)
    {
        _database.Save(name);
    }
}
```

**✅ Solução Aplicando DIP e ISP:**

```csharp
// Abstração (interface)
public interface IDatabase
{
    void Save(string data);
}

// Implementação de baixo nível
public class MySQLDatabase : IDatabase
{
    public void Save(string data)
    {
        Console.WriteLine($"Salvando {data} no MySQL");
    }
}

public class PostgreSQLDatabase : IDatabase
{
    public void Save(string data)
    {
        Console.WriteLine($"Salvando {data} no PostgreSQL");
    }
}

// Interface segregada para UserService
public interface IUserService
{
    void CreateUser(string name);
}

// Alto nível depende de abstração
public class UserService : IUserService
{
    private readonly IDatabase _database; // Depende de abstração

    public UserService(IDatabase database) // Injeção de dependência
    {
        _database = database;
    }

    public void CreateUser(string name)
    {
        _database.Save(name);
    }
}

// Outro serviço que depende apenas do que precisa (ISP)
public class CheckoutService
{
    private readonly IUserService _userService; // Depende apenas de IUserService

    public CheckoutService(IUserService userService)
    {
        _userService = userService;
    }

    public void Checkout()
    {
        _userService.CreateUser("João");
        Console.WriteLine("Checkout realizado com sucesso");
    }
}

// Uso com Dependency Injection
var mysqlDb = new MySQLDatabase();
var userService = new UserService(mysqlDb);
var checkoutService = new CheckoutService(userService);
checkoutService.Checkout();
```

## 🎯 Casos de Uso Reais

### Sistema de E-commerce

Exemplo completo de aplicação de SOLID em um sistema de e-commerce.

### Sistema de Autenticação

Como aplicar SOLID em um sistema de autenticação e autorização.

### API REST

Design de API REST seguindo princípios SOLID.

## 🔄 Padrões de Design Relacionados

### Strategy Pattern e OCP

Como o Strategy Pattern implementa o Open/Closed Principle.

### Factory Pattern e DIP

Como o Factory Pattern facilita a Dependency Inversion.

### Adapter Pattern e ISP

Como o Adapter Pattern resolve problemas de Interface Segregation.

## 🌍 Exemplos em Diferentes Linguagens

### Java

Exemplos de SOLID em Java.

### C#

Exemplos de SOLID em C#.

### Python

Exemplos de SOLID em Python.

---

[← Voltar ao índice da trilha](../README.md)
