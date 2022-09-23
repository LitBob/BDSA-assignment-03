using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
var connectionString = configuration.GetConnectionString("ConnectionString");