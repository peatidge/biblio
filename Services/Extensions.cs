//TODO:b 145. Install Mapster & Mapster DI Nuget Package and add using Mapster to Extensions.cs
//https://github.com/MapsterMapper/Mapster
//https://github.com/MapsterMapper/Mapster/wiki/Dependency-Injectio
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Tela.Data;
using Tela.Data.Core;
using Tela.Models;
namespace Tela.Services;
//TODO:b 144. Add Class for Extensions to DI Services
public static class Extensions
{
    //TODO:b 146. Add Mapster to DI Services
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        //TODO:b 147. Instantiate Mapster Config
        var config = new TypeAdapterConfig();
        //TODO:b 154. Add Mapping for BookDTO
        TypeAdapterConfig<Data.Organisation.Inventory.Book, BookDTO>
            .NewConfig()
            //TODO:b 155. Add Before Mapping to Check State ensuring it is reevaluated prior to mapping
            .BeforeMapping((src, result) => src.StateCheck())
            .Map(dest => dest.Title, src => src.Reference.Title)
            .Map(dest => dest.ISBN, src => src.Reference.ISBN)
            .Map(dest => dest.TransactionCount, (src) => src.Transactions.Count())
            .Map(dest => dest.LoanCount, src => src.Loans.Count())
            .Map(dest => dest.HoldCount, src => src.Holds.Count())
            .Map(dest => dest.RestorationCount, src => src.Restorations.Count());
        //TODO:b 160. Add Mapping for LibrarianDTO
        TypeAdapterConfig<Data.Organisation.Librarian, LibrarianDTO>
            .NewConfig()
            .Map(dest => dest.HoldCount, src => src.Transactions.OfType<Data.Organisation.Inventory.Hold>().Count())
            .Map(dest => dest.LoanCount, src => src.Transactions.OfType<Data.Organisation.Inventory.Loan>().Count())
            .Map(dest => dest.TransactionCount, src => src.Transactions.Count())
            .Map(dest => dest.RestorationCount, src => src.Restorations.Count());
        //TODO:b 165. Add Mapping for MemberDTO
        TypeAdapterConfig<Data.Organisation.Member, MemberDTO>
            .NewConfig()
            .Map(dest => dest.HoldCount, src => src.Transactions.OfType<Data.Organisation.Inventory.Hold>().Count())
            .Map(dest => dest.LoanCount, src => src.Transactions.OfType<Data.Organisation.Inventory.Loan>().Count())
            .Map(dest => dest.TransactionCount, src => src.Transactions.Count());
        //TODO:b 167. Add Mapping for ReferenceDTO
        TypeAdapterConfig<Data.Organisation.Inventory.Reference, ReferenceDTO>
            .NewConfig()
            .Map(dest => dest.BookCount, src => src.Books.Count());
        //TODO:b 169. Add Mapping for RestorationDTO
        TypeAdapterConfig<Data.Organisation.Inventory.Restoration, RestorationDTO>
            .NewConfig();
        //TODO:b 174. Add Mapping for TransactionDTO
        TypeAdapterConfig<Data.Organisation.Inventory.Transaction, TransactionDTO>
           .NewConfig()
           .Map(dest => dest.ISBN, src => src.Book.Reference.ISBN)
           .Map(dest => dest.Title, src => src.Book.Reference.Title)
           .Map(dest => dest.UID, src => src.Book.UID);
        //TODO:b 148. Add Mapster Config to DI Services as Singleton
        services.AddSingleton(config);
        //TODO:b 149. Add Mapper DI to DI Services (Scoped to HTTP Context, Request, Response)
        services.AddScoped<IMapper,ServiceMapper>();
        return services; 
    }
    
    //TODO:b 187. Include AddLibraryServices to DI Services 
    public static IServiceCollection AddLibraryServices(this IServiceCollection services)
    {
        services.AddScoped<LibraryService>();
        //TODO:b 340. Add ReferenceService to DI
        services.AddScoped<ReferenceService>();
        //TODO:b 383. Add TransactionService to DI
        services.AddScoped<TransactionService>();
        //TODO:b 412b. Add RestorationService to DI
        services.AddScoped<RestorationService>();

        services.AddSingleton<ISeedMe,SeedMe>();

        return services;
    }
    
    
    //TODO:b 420. Add IsAndIsNotFuturistic to Extensions
    /// <summary>
    /// This method checks if the date reference passed in as an argument is not null i.e. "Is". 
    /// If then adds the && "And" logic for checking if the date is not in future the future i.e. "IsNotFuturistic"
    /// Thus "IsAndIsNotFuturistic" (to be and not to be futuristic)
    /// In other words, this method checks if the date is not null and is not in the future
    /// </summary>
    /// <param name="dt">The date time being tested</param>
    /// <returns></returns>
    public static bool IsAndIsNotFuturistic(this DateTime? dt) => dt.HasValue && dt < DateTime.Now;
}
