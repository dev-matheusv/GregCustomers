# API

## Authentication
JWT issued by `POST /api/auth/login`.
Write endpoints require `admin` role.

## Error handling
Global middleware maps common exceptions to RFC 7807 Problem Details:
- 400: ArgumentException
- 404: KeyNotFoundException
- 409: SQL unique constraint violations (email/address)
- 500: unexpected errors