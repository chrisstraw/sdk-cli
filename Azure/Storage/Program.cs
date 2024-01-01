// See https://aka.ms/new-console-template for more information
using Azure.SDK.Blob;

//Console.WriteLine("Hello, World!");

var helper = new BlobUtilities();
var connStr = "invalid"; //pull from config or CLI input
await helper.DownloadAllContainers(connStr, @"C:\blob-backup");


