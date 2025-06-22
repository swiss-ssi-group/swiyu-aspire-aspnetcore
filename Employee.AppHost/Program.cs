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
var EXTERNAL_URL = builder.AddParameter("EXTERNAL_URL");
var OPENID_CLIENT_METADATA_FILE = builder.AddParameter("OPENID_CLIENT_METADATA_FILE");
var VERIFIER_DID = builder.AddParameter("VERIFIER_DID");
var DID_VERIFICATION_METHOD = builder.AddParameter("DID_VERIFICATION_METHOD");
var VERIFIER_NAME = builder.AddParameter("VERIFIER_NAME");
var SIGNING_KEY = builder.AddParameter("SIGNING_KEY", secret: true);
var POSTGRES_USER = builder.AddParameter("POSTGRES_USER");
var POSTGRES_PASSWORD = builder.AddParameter("POSTGRES_PASSWORD", secret: true);
var POSTGRES_DB = builder.AddParameter("POSTGRES_DB");
var POSTGRES_JDBC = builder.AddParameter("POSTGRES_JDBC");

var swiyuOid4vp = builder.AddContainer("swiyu-oid4vp", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-oid4vp", "latest")
    .WithEnvironment("EXTERNAL_URL", EXTERNAL_URL)
    .WithEnvironment("OPENID_CLIENT_METADATA_FILE", OPENID_CLIENT_METADATA_FILE)
    .WithEnvironment("VERIFIER_DID", VERIFIER_DID)
    .WithEnvironment("DID_VERIFICATION_METHOD", DID_VERIFICATION_METHOD)
    .WithEnvironment("VERIFIER_NAME", VERIFIER_NAME)
    .WithEnvironment("SIGNING_KEY", SIGNING_KEY)
    .WithEnvironment("POSTGRES_USER", POSTGRES_USER)
    .WithEnvironment("POSTGRES_PASSWORD", POSTGRES_PASSWORD)
    .WithEnvironment("POSTGRES_DB", POSTGRES_DB)
    .WithEnvironment("POSTGRES_JDBC", POSTGRES_JDBC)
    .WithHttpEndpoint(port: 8083, targetPort:8080, name: "swiyu-oid4vp-endpoint");

//OID4VP_URL: ${ OID4VP_URL}
//# Persistence
//POSTGRES_USER: "verifier_mgmt_user"
//POSTGRES_PASSWORD: "secret"
//POSTGRES_DB: "verifier_db"
//POSTGRES_JDBC: "jdbc:postgresql://verifier_postgres:5432/verifier_db"

var swiyuVerifierMgmt = builder.AddContainer("swiyu-verifier-mgmt", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-management", "latest")
    .WithEnvironment("EXTERNAL_URL", "verifier_db")
    .WithEnvironment("POSTGRES_USER", POSTGRES_USER)
    .WithEnvironment("POSTGRES_PASSWORD", POSTGRES_PASSWORD)
    .WithEnvironment("POSTGRES_DB", POSTGRES_DB)
    .WithEnvironment("POSTGRES_JDBC", POSTGRES_JDBC)
    .WithHttpEndpoint(port: 8082, targetPort: 8080, name: "swiyu-verifier-mgmt-endpoint");

builder.AddProject<Projects.EmployeeOnboarding>("employeeonboarding");

builder.Build().Run();

