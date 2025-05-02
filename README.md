# CleanArchitecture

## Qué editar para ejecutar el proyecto principal correctamente

1. Dentro de appsettings modificar `EmailSettings`:
   1. `FromAddress`: correo que "enviará" los emails al crear un nuevo registro.
   2. `ApiKey`: [contraseña de aplicación generada por gmail](https://myaccount.google.com/apppasswords)
   3. `FromName`: nombre de quién envía los correos electrónicos
2. Dentro de la clase `CreateStreamerCommandHandler.cs` agregar un correo a quién le llegarán los mails al crear un nuevo **Streamer**.
3. Opcionalmente, dentro de la clase `StreamerDbContextSeed.cs` modificar `CreatedBy` si se quiere agregar un usuario más "personalizado"
