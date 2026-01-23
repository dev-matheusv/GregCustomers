# Technical decisions

## Why store the logo in the database
The challenge is kept self-contained without external dependencies.
Production alternative: upload to object storage (Azure Blob / S3) and store only URL + metadata.

## Why Dapper + Stored Procedures for writes
Stored procedures are explicit, easy to review and match the challenge requirements.
Dapper keeps the command layer lightweight and fast.

## Why EF Core for reads
EF Core read models simplify queries, pagination and mapping while keeping the write model independent.

## Why simple JWT instead of Identity
The scope is a technical test with limited time.
In production, Identity + hashed credentials, refresh tokens and key rotation would be adopted.