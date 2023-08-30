using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AirlineWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace AirlineWeb.Data
{
    public class AirlineDbContext:DbContext
    {
        public AirlineDbContext(DbContextOptions<AirlineDbContext> options):base(options)
        {}
        public DbSet<WebhookSubscription> WebhookSubscriptions{get;set;}
        public DbSet<FlightDetail> FlightDetails{get;set;}
    }
}