using System;
using System.Collections.Generic;
using CommandService.Models;
using CommandService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CommandService.Data
{
    public static class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using(var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();

                var platforms = grpcClient.ReturnAllPlatforms();

                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepository>(), platforms);
            }
        }

        private static void SeedData(ICommandRepository repository, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeeding new Platforms...");

            foreach (var plat in platforms)
            {
                if (!repository.ExternalPlatformExits(plat.ExternalID))
                {
                    repository.CreatePlatform(plat);
                }
                repository.SaveChanges();
            }
        }
    }
}