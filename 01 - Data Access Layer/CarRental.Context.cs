﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SunshineRentals
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Objects;
    using System.Data.Objects.DataClasses;
    using System.Linq;
    
    public partial class CarRentalEntities : DbContext
    {
        public CarRentalEntities()
            : base("name=CarRentalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Fleet> Fleets { get; set; }
        public DbSet<Manufactor> Manufactors { get; set; }
        public DbSet<Rental> Rentals { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<EventExtention> EventExtentions { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<vwAllEvent> vwAllEvents { get; set; }
    
        public virtual ObjectResult<GetAvailableCars_Result> GetAvailableCars(Nullable<int> isAutomatic, Nullable<int> year, Nullable<int> manufactor, string model, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate, string keywords)
        {
            var isAutomaticParameter = isAutomatic.HasValue ?
                new ObjectParameter("isAutomatic", isAutomatic) :
                new ObjectParameter("isAutomatic", typeof(int));
    
            var yearParameter = year.HasValue ?
                new ObjectParameter("year", year) :
                new ObjectParameter("year", typeof(int));
    
            var manufactorParameter = manufactor.HasValue ?
                new ObjectParameter("manufactor", manufactor) :
                new ObjectParameter("manufactor", typeof(int));
    
            var modelParameter = model != null ?
                new ObjectParameter("model", model) :
                new ObjectParameter("model", typeof(string));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("fromDate", fromDate) :
                new ObjectParameter("fromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("toDate", toDate) :
                new ObjectParameter("toDate", typeof(System.DateTime));
    
            var keywordsParameter = keywords != null ?
                new ObjectParameter("keywords", keywords) :
                new ObjectParameter("keywords", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetAvailableCars_Result>("GetAvailableCars", isAutomaticParameter, yearParameter, manufactorParameter, modelParameter, fromDateParameter, toDateParameter, keywordsParameter);
        }
    
        public virtual ObjectResult<GetCarForRent_Result> GetCarForRent(Nullable<int> id)
        {
            var idParameter = id.HasValue ?
                new ObjectParameter("id", id) :
                new ObjectParameter("id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetCarForRent_Result>("GetCarForRent", idParameter);
        }
    
        public virtual ObjectResult<GetUnlinkedVehicles_Result> GetUnlinkedVehicles()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetUnlinkedVehicles_Result>("GetUnlinkedVehicles");
        }
    
        public virtual ObjectResult<Nullable<int>> InsertOrder(Nullable<int> carId, string user, Nullable<System.DateTime> fromDate, Nullable<System.DateTime> toDate)
        {
            var carIdParameter = carId.HasValue ?
                new ObjectParameter("carId", carId) :
                new ObjectParameter("carId", typeof(int));
    
            var userParameter = user != null ?
                new ObjectParameter("user", user) :
                new ObjectParameter("user", typeof(string));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("fromDate", fromDate) :
                new ObjectParameter("fromDate", typeof(System.DateTime));
    
            var toDateParameter = toDate.HasValue ?
                new ObjectParameter("toDate", toDate) :
                new ObjectParameter("toDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("InsertOrder", carIdParameter, userParameter, fromDateParameter, toDateParameter);
        }
    
        public virtual int GetAvailableSlot(Nullable<int> eventTypeId, Nullable<System.DateTime> fromDate)
        {
            var eventTypeIdParameter = eventTypeId.HasValue ?
                new ObjectParameter("eventTypeId", eventTypeId) :
                new ObjectParameter("eventTypeId", typeof(int));
    
            var fromDateParameter = fromDate.HasValue ?
                new ObjectParameter("fromDate", fromDate) :
                new ObjectParameter("fromDate", typeof(System.DateTime));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("GetAvailableSlot", eventTypeIdParameter, fromDateParameter);
        }
    }
}
