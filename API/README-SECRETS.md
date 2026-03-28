# Configuración de Secrets para Desarrollo

Este proyecto usa **User Secrets** de .NET para almacenar credenciales sensibles que **NO deben subirse a GitHub**.

## 🔐 Configurar Twilio (requerido)

Ejecutá estos comandos desde la carpeta `API`:

```powershell
dotnet user-secrets set "Twilio:AccountSid" "TU_ACCOUNT_SID"
dotnet user-secrets set "Twilio:AuthToken" "TU_AUTH_TOKEN"
dotnet user-secrets set "Twilio:From" "whatsapp:+14155238886"
dotnet user-secrets set "Twilio:ToTest" "whatsapp:+TU_NUMERO"
dotnet user-secrets set "Twilio:VerificationCodeContentSid" "TU_CONTENT_SID"
```

## 📍 Dónde se guardan

Los secrets se guardan en tu máquina en:
```
%APPDATA%\Microsoft\UserSecrets\e381a0fa-3d19-413b-bee3-ab2be8036d63\secrets.json
```

**Nunca se suben a GitHub** ✅

## 🔍 Ver tus secrets actuales

```powershell
dotnet user-secrets list
```

## 🗑️ Borrar un secret

```powershell
dotnet user-secrets remove "Twilio:AccountSid"
```

## ⚠️ Importante

- `appsettings.json` → Se sube a GitHub (NO poner credenciales)
- `appsettings.Development.json` → NO se sube (está en .gitignore)
- **User Secrets** → NO se suben, son locales a tu máquina ✅ **RECOMENDADO**

## 🚀 Para Producción

En producción usá variables de entorno o servicios como:
- Azure Key Vault
- AWS Secrets Manager
- Variables de entorno del servidor
