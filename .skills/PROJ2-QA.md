# QA für PROJ-2: Auth-System

## Test-Coverage

### Unit Tests

#### RegisterEndpoint
- `Register_ValidRequest_ReturnsCreatedUser`
- `Register_DuplicateEmail_ReturnsBadRequest`
- `Register_InvalidEmail_ReturnsBadRequest`
- `Register_WeakPassword_ReturnsBadRequest`
- `Register_MissingRequiredFields_ReturnsBadRequest`

#### LoginEndpoint
- `Login_ValidCredentials_ReturnsToken`
- `Login_WrongPassword_ReturnsUnauthorized`
- `Login_NonExistentUser_ReturnsUnauthorized`
- `Login_InactiveUser_ReturnsUnauthorized`

#### RefreshTokenEndpoint
- `Refresh_ValidToken_ReturnsNewTokens`
- `Refresh_ExpiredToken_ReturnsUnauthorized`
- `Refresh_InvalidToken_ReturnsUnauthorized`

#### KassaCode Endpoints
- `GenerateKassaCode_ValidRequest_Returns6DigitCode`
- `LoginWithKassaCode_ValidCode_ReturnsToken`
- `LoginWithKassaCode_ExpiredCode_ReturnsBadRequest`
- `LoginWithKassaCode_InvalidCode_ReturnsUnauthorized`

#### QR Login Endpoints
- `GenerateQrLogin_ValidRequest_ReturnsQrToken`
- `LoginWithQr_ValidToken_ReturnsToken`
- `LoginWithQr_ExpiredToken_ReturnsBadRequest`

## Security Tests

- [ ] Password wird mit BCrypt gehashed
- [ ] JWT enthält korrekte Claims (UserId, Email, Role)
- [ ] Refresh Token wird in Datenbank gespeichert
- [ ] Abgelaufene Tokens werden abgelehnt
- [ ] KassaCode läuft nach 5 Minuten ab
- [ ] QR Token läuft nach 24 Stunden ab

## API Tests

### Happy Path

```
POST /api/auth/register
→ 201 Created + UserId

POST /api/auth/login  
→ 200 OK + AccessToken, RefreshToken

POST /api/auth/refresh
→ 200 OK + neue Tokens

POST /api/auth/logout
→ 204 No Content

POST /api/auth/kassa-code
→ 200 OK + Code

POST /api/auth/kassa-code/login
→ 200 OK + AccessToken, RefreshToken

POST /api/auth/qr/generate
→ 200 OK + QRToken

POST /api/auth/qr/login
→ 200 OK + AccessToken, RefreshToken
```

### Error Cases

```
POST /api/auth/register (duplicate email)
→ 400 Bad Request + "E-Mail bereits registriert"

POST /api/auth/login (wrong password)
→ 401 Unauthorized + "Ungültige Anmeldedaten"

POST /api/auth/refresh (invalid token)
→ 401 Unauthorized

POST /api/auth/kassa-code/login (expired)
→ 400 Bad Request + "Code abgelaufen"
```

## Performance

- [ ] Login Response < 200ms (ohne Netzwerk)
- [ ] Token Refresh < 100ms
- [ ] Keine N+1 Queries bei Login

## Logging

- Login-Fehler mit E-Mail (nicht Passwort!)
- Registrierung mit UserId
- Logout mit UserId
- Token-Refresh mit UserId
