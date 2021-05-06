namespace FSToDoList

// add calls to use model-view-controller

open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Mvc
open Microsoft.EntityFrameworkCore
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open FSToDoList.DataContext
open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks

type Startup private () =
    new (configuration: IConfiguration) as this =
        Startup() then
        this.Configuration <- configuration

    member this.ConfigureServices(services: IServiceCollection) =
        // services to be added to the framework
        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2) |> ignore
        //services.AddDbContext<ToDoContext>(fun options -> options.UseInMemoryDatabase("DB_ToDo") |> ignore) |> ignore
        services.AddEntityFrameworkNpgsql().AddDbContext<ToDoContext>(fun x -> x.UseNpgsql( this.Configuration.GetConnectionString("DB_ToDo") )|> ignore) |> ignore

    // configure the HTTP request pipeline.
    member this.Configure(app: IApplicationBuilder, env: IHostingEnvironment) =
        if (env.IsDevelopment()) then
            app.UseDeveloperExceptionPage() |> ignore
        else
            app.UseHsts() |> ignore

        app.UseHttpsRedirection() |> ignore
        app.UseMvc() |> ignore

    member val Configuration : IConfiguration = null with get, set
