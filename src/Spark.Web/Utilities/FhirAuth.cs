using Hl7.Fhir.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
namespace Spark.Web.Utilities;

public class FhirAuth
{
    public static string getUnauthorizeJson()
    {
        return @"{
                ""resourceType"": ""OperationOutcome"",
                ""issue"": [
                    {
                    ""severity"": ""error"",
                    ""diagnostics"": ""Unauthorize""
                    }
                ]
                }";
    }
    public static string getUnauthenticateJson()
    {
        return @"{
                ""resourceType"": ""OperationOutcome"",
                ""issue"": [
                    {
                    ""severity"": ""error"",
                    ""diagnostics"": ""Unauthenticate""
                    }
                ]
                }";
    }

    //Note: verifyAccessToken will be modified in the future to integrate with identity server.
    public static string verifyAccessToken(string accessToken)
    {
        if (accessToken != "abc")
        {
            return getUnauthorizeJson();
        }
        return "";
    }
}