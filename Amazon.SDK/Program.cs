// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using AWS.SDK.SystemManager;

var paramStoreUtil = new ParameterStore();
await paramStoreUtil.CopyToAnotherAccountAsync(new AWS.SDK.Models.AwsProfileKey(), new AWS.SDK.Models.AwsProfileKey(), "/Test");
