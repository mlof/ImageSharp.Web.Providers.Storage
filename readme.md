# ImageSharp.Web.Providers.Storage

## Getting started


```csharp
// register Storage.net provider in your IOC container 
services.AddSingleton(provider => StorageFactory.Blobs.DirectoryFiles(directoryFullName: ""));

// remove the default ImageProvider, and add the StorageNetImageProvider.
services.AddImageSharp(options => { })
                .RemoveProvider<PhysicalFileSystemProvider>()
                .AddProvider<StorageNetImageProvider>();
```