var builder = DistributedApplication.CreateBuilder(args);

//EXTERNAL_URL: ${ EXTERNAL_URL}
//OPENID_CLIENT_METADATA_FILE: ${ OPENID_CLIENT_METADATA_FILE}
//VERIFIER_DID: ${ VERIFIER_DID}
//DID_VERIFICATION_METHOD: ${ DID_VERIFICATION_METHOD}
//VERIFIER_NAME: ${ VERIFIER_NAME}
//SIGNING_KEY: ${ SIGNING_KEY}

//# Persistence
//POSTGRES_USER: "verifier_mgmt_user"
//POSTGRES_PASSWORD: "secret"
//POSTGRES_DB: "verifier_db"
//POSTGRES_JDBC: "jdbc:postgresql://verifier_postgres:5432/verifier_db"

// Add a parameter named "example-parameter-name"
var ExternalVerifierUrl = builder.AddParameter("ExternalVerifierUrl");
var OpenIdClientMetaDataFile = builder.AddParameter("OpenIdClientMetaDataFile");
var VerifierDid = builder.AddParameter("VerifierDid");
var DidVerifierMethod = builder.AddParameter("DidVerifierMethod");
var VerifierName = builder.AddParameter("VerifierName");
var SigningKey = builder.AddParameter("SigningKey", secret: true);
var PostGresUser = builder.AddParameter("PostGresUser");
var PostGresPassword = builder.AddParameter("PostGresPassword", secret: true);
var PostGresDb = builder.AddParameter("PostGresDb");
var PostGresJdbc = builder.AddParameter("PostGresJdbc");

var swiyuOid4vp = builder.AddContainer("swiyu-oid4vp", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-oid4vp", "latest")
    .WithBindMount(Path.GetFullPath("Properties/swiyu-oid4vp"), "/configfiles")
    .WithEnvironment("EXTERNAL_URL", ExternalVerifierUrl)
    .WithEnvironment("OPENID_CLIENT_METADATA_FILE", OpenIdClientMetaDataFile)
    .WithEnvironment("VERIFIER_DID", VerifierDid)
    .WithEnvironment("DID_VERIFICATION_METHOD", DidVerifierMethod)
    .WithEnvironment("VERIFIER_NAME", VerifierName)
    .WithEnvironment("SIGNING_KEY", SigningKey)
    .WithEnvironment("POSTGRES_USER", PostGresUser)
    .WithEnvironment("POSTGRES_PASSWORD", PostGresPassword)
    .WithEnvironment("POSTGRES_DB", PostGresDb)
    .WithEnvironment("POSTGRES_JDBC", PostGresJdbc)
    .WithHttpEndpoint(port: 8083, targetPort:8080, name: "swiyu-oid4vp-endpoint");

//OID4VP_URL: ${ OID4VP_URL}
//# Persistence
//POSTGRES_USER: "verifier_mgmt_user"
//POSTGRES_PASSWORD: "secret"
//POSTGRES_DB: "verifier_db"
//POSTGRES_JDBC: "jdbc:postgresql://verifier_postgres:5432/verifier_db"

var swiyuVerifierMgmt = builder.AddContainer("swiyu-verifier-mgmt", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-management", "latest")
    .WithEnvironment("EXTERNAL_URL", "verifier_db")
    .WithEnvironment("POSTGRES_USER", PostGresUser)
    .WithEnvironment("POSTGRES_PASSWORD", PostGresPassword)
    .WithEnvironment("POSTGRES_DB", PostGresDb)
    .WithEnvironment("POSTGRES_JDBC", PostGresJdbc)
    .WithHttpEndpoint(port: 8082, targetPort: 8080, name: "swiyu-verifier-mgmt-endpoint");

builder.AddProject<Projects.EmployeeOnboarding>("employeeonboarding");

builder.Build().Run();

