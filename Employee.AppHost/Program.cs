using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var externalVerifierUrl = builder.AddParameter("externalverifierurl");
var openIdClientMetaDataFile = builder.AddParameter("openidclientmetadatafile");
var verifierDid = builder.AddParameter("verifierdid");
var didVerifierMethod = builder.AddParameter("didverifiermethod");
var verifierName = builder.AddParameter("verifiername");
var signingKey = builder.AddParameter("signingkey");
var postGresUser = builder.AddParameter("postgresuser");
var postGresPassword = builder.AddParameter("postgrespassword", secret: true);
var postGresDb = builder.AddParameter("postgresdb");
var postGresJdbc = builder.AddParameter("postgresjdbc");

var swiyuOid4vp = builder.AddContainer("swiyu-oid4vp", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-oid4vp", "latest")
    .WithEnvironment("EXTERNAL_URL", externalVerifierUrl)
    .WithEnvironment("OPENID_CLIENT_METADATA_FILE", openIdClientMetaDataFile)
    .WithEnvironment("VERIFIER_DID", verifierDid)
    .WithEnvironment("DID_VERIFICATION_METHOD", didVerifierMethod)
    .WithEnvironment("VERIFIER_NAME", verifierName)
    .WithEnvironment("SIGNING_KEY", signingKey)
    .WithEnvironment("POSTGRES_USER", postGresUser)
    .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
    .WithEnvironment("POSTGRES_DB", postGresDb)
    .WithEnvironment("POSTGRES_JDBC", postGresJdbc)
    .WithHttpEndpoint(port: 80, targetPort: 8080, name: "swiyu-oid4vp-endpoint")
    .WithExternalHttpEndpoints();

    //var swiyuVerifierMgmt = builder.AddContainer("swiyu-verifier-mgmt", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-management", "latest")
    //    .WithEnvironment("EXTERNAL_URL", externalVerifierUrl)
    //    .WithEnvironment("POSTGRES_USER", postGresUser)
    //    .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
    //    .WithEnvironment("POSTGRES_DB", postGresDb)
    //    .WithEnvironment("POSTGRES_JDBC", postGresJdbc)
    //    .WithHttpEndpoint(port: 8082, targetPort: 8080, name: "swiyu-verifier-mgmt-endpoint");

    builder.AddProject<Projects.EmployeeOnboarding>("employeeonboarding")
        .WithExternalHttpEndpoints();

builder.Build().Run();

