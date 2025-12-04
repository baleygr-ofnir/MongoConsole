using System.IO;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Configuration;
using MongoConsole.Data;
using MongoConsole.Models;
using MongoDB.Driver;

namespace MongoConsole;

class Program
{
    private static IConfigurationRoot _builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
    private static string _connectionString = _builder.GetConnectionString("Atlas");
    private static string[] _menuOptions =
        [
            "1. Add User",
            "2. Get All Users",
            "3. Get User",
            "4. Update User",
            "5. Delete User",
            "6. Delete All Users",
            "0. Exit"
        ];
    
    static async Task Main(string[] args)
    {
        
        await Menu(_menuOptions);
    }

    static async Task Menu(string[] menuOptions)
    {
        bool running = true;
        while (running)
        {
            Console.WriteLine("Choose menu option: ");
            foreach (var menuOption in menuOptions)
            {
                Console.WriteLine(menuOption);
            }

            char pressedKey = Console.ReadKey(true).KeyChar;
            switch (pressedKey)
            {
                case '1':
                    Console.WriteLine(await AddUser());
                    break;
                case '2':
                    await GetUsers();
                    break;
                case '3':
                    Console.WriteLine(await GetUser(GetUserId()));
                    break;
                case '4':
                    Console.WriteLine(await UpdateUser(GetUserId()));
                    break;
                case '5':
                    Console.WriteLine(await DeleteUser(GetUserId()));
                    break;
                case '6':
                    Console.WriteLine(await DeleteUsers());
                    break;
                case '0':
                    running = false;
                    Console.WriteLine("Program ended.");
                    break;
                default:
                    Console.WriteLine("Incorrect choice.");
                    break;
            }

            if (pressedKey != '0')
            {
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }


    static async Task<string> AddUser()
    {
        MongoCRUD service = new(_connectionString, "UserCatalogue");

        User newUser = CreateUserObject(); 
        await service.AddUser("Users", newUser);

        return $"User \"{newUser}\" was added successfully...";
    }

    static async Task GetUsers()
    {
        MongoCRUD service = new(_connectionString, "UserCatalogue");
        var users = await service.GetUsers("Users");
        foreach (var user in users)
        {
            Console.WriteLine(user);
        }
    }

    static async Task<User> GetUser(string userId)
    {
        MongoCRUD service = new(_connectionString, "UserCatalogue");
        User user;
     
        user = await service.GetUser("Users", userId);
        return user;
    }

    static async Task<User> UpdateUser(string userId)
    {
        MongoCRUD service = new(_connectionString, "UserCatalogue");
        User updatedUser = CreateUserObject();
        
        return await service.UpdateUser("Users", userId, updatedUser);
    }
    
    private static async Task<string> DeleteUser(string userId)
    {
        MongoCRUD service = new MongoCRUD(_connectionString, "UserCatalogue");

        return await service.DeleteUser("Users", userId);
    }

    private static async Task<string> DeleteUsers()
    {
        MongoCRUD service = new(_connectionString, "UserCatalogue");
        return await service.DeleteUsers("Users");
    }
    private static string GetUserId()
    {
        Console.Write("Enter user ID: ");
        return Console.ReadLine();
    }
    private static User CreateUserObject()
    {
        Console.Clear();
        string name;
        int age;
        string email;
        User user;
        Console.Write("Enter user name: ");
        name = Console.ReadLine();
        Console.Write("Enter age: ");
        int.TryParse(Console.ReadLine(), out age);
        Console.Write("Enter user email: ");
        email = Console.ReadLine();
        user = new()
        {
            Name = name,
            Age = age,
            Email = email,
            UserAddress = CreateAddressObject()
        };
        return user;
    }

    private static Address CreateAddressObject()
    {
        Console.Clear();
        string street;
        string city;
        string state;
        string zipCode;
        Address newAddress;
        
        Console.Write("Enter street: ");
        street = Console.ReadLine();
        Console.Write("Enter city: ");
        city = Console.ReadLine();
        Console.Write("Enter state: ");
        state = Console.ReadLine();
        Console.Write("Enter zip code: ");
        zipCode = Console.ReadLine();
        newAddress = new()
        {
            Street = street,
            City = city,
            State = state,
            ZipCode = zipCode
        };
        
        return newAddress;
    }
}