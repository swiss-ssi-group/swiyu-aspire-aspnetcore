var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.EmployeeOnboarding>("employeeonboarding");

builder.Build().Run();
