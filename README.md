# **ESTRUCTURA DE SOFTWARE**

# **SERVICIO REGISTRO DE PERSONAS API**

|  |  |
| --- | --- |
| **CAPA** | BACKEND |
| **PLATAFORMA** | SERVER – WINDOWS |
| **ACCESO** | http://localhost:[PUERTO]/ |
| **TIPO** | .NET |

## 1. DESCRIPCIÓN GENERAL

El servicio Registro de Personas API proporciona endpoints para gestionar operaciones relacionadas con el registro de personas, incluyendo la validación y almacenamiento de datos personales y de contacto como teléfonos, correos electrónicos y direcciones físicas.

## 2. CONFIGURACIÓN INICIAL

### 2.1. Creación de Base de Datos

Para el funcionamiento correcto del sistema, se debe crear una base de datos con las siguientes tablas:

```sql
-- Tabla principal de personas
CREATE TABLE Personas (
    DocumentoIdentidad VARCHAR(20) PRIMARY KEY,
    Nombres VARCHAR(100) NOT NULL,
    Apellidos VARCHAR(100) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE()
);

-- Teléfonos asociados
CREATE TABLE Telefonos (
    Id INT PRIMARY KEY IDENTITY,
    Numero VARCHAR(20) NOT NULL,
    PersonaId VARCHAR(20) NOT NULL,
    CONSTRAINT FK_Telefonos_Personas FOREIGN KEY (PersonaId)
        REFERENCES Personas(DocumentoIdentidad)
        ON DELETE CASCADE
);

-- Correos electrónicos asociados
CREATE TABLE CorreosElectronicos (
    Id INT PRIMARY KEY IDENTITY,
    Direccion VARCHAR(100) NOT NULL,
    PersonaId VARCHAR(20) NOT NULL,
    CONSTRAINT FK_CorreosElectronicos_Personas FOREIGN KEY (PersonaId)
        REFERENCES Personas(DocumentoIdentidad)
        ON DELETE CASCADE
);

-- Direcciones físicas asociadas
CREATE TABLE DireccionesFisicas (
    Id INT PRIMARY KEY IDENTITY,
    Calle VARCHAR(100) NOT NULL,
    Ciudad VARCHAR(50) NOT NULL,
    CodigoPostal VARCHAR(10),
    PersonaId VARCHAR(20) NOT NULL,
    CONSTRAINT FK_DireccionesFisicas_Personas FOREIGN KEY (PersonaId)
        REFERENCES Personas(DocumentoIdentidad)
        ON DELETE CASCADE
);
```

### 2.2. Configuración de Conexión

En el archivo `appsettings.json`, se debe configurar la cadena de conexión según la base de datos que se esté utilizando:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=[SERVER];Initial Catalog=[DATABASE];Persist Security Info=True;User ID=[USER];Password=[PASSWORD];Connect Timeout=3600"
  }
}
```

## 3. MÉTODOS

### 3.1. Gestión de Personas

#### 3.1.1. Registrar Persona

Registra una nueva persona en el sistema con sus datos de contacto.

Acceso: `api/Persona/registrar`  
Formato: JSON  
Servicio: REST / POST

##### 3.1.1.1. Entrada

| **Nombre** | **Descripción** | **Tipo** | **Requerido** |
|------------|-----------------|----------|---------------|
| documentoIdentidad | Documento de identidad | String | Sí |
| nombres | Nombres de la persona | String | Sí |
| apellidos | Apellidos de la persona | String | Sí |
| fechaNacimiento | Fecha de nacimiento | Date | Sí |
| telefonos | Lista de teléfonos | Array | No |
| correos | Lista de correos electrónicos | Array | No |
| direcciones | Lista de direcciones físicas | Array | No |

**Estructura de teléfonos:**
| **Nombre** | **Descripción** | **Tipo** | **Requerido** |
|------------|-----------------|----------|---------------|
| numero | Número telefónico | String | Sí |

**Estructura de correos:**
| **Nombre** | **Descripción** | **Tipo** | **Requerido** |
|------------|-----------------|----------|---------------|
| direccion | Dirección de correo electrónico | String | Sí |

**Estructura de direcciones:**
| **Nombre** | **Descripción** | **Tipo** | **Requerido** |
|------------|-----------------|----------|---------------|
| calle | Nombre de la calle | String | Sí |
| ciudad | Ciudad | String | Sí |
| codigoPostal | Código postal | String | No |

Ejemplo de entrada:
```json
{
  "documentoIdentidad": "1234567890",
  "nombres": "Juan Carlos",
  "apellidos": "García López",
  "fechaNacimiento": "1990-05-15",
  "telefonos": [
    {
      "numero": "3123456789"
    },
    {
      "numero": "6043211234"
    }
  ],
  "correos": [
    {
      "direccion": "juan.garcia@email.com"
    }
  ],
  "direcciones": [
    {
      "calle": "Calle 123 # 45-67",
      "ciudad": "Medellín",
      "codigoPostal": "050001"
    }
  ]
}
```

##### 3.1.1.2. Salida

En caso de éxito:
```json
{
  "exito": true,
  "mensaje": "Registro exitoso",
  "detalle": "La persona fue registrada exitosamente.",
  "resultado": {
    "documentoIdentidad": "1234567890",
    "nombres": "Juan Carlos",
    "apellidos": "García López",
    "fechaNacimiento": "1990-05-15T00:00:00",
    "telefonos": [
      {
        "numero": "3123456789"
      },
      {
        "numero": "6043211234"
      }
    ],
    "correos": [
      {
        "direccion": "juan.garcia@email.com"
      }
    ],
    "direcciones": [
      {
        "calle": "Calle 123 # 45-67",
        "ciudad": "Medellín",
        "codigoPostal": "050001"
      }
    ]
  }
}
```

En caso de error por datos incorrectos:
```json
{
  "exito": false,
  "mensaje": "Parámetros incorrectos",
  "detalle": "El documento de identidad solo puede contener números",
  "resultado": null
}
```

En caso de error por registro duplicado:
```json
{
  "exito": false,
  "mensaje": "Registro duplicado",
  "detalle": "Ya existe una persona con el documento '1234567890'.",
  "resultado": null
}
```

## 4. REGLAS DE VALIDACIÓN

### 4.1. Validación de Documento de Identidad
- El documento de identidad es obligatorio.
- Solo puede contener caracteres numéricos.
- No puede existir un registro previo con el mismo documento de identidad.

### 4.2. Validación de Nombres y Apellidos
- Los campos de nombres y apellidos son obligatorios.
- Solo pueden contener caracteres alfabéticos y espacios.

### 4.3. Validación de Información de Contacto
- Debe registrarse al menos un medio de contacto (correo electrónico o dirección física).
- Los campos de número telefónico, dirección de correo y datos de dirección física (calle y ciudad) son obligatorios cuando se incluyen.

## 5. CONSIDERACIONES TÉCNICAS

### 5.1. Manejo de Transacciones
- Todas las operaciones de guardado se realizan dentro de una transacción.
- Si ocurre algún error durante el proceso de guardado, se hace rollback de la transacción.

### 5.2. Manejo de Errores
- Los errores de validación son manejados y retornados como respuestas con información detallada.
- Los errores de base de datos son capturados y notificados al cliente.

### 5.3. Arquitectura
- El sistema implementa una arquitectura en capas:
  - **Controladores**: Manejo de solicitudes HTTP y respuestas.
  - **Servicios**: Lógica de negocio y validaciones.
  - **Repositorios**: Acceso a datos y operaciones CRUD.
  - **Entidades**: Modelos de datos.
  - **DTOs**: Objetos de transferencia de datos para las API.
