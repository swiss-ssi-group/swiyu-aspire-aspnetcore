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

var swiyuOid4vp = builder.AddContainer("swiyu-oid4vp", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-oid4vp", "latest")
    .WithEnvironment("EXTERNAL_URL", "verifier_db")
    .WithEnvironment("OPENID_CLIENT_METADATA_FILE", "verifier_db")
    .WithEnvironment("VERIFIER_DID", "verifier_db")
    .WithEnvironment("DID_VERIFICATION_METHOD", "verifier_db")
    .WithEnvironment("VERIFIER_NAME", "verifier_db")
    .WithEnvironment("SIGNING_KEY", "verifier_db")
    .WithEnvironment("POSTGRES_USER", "verifier_db")
    .WithEnvironment("POSTGRES_PASSWORD", "verifier_db")
    .WithEnvironment("POSTGRES_DB", "verifier_db")
    .WithEnvironment("POSTGRES_JDBC", "verifier_db")
    .WithHttpEndpoint(port: 8083, targetPort:8080, name: "swiyu-oid4vp-endpoint");

//OID4VP_URL: ${ OID4VP_URL}
//# Persistence
//POSTGRES_USER: "verifier_mgmt_user"
//POSTGRES_PASSWORD: "secret"
//POSTGRES_DB: "verifier_db"
//POSTGRES_JDBC: "jdbc:postgresql://verifier_postgres:5432/verifier_db"

var swiyuVerifierMgmt = builder.AddContainer("swiyu-verifier-mgmt", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-management", "latest")
    .WithEnvironment("EXTERNAL_URL", "verifier_db")
    .WithEnvironment("POSTGRES_USER", "verifier_db")
    .WithEnvironment("POSTGRES_PASSWORD", "verifier_db")
    .WithEnvironment("POSTGRES_DB", "verifier_db")
    .WithEnvironment("POSTGRES_JDBC", "verifier_db")
    .WithHttpEndpoint(port: 8082, targetPort: 8080, name: "swiyu-verifier-mgmt-endpoint");

builder.AddProject<Projects.EmployeeOnboarding>("employeeonboarding");

builder.Build().Run();

