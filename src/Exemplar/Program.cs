using System.ComponentModel.DataAnnotations;
using Serilog;
using Exemplar.HealthChecks;
using Winton.Extensions.Configuration.Consul;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information( "Starting up" );
try
{
    var builder = WebApplication.CreateBuilder( args );


    builder.Services.AddAuthentication( options =>
     {
         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
     } ).AddJwtBearer( options =>
      {
          options.Authority = "https://login.microsoftonline.com/xxxxxxxxxxxxxxxxxxxxxxxxxx";
          options.Audience = "xxxxxxxxxxxxxxxxxxxxxxxxx";
          options.TokenValidationParameters.ValidateLifetime = false;
          options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
      } );

    builder.Services.AddAuthorization();

    builder.Services.AddCors();

    builder.Services.AddScoped<IHelloService, HelloService>();


    //Serilog https://blog.datalust.co/using-serilog-in-net-6/
    builder.Host.UseSerilog( ( ctx, lc ) => lc
         .WriteTo.Console()
         .ReadFrom.Configuration( ctx.Configuration ) );

    // Add services to the container.

    //Health Checks
    builder.Services.AddHealthChecks().AddCheck<GeneralHealthCheck>( "GeneralCheck" );


    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen( c =>
    {
        c.SwaggerDoc( "v1", new()
        {
            Title = builder.Environment.ApplicationName,
            Version = "v1"
        } );
        c.AddSecurityDefinition( "Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        } );
        c.AddSecurityRequirement( new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,

            },
            new List<string>()
        }} );
    } );



    //---------------------------------------------------------------------------------------------------------------------


    var app = builder.Build();
    //Consul
    builder.Configuration
    .AddConsul( $"{app.Environment.ApplicationName}/{app.Environment.EnvironmentName}", options =>
          {
              var consulHost = app.Configuration.GetValue<string>( "ConsulHost" );
              var consulToken = app.Configuration.GetValue<string>( "ConsulToken" );
              options.ReloadOnChange = true;
              options.ConsulConfigurationOptions = cco =>
              {
                  cco.Address = new Uri( consulHost );
                  cco.Token = consulToken;
              };
          } );


    //Logging
    app.UseSerilogRequestLogging();
    if ( app.Environment.IsDevelopment() )
    {
        app.UseDeveloperExceptionPage();


    }

    //Swagger
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseCors( p =>
     {
         p.AllowAnyOrigin();
         p.WithMethods( "GET" );
         p.AllowAnyHeader();
     } );


    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseEndpoints( endpoints =>
                 {
                     endpoints.MapControllers();
                     endpoints.MapCustomHealthChecks( ".NET 6 Exemplar Web API" );
                 } );

    app.UseHttpsRedirection();

    app.MapControllers();

    //Minimal API 
    app.MapGet( "/", ( IConfiguration config ) => $"{ config.GetValue<string>( "message" )} - {config.GetValue<string>( "autor:lastname" )} - {config.GetValue<string>( "languages:0" )}" );
    app.MapGet( "/date", () => Results.Ok( new { Birth = new System.DateOnly( 1984, 7, 21 ).ToLongDateString(), Time = new System.TimeOnly( 15, 0 ).ToLongTimeString()} ) );
    app.MapGet( "/oops", new Func<string>( () => throw new InvalidOperationException( "Oops!" ) ) ).WithName( "Exc" ).WithTags( "Testing" );
    app.MapGet( "/quote", async () => await new HttpClient().GetStringAsync( "https://ron-swanson-quotes.herokuapp.com/v2/quotes" ) );
    app.MapGet( "/person", () => Results.Ok( new Person( "Elon", "Musk" ) ) ).Produces<Person>( 200 ).Produces( 404 );
    app.MapGet( "/Hello", ( bool? isHappy, IHelloService service, ClaimsPrincipal user ) =>
    {
        if ( isHappy is null )
            return Results.BadRequest( "Please tell if you are happy or not :-)" );

        return Results.Ok( service.Hello( user, ( bool )isHappy ) );
    } ).WithTags( "Secured Area" ).RequireAuthorization();


    // Run the app
    app.Run();

}
catch ( Exception ex )
{
    Log.Fatal( ex, "Unhandled exception" );
}
finally
{
    Log.Information( "Shut down complete" );
    Log.CloseAndFlush();
}



public record Person( string FirstName, [Required] string LastName );