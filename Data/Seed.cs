using CsvHelper;
using Microsoft.EntityFrameworkCore;
using RenzGrandWeddingblazor.ph.Data.Entities;
using System.Reflection;
using System.Text;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();

        // Seed Product Lines
        string resourceName;

        resourceName = "RenzGrandWeddingblazor.ph.Data.SeedData.ProductLine.csv";
        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252)))
            {
                CsvReader csvReader = new CsvReader(reader);
                csvReader.Configuration.HeaderValidated = null;
                csvReader.Configuration.MissingFieldFound = null;

                ProductLine[] productLines = csvReader.GetRecords<ProductLine>().ToArray();

                foreach (ProductLine productLine in productLines)
                {
                    modelBuilder.Entity<ProductLine>().HasData(
                        new ProductLine
                        {
                            ProductLineId = productLine.ProductLineId,
                            ProductLineCode = productLine.ProductLineCode,
                            ProductLineName = productLine.ProductLineName
                        });

                }
            }
        }
        
        resourceName = "RenzGrandWeddingblazor.ph.Data.SeedData.Sales.csv";
        using (Stream stream = assembly.GetManifestResourceStream(resourceName))
        {
            using (StreamReader reader = new StreamReader(stream, Encoding.GetEncoding(1252)))
            {
                CsvReader csvReader = new CsvReader(reader);
                csvReader.Configuration.HeaderValidated = null;
                csvReader.Configuration.MissingFieldFound = null;

                Sales[] productLines = csvReader.GetRecords<Sales>().ToArray();

                foreach (Sales productLine in productLines)
                {
                    modelBuilder.Entity<Sales>().HasData(
                        new Sales
                        {
                            SalesId = productLine.SalesId,
                            SalesName = productLine.SalesName,
                            SalesDescription = productLine.SalesDescription
                        });

                }
            }
        }
    }
}
