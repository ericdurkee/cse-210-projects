using System;
using System.Collections.Generic;
public class Address
{
    private string street;
    private string city;
    private string state;
    private string country;

    public Address(string street, string city, string state, string country)
    {
        this.street = street;
        this.city = city;
        this.state = state;
        this.country = country;
    }
    public bool IsInUSA()
    {
        return country.Equals("USA", StringComparison.OrdinalIgnoreCase);
    }
    public string GetFullAddress()
    {
        return $"{street}\n{city}, {state}\n{country}";
    }
}
public class Customer
{
    private string name;
    private Address address;

    public Customer(string name, Address address)
    {
        this.name = name;
        this.address = address;
    }
    public bool IsInUSA()
    {
        return address.IsInUSA();
    }
    public string GetName()
    {
        return name;
    }
    public string GetAddress()
    {
        return address.GetFullAddress();
    }
}
public class Product
{
    private string name;
    private string productId;
    private decimal price;
    private int quantity;

    public Product(string name, string productId, decimal price, int quantity)
    {
        this.name = name;
        this.productId = productId;
        this.price = price;
        this.quantity = quantity;
    }
    public decimal GetTotalCost()
    {
        return price * quantity;
    }
    public string GetProductDetails()
    {
        return $"{name} (ID: {productId}) - {quantity} @ {price:C} each";
    }
}
public class Order
{
    private List<Product> products;
    private Customer customer;
    private const decimal USA_Shipping_Cost = 5.00m;
    private const decimal International_Shipping_Cost = 35.00m;

    public Order(Customer customer)
    {
        this.customer = customer;
        products = new List<Product>();
    }
    public void AddProduct(Product product)
    {
        products.Add(product);
    }
    public decimal CalculateTotalPrice()
    {
        decimal totalCost = 0;
        foreach (var product in products)
        {
            totalCost += product.GetTotalCost();
        }
        if (customer.IsInUSA())
        {
            totalCost += USA_Shipping_Cost;
        }
        else
        {
            totalCost += International_Shipping_Cost;
        }

        return totalCost;
    }
    public string GetPackingLabel()
    {
        string label = "Packing Label:\n";
        foreach (var product in products)
        {
            label += $"{product.GetProductDetails()}\n";
        }
        return label;
    }
    public string GetShippingLabel()
    {
        return $"Shipping Label:\n{customer.GetName()}\n{customer.GetAddress()}";
    }
}
public class Program
{
    public static void Main(string[] args)
    {
        Address address1 = new Address("777 Main St", "Charlotte", "NC", "USA");
        Address address2 = new Address("888 First St", "Toronto", "ON", "Canada");

        Customer customer1 = new Customer("Molly Pert", address1);
        Customer customer2 = new Customer("Johnston Earl", address2);

        Order order1 = new Order(customer1);
        order1.AddProduct(new Product("Speaker", "W123", 20.00m, 2));
        order1.AddProduct(new Product("Microphone", "G456", 19.99m, 3));

        Order order2 = new Order(customer2);
        order2.AddProduct(new Product("Iphone", "T789", 899.00m, 2));
        order2.AddProduct(new Product("Laptop", "D012", 200.00m, 3));

        Console.WriteLine(order1.GetPackingLabel());
        Console.WriteLine(order1.GetShippingLabel());
        Console.WriteLine($"Total Price: {order1.CalculateTotalPrice():C}\n");

        Console.WriteLine(order2.GetPackingLabel());
        Console.WriteLine(order2.GetShippingLabel());
        Console.WriteLine($"Total Price: {order2.CalculateTotalPrice():C}");
    }
}
