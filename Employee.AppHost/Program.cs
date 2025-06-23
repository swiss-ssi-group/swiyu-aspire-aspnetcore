using Microsoft.Extensions.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

var postGresUser = builder.AddParameter("postgresuser");
var postGresPassword = builder.AddParameter("postgrespassword", secret: true);
var postGresDbIssuer = builder.AddParameter("postgresdbissuer");
var postGresJdbcIssuer = builder.AddParameter("postgresjdbcissuer");
var postGresDbVerifier = builder.AddParameter("postgresdbverifier");
var postGresJdbcVerifier = builder.AddParameter("postgresjdbcverifier");

// Issuer
var issuerExternalUrl = builder.AddParameter("issuerexternalurl");
var issuerId = builder.AddParameter("issuerid");
var issuerDidSdJwtVerficiationMethod = builder.AddParameter("issuerdidsdjwtverificationmethod");
var issuerSdJwtKey = builder.AddParameter("issuersdjwtkey");
var issuerOpenIdConfigFile = builder.AddParameter("issueropenidconfigfile");
var issuerMetaDataConfigFile = builder.AddParameter("issuermetadataconfigfile");
var issuerTokenTtl = builder.AddParameter("issuertokenttl");

// Issuer
var swiyuOid4vci = builder.AddContainer("swiyu-oid4vci", "ghcr.io/swiyu-admin-ch/eidch-issuer-agent-oid4vci", "latest")
    .WithEnvironment("EXTERNAL_URL", issuerExternalUrl)
    .WithEnvironment("ISSUER_ID", issuerId)
    .WithEnvironment("DID_SDJWT_VERIFICATION_METHOD", issuerDidSdJwtVerficiationMethod)
    .WithEnvironment("SDJWT_KEY", issuerSdJwtKey)
    .WithEnvironment("OPENID_CONFIG_FILE", issuerOpenIdConfigFile)
    .WithEnvironment("METADATA_CONFIG_FILE", issuerMetaDataConfigFile)
    .WithEnvironment("TOKEN_TTL", issuerTokenTtl)
    .WithEnvironment("POSTGRES_USER", postGresUser)
    .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
    .WithEnvironment("POSTGRES_DB", postGresDbIssuer)
    .WithEnvironment("POSTGRES_JDBC", postGresJdbcIssuer)
    .WithHttpEndpoint(port: 80, targetPort: 8080, name: "swiyu-oid4vci-endpoint")
    .WithExternalHttpEndpoints();

// Verifier
var verifierExternalUrl = builder.AddParameter("externalverifierurl");
var verifierOpenIdClientMetaDataFile = builder.AddParameter("openidclientmetadatafile");
var verifierDid = builder.AddParameter("verifierdid");
var didVerifierMethod = builder.AddParameter("didverifiermethod");
var verifierName = builder.AddParameter("verifiername");
var verifierSigningKey = builder.AddParameter("signingkey");

// Verifier
var swiyuOid4vp = builder.AddContainer("swiyu-oid4vp", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-oid4vp", "latest")
    .WithEnvironment("EXTERNAL_URL", verifierExternalUrl)
    .WithEnvironment("OPENID_CLIENT_METADATA_FILE", verifierOpenIdClientMetaDataFile)
    .WithEnvironment("VERIFIER_DID", verifierDid)
    .WithEnvironment("DID_VERIFICATION_METHOD", didVerifierMethod)
    .WithEnvironment("VERIFIER_NAME", verifierName)
    .WithEnvironment("SIGNING_KEY", verifierSigningKey)
    .WithEnvironment("POSTGRES_USER", postGresUser)
    .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
    .WithEnvironment("POSTGRES_DB", postGresDbVerifier)
    .WithEnvironment("POSTGRES_JDBC", postGresJdbcVerifier)
    .WithHttpEndpoint(port: 80, targetPort: 8080, name: "swiyu-oid4vp-endpoint")
    .WithExternalHttpEndpoints();

//var swiyuVerifierMgmt = builder.AddContainer("swiyu-verifier-mgmt", "ghcr.io/swiyu-admin-ch/eidch-verifier-agent-management", "latest")
//    .WithEnvironment("EXTERNAL_URL", externalVerifierUrl)
//    .WithEnvironment("POSTGRES_USER", postGresUser)
//    .WithEnvironment("POSTGRES_PASSWORD", postGresPassword)
//    .WithEnvironment("POSTGRES_DB", postGresDbVerifier)
//    .WithEnvironment("POSTGRES_JDBC", postGresJdbcVerifier)
//    .WithHttpEndpoint(port: 8082, targetPort: 8080, name: "swiyu-verifier-mgmt-endpoint");

builder.AddProject<Projects.EmployeeOnboarding>("employeeonboarding")
        .WithExternalHttpEndpoints();

builder.Build().Run();

