using MediatR;
using FluentValidation;

using Azure.Storage.Blobs;
using Microsoft.Extensions.DependencyInjection;
using FileService.Infrastructure.Abstractions;
using FileService.Infrastructure.Services;
using FileService.Application.Handlers;
using Azure.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services
//builder.Services.AddControllers()
//    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(UploadFileCommandHandler).Assembly);
});


// TODO: FileService-specific DI (Storage, Validators, Handlers)
builder.Services.AddScoped<IBlobStorageService, AzureBlobStorageService>();

builder.Services.AddSingleton(x =>
{
    var accountName = "devstoreaccount1";
    var accountKey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
    var blobUri = new Uri("http://azurite:10000/devstoreaccount1");

    var credential = new StorageSharedKeyCredential(accountName, accountKey);
    return new BlobServiceClient(blobUri, credential);
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseRouting();

app.UseAuthorization();
app.MapControllers();

app.Run();
