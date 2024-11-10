using Microsoft.EntityFrameworkCore;
using Stretch4U.Migrations;

new DesignTimeContextFactory()
    .CreateDbContext(args)
    .Database
    .Migrate();