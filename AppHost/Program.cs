var builder = DistributedApplication.CreateBuilder(args);

// Projects.OrderService 
var orderService = builder.AddProject<Projects.OrderService>("OrderService");
var userService = builder.AddProject<Projects.UserService>("UserService");
var productService = builder.AddProject<Projects.ProductService>("ProductService");
var paymentService = builder.AddProject<Projects.PaymentService>("PaymentService");
var gateway = builder.AddProject<Projects.Gateway>("Gateway");

var webapp = builder.AddProject<Projects.Webapp>("Webapp");



builder.Build().Run();
