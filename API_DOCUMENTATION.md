# TCC API Documentation

**Base URL:** `http://localhost:5095` (development)  
**API Version:** v1  
**Authentication:** JWT Bearer Token  
**Content-Type:** `application/json`  
**CORS:** Enabled for all origins

---

## Table of Contents

1. [Authentication](#authentication)
2. [Common Response Format](#common-response-format)
3. [Error Handling](#error-handling)
4. [Roles](#roles)
5. [User Controller](#user-controller)
6. [Patient Controller](#patient-controller)
7. [Psychologist Controller](#psychologist-controller)
8. [Appointment Controller](#appointment-controller)
9. [Admin Controller](#admin-controller)
10. [Data Models](#data-models)

---

## Authentication

The API uses **JWT Bearer Token** authentication. Include the token in the Authorization header:

```
Authorization: Bearer <your_jwt_token>
```

### Login Endpoint
- **POST** `/api/user/login`
- Returns JWT token on successful authentication
- Token must be included in subsequent requests

### Token Claims
- `Id`: User ID
- `Role`: User role (C=Patient, P=Psychologist, A=Admin)

---

## Common Response Format

All endpoints return a standardized `Result<T>` wrapper:

```json
{
  "success": true,
  "data": { ... }
}
```

### Properties
| Property | Type | Description |
|----------|------|-------------|
| `success` | boolean | Indicates if the operation succeeded |
| `data` | T (generic) | Response payload (null on error) |

### HTTP Status Codes
| Code | Description |
|------|-------------|
| 200 | Success |
| 400 | Bad Request (validation failed) |
| 401 | Unauthorized (invalid/missing token) |
| 403 | Forbidden (insufficient role) |
| 404 | Not Found |
| 500 | Internal Server Error |

---

## Error Handling

Validation errors return `400 Bad Request` with `success: false` and error details in `data`.

Server errors return `500 Internal Server Error` with error message.

---

## Roles

| Role | Code | Description |
|------|------|-------------|
| Patient | `C` | Clinic client/patient |
| Psychologist | `P` | Licensed psychologist |
| Admin | `A` | System administrator |

Role is returned in login response and validated via `[Authorize(Roles = "...")]` attributes.

---

## User Controller

**Base Path:** `/api/user`

### 1. Login

Authenticate user and receive JWT token.

**Endpoint:** `POST /api/user/login`  
**Auth:** Public (no token required)

#### Request Body
```json
{
  "cpf": "string (11 chars, required)",
  "password": "string (min 6 chars, required)"
}
```

#### Response (200 OK)
```json
{
  "success": true,
  "data": {
    "id": 1,
    "token": "eyJhbGciOiJIUzI1NiIs...",
    "role": "C"
  }
}
```

#### Response Fields
| Field | Type | Description |
|-------|------|-------------|
| `id` | integer | User ID |
| `token` | string | JWT Bearer token |
| `role` | string | User role (C, P, or A) |

---

### 2. Create Address

Create a new address for a patient.

**Endpoint:** `POST /api/user/createAddress`  
**Auth:** Required (Role: C - Patient)

#### Request Body
```json
{
  "id": 0,
  "cep": "string (8 chars, required)",
  "city": "string (required)",
  "state": "string (required)",
  "street": "string (required)",
  "number": "string (required)",
  "neighborhood": "string (required)",
  "isApartment": true,
  "floor": 1,
  "apartmentNumber": 101,
  "patientId": 1
}
```

#### Response (200 OK)
```json
{
  "success": true,
  "data": {
    "id": 1,
    "cep": "01234567",
    "city": "São Paulo",
    "state": "SP",
    "number": "123",
    "isApartment": true,
    "apartmentNumber": 101
  }
}
```

---

### 3. Edit Address

Update an existing address.

**Endpoint:** `PUT /api/user/editAddress`  
**Auth:** Required

#### Request Body
Same as Create Address (must include `id` of address to update)

#### Response (200 OK)
```json
{
  "success": true,
  "data": true
}
```

---

### 4. Create Phone Number

Add a phone number for a patient.

**Endpoint:** `POST /api/user/createNumber`  
**Auth:** Required (Role: C - Patient)

#### Request Body
```json
{
  "id": 0,
  "number": "string (9 digits, required)",
  "isEmergencyContact": true,
  "countryCode": "55",
  "ddd": "11"
}
```

#### Response (200 OK)
```json
{
  "success": true,
  "data": {
    "id": 1,
    "number": "999999999",
    "ddd": "11"
  }
}
```

---

### 5. Create Email

Add an email for a patient.

**Endpoint:** `POST /api/user/createEmail`  
**Auth:** Required (Role: C - Patient)

#### Request Body
```json
{
  "id": 0,
  "address": "user",
  "extension": "example.com"
}
```

#### Response (200 OK)
```json
{
  "success": true,
  "data": {
    "id": 1,
    "address": "user",
    "extension": "example.com"
  }
}
```

---

## Patient Controller

**Base Path:** `/api/patient`

### 1. Create Patient

Register a new patient.

**Endpoint:** `POST /api/patient/createPatient`  
**Auth:** Public (commented Authorize)

#### Request Body
```json
{
  "name": "string (required)",
  "lastName": "string (required)",
  "cpf": "string (11 chars, required)",
  "age": "string (1-100, required)",
  "password": "string (min 6 chars, required)"
}
```

#### Response (200 OK)
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "João",
    "lastName": "Silva",
    "cpf": "12345678901",
    "age": "25",
    "role": "C"
  }
}
```

---

### 2. Get Patient by ID

Retrieve patient details.

**Endpoint:** `GET /api/patient/get-by-id/{id}`  
**Auth:** Required (Role: P - Psychologist)

#### Path Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| `id` | integer | Patient ID |

#### Response (200 OK)
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "João",
    "lastName": "Silva",
    "cpf": "12345678901",
    "age": "25",
    "role": "C"
  }
}
```

---

### 3. List All Patients

Get list of all patients.

**Endpoint:** `GET /api/patient/list`  
**Auth:** Required (Role: P, A)

#### Response (200 OK)
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "João",
      "lastName": "Silva",
      "cpf": "12345678901",
      "age": "25",
      "role": "C"
    }
  ]
}
```

---

### 4. Link Patient to Psychologist

Associate a patient with a psychologist.

**Endpoint:** `POST /api/patient/link-psychologist`  
**Auth:** Required

#### Request Body
```json
{
  "patientId": 1,
  "psychologistId": 1
}
```

#### Response (200 OK)
```json
{
  "success": true,
  "data": true
}
```

---

### 5. Get Patient's Psychologist

Get the psychologist assigned to a patient.

**Endpoint:** `GET /api/patient/{patientId}/psychologist`  
**Auth:** Required

#### Path Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| `patientId` | integer | Patient ID |

#### Response (200 OK)
```json
{
  "success": true,
  "data": {
    "patientId": 1,
    "psychologistId": 1,
    "name": "Dr. Maria",
    "lastName": "Santos",
    "specialization": "Clinical Psychology"
  }
}
```

---

## Psychologist Controller

**Base Path:** `/api/psychologist`

### 1. List All Psychologists

Get list of all psychologists.

**Endpoint:** `GET /api/psychologist/list`  
**Auth:** Required (Role: A - commented out)

#### Response (200 OK)
```json
{
  "success": true,
  "data": [
    {
      "id": 1,
      "name": "Maria",
      "lastName": "Santos",
      "cpf": "12345678901",
      "crp": "12345678",
      "specialization": "Clinical Psychology"
    }
  ]
}
```

---

### 2. Register Psychologist

Create a new psychologist account.

**Endpoint:** `POST /api/psychologist/register`  
**Auth:** Public (commented Authorize)

#### Request Body
```json
{
  "name": "string (required)",
  "lastName": "string (required)",
  "cpf": "string (11 chars, required)",
  "age": "string (1-100, required)",
  "password": "string (min 6 chars, required)",
  "crp": "string (8 chars, required)",
  "specialization": "string (max 50 chars, required)"
}
```

#### Response (200 OK)
```json
{
  "success": true,
  "data": {
    "id": 1,
    "name": "Maria",
    "lastName": "Santos",
    "cpf": "12345678901",
    "crp": "12345678",
    "specialization": "Clinical Psychology"
  }
}
```

---

### 3. Edit Psychologist

Update psychologist information.

**Endpoint:** `PUT /api/psychologist/edit`  
**Auth:** Required

#### Request Body
```json
{
  "id": 1,
  "name": "string (required)",
  "lastName": "string (required)",
  "cpf": "string (11 chars, required)",
  "age": "string (1-100, required)",
  "password": "string (min 6 chars, required)",
  "crp": "string (7 chars, required)",
  "specialization": "string (max 50 chars, required)"
}
```

#### Response (200 OK)
```json
{
  "success": true,
  "data": true
}
```

---

## Appointment Controller

**Base Path:** `/api/appointment`

### 1. Create Appointment

Schedule a new appointment.

**Endpoint:** `POST /api/appointment/Create`  
**Auth:** Required

#### Request Body
```json
{
  "type": "C",
  "dateAndTime": "2026-09-15T14:30:00",
  "notes": "string (optional)",
  "hadTreatment": "S",
  "physicalHealth": "string (max 40 chars, optional)",
  "maritalStatus": "S",
  "habits": "string (optional)",
  "searchReason": "string (optional)",
  "patientId": 1,
  "psychologistId": 1
}
```

#### Field Details
| Field | Type | Constraints | Description |
|-------|------|-------------|-------------|
| `type` | string | 1 char, required | Appointment type |
| `dateAndTime` | datetime | required | Appointment date/time |
| `notes` | string | optional | Clinical notes |
| `hadTreatment` | string | 1 char, optional | Previous treatment (S/N) |
| `physicalHealth` | string | max 40 chars, optional | Physical health notes |
| `maritalStatus` | string | 1 char, optional | Marital status |
| `habits` | string | optional | Patient habits |
| `searchReason` | string | optional | Reason for seeking therapy |
| `patientId` | integer | required | Patient ID |
| `psychologistId` | integer | required | Psychologist ID |

#### Response (200 OK)
```json
{
  "success": true,
  "data": true
}
```

---

### 2. Get Appointment by ID

Retrieve appointment details (simple version).

**Endpoint:** `GET /api/appointment/get-by-id/{id}`  
**Auth:** Required (Role: P, A - commented out)

#### Path Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| `id` | integer | Appointment ID |

#### Response (200 OK)
```json
{
  "success": true,
  "data": {
    "id": 1,
    "type": "C",
    "day": 15,
    "month": 9,
    "hour": 14,
    "patientId": 1,
    "psychologistId": 1
  }
}
```

---

### 3. Get Availability by Date

Get available time slots for a psychologist on a specific date.

**Endpoint:** `GET /api/appointment/availability/{psychologistId}`  
**Auth:** Required

#### Path Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| `psychologistId` | integer | Psychologist ID |

#### Query Parameters
| Parameter | Type | Description |
|-----------|------|-------------|
| `date` | date (DateOnly) | Date to check availability (format: YYYY-MM-DD) |

#### Response (200 OK)
```json
{
  "success": true,
  "data": {
    "psychologistId": 1,
    "date": "2026-09-15",
    "weekDay": 2,
    "avaliability": true,
    "hours": [
      { "hour": "09:00", "available": true },
      { "hour": "10:00", "available": false },
      { "hour": "14:00", "available": true }
    ]
  }
}
```

---

### 4. Create Availability Days

Set up recurring availability schedule for a psychologist.

**Endpoint:** `POST /api/appointment/availability`  
**Auth:** Required

#### Request Body
```json
{
  "psychologistId": 1,
  "days": [
    {
      "weekDay": 1,
      "hours": ["09:00", "10:00", "11:00", "14:00", "15:00"]
    },
    {
      "weekDay": 3,
      "hours": ["09:00", "10:00", "14:00"]
    }
  ]
}
```

#### WeekDay Values
| Value | Day |
|-------|-----|
| 0 | Sunday |
| 1 | Monday |
| 2 | Tuesday |
| 3 | Wednesday |
| 4 | Thursday |
| 5 | Friday |
| 6 | Saturday |

#### Response (200 OK)
```json
{
  "success": true,
  "data": true
}
```

---

### 5. Update Appointment Status

Update the status of an appointment.

**Endpoint:** `PUT /api/appointment/status`  
**Auth:** Required

#### Request Body
```json
{
  "id": 1,
  "status": "Pendente"
}
```

#### Status Enum Values
| Value | Description |
|-------|-------------|
| `Pendente` | Pending |
| `Finalizado` | Completed |
| `Cancelado` | Cancelled |

#### Response (200 OK)
```json
{
  "success": true,
  "data": true
}
```

---

## Admin Controller

**Base Path:** `/api/admin`

### 1. Create Admin

Create a new admin user.

**Endpoint:** `POST /api/admin/createAdmin`  
**Auth:** Required (Role: A - commented out)

#### Request Body
```json
{
  "name": "string (2-100 chars, required)",
  "lastName": "string (2-100 chars, required)",
  "cpf": "string (11 chars, required)",
  "password": "string (min 6 chars, required)",
  "role": "A"
}
```

#### Response (200 OK)
```json
{
  "success": true,
  "data": true
}
```

---

## Data Models

### UserModelDTO (Base for Patient/Psychologist Registration)
```json
{
  "name": "string (required)",
  "lastName": "string (required)",
  "cpf": "string (11 chars, required)",
  "age": "string (1-100, required)",
  "password": "string (min 6 chars, required)"
}
```

### AddressEntryDTO
```json
{
  "id": 0,
  "cep": "string (required)",
  "city": "string (required)",
  "state": "string (required)",
  "street": "string (required)",
  "number": "string (required)",
  "neighborhood": "string (required)",
  "isApartment": true,
  "floor": 1,
  "apartmentNumber": 101,
  "patientId": 1
}
```

### AddressReturnDTO
```json
{
  "id": 1,
  "cep": "01234567",
  "city": "São Paulo",
  "state": "SP",
  "number": "123",
  "isApartment": true,
  "apartmentNumber": 101
}
```

### PhoneNumberEntryDTO
```json
{
  "id": 0,
  "number": "999999999",
  "isEmergencyContact": true,
  "countryCode": "55",
  "ddd": "11"
}
```

### PhoneNumberReturnDTO
```json
{
  "id": 1,
  "number": "999999999",
  "ddd": "11"
}
```

### EmailEntryDTO
```json
{
  "id": 0,
  "address": "user",
  "extension": "example.com"
}
```

### EmailReturnDTO
```json
{
  "id": 1,
  "address": "user",
  "extension": "example.com"
}
```

### ListPatientDTO
```json
{
  "id": 1,
  "name": "João",
  "lastName": "Silva",
  "cpf": "12345678901",
  "age": "25",
  "role": "C"
}
```

### ListPsychologistDTO
```json
{
  "id": 1,
  "name": "Maria",
  "lastName": "Santos",
  "cpf": "12345678901",
  "crp": "12345678",
  "specialization": "Clinical Psychology"
}
```

### CreateAppointmentDTO
```json
{
  "type": "C",
  "dateAndTime": "2026-09-15T14:30:00",
  "notes": "string",
  "hadTreatment": "S",
  "physicalHealth": "string",
  "maritalStatus": "S",
  "habits": "string",
  "searchReason": "string",
  "patientId": 1,
  "psychologistId": 1
}
```

### ReturnAppointmentDTOsimple
```json
{
  "id": 1,
  "type": "C",
  "day": 15,
  "month": 9,
  "hour": 14,
  "patientId": 1,
  "psychologistId": 1
}
```

### ReturnAppointmentDTOextend
```json
{
  "id": 1,
  "type": "C",
  "day": 15,
  "month": 9,
  "hour": 14,
  "notes": "string",
  "hadTreatment": "S",
  "physicalHealth": "string",
  "maritalStatus": "S",
  "habits": "string",
  "searchReason": "string",
  "patientId": 1,
  "psychologistId": 1
}
```

### CreateServiceDaysDTO
```json
{
  "psychologistId": 1,
  "days": [
    {
      "weekDay": 1,
      "hours": ["09:00", "10:00", "11:00"]
    }
  ]
}
```

### CreateServiceDayDTO
```json
{
  "weekDay": 1,
  "hours": ["09:00", "10:00", "11:00"]
}
```

### HourAvailabilityDTO
```json
{
  "hour": "09:00",
  "available": true
}
```

### ReturnAvailabilityDTO
```json
{
  "psychologistId": 1,
  "date": "2026-09-15",
  "weekDay": 2,
  "avaliability": true,
  "hours": [
    { "hour": "09:00", "available": true },
    { "hour": "10:00", "available": false }
  ]
}
```

### EntryUpdateAppointmentStatusDTO
```json
{
  "id": 1,
  "status": "Pendente"
}
```

### AppointmentStatus Enum
- `Pendente` - Pending
- `Finalizado` - Completed
- `Cancelado` - Cancelled

### RegisterAdminDTO
```json
{
  "name": "string",
  "lastName": "string",
  "cpf": "string (11 chars)",
  "password": "string (min 6 chars)",
  "role": "A"
}
```

### LinkPatientPsychologistDTO
```json
{
  "patientId": 1,
  "psychologistId": 1
}
```

### ReturnPatientPsychologistDTO
```json
{
  "patientId": 1,
  "psychologistId": 1,
  "name": "Maria",
  "lastName": "Santos",
  "specialization": "Clinical Psychology"
}
```

---

## Database Schema Reference

The API uses MariaDB/MySQL with the following main tables:

- `admin` - Admin users
- `patient` - Patients
- `psychologist` - Psychologists
- `appointment` - Appointments
- `patient_address` - Patient addresses
- `patient_email` - Patient emails
- `patient_phone_number` - Patient phone numbers
- `psychologist_address` - Psychologist addresses
- `psychologist_email` - Psychologist emails
- `psychologist_phone_number` - Psychologist phone numbers
- `service_days` - Psychologist recurring availability (week days)
- `avaliable_time` - Specific time slots for service days
- `patient_psychologist` - Patient-Psychologist relationships
- `payment` - Payment records

---

## Swagger UI

Available in development at: `http://localhost:5095/swagger`

---

## Quick Start for Frontend

1. **Login**: `POST /api/user/login` with CPF + password → store JWT token
2. **Include token** in Authorization header for all subsequent requests
3. **Patient registration**: `POST /api/patient/createPatient`
4. **Psychologist registration**: `POST /api/psychologist/register`
5. **Schedule appointment**: `POST /api/appointment/Create`
6. **Check availability**: `GET /api/appointment/availability/{psychologistId}?date=2026-09-15`
7. **Set psychologist schedule**: `POST /api/appointment/availability`

---

## Notes for Frontend Developers

- All dates use ISO 8601 format (`YYYY-MM-DDTHH:mm:ss`)
- `DateOnly` parameters use `YYYY-MM-DD` format
- CPF is always 11 digits (no formatting)
- Phone numbers: DDD (2 digits) + Number (9 digits) + CountryCode (2 digits, typically "55")
- Role-based endpoints have `[Authorize(Roles = "...")]` but some are commented out
- The API returns Portuguese/Brazilian error messages
- Swagger is enabled in development for interactive testing