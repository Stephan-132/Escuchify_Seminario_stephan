# 🎵 Escuchify - Proyecto Final Seminario II

> **Desarrollo Backend + Frontend**
> Sistema de gestión musical con arquitectura cliente-servidor, base de datos relacional e interfaz moderna.

![Estado](https://img.shields.io/badge/Estado-Finalizado-green?style=for-the-badge)
![Tech](https://img.shields.io/badge/.NET_9-Blazor-purple?style=for-the-badge)
![DB](https://img.shields.io/badge/SQLite-EF_Core-blue?style=for-the-badge)
![Tailwind](https://img.shields.io/badge/Tailwind_CSS-v4-cyan?style=for-the-badge)

---

## 👥 Integrantes del Grupo: "Codigo Universitario"

* **Integrante 1** - Legajo: XXXXX
* **Integrante 2** - Legajo: XXXXX
* **Integrante 3** - Legajo: XXXXX
* **Integrante 4** - Legajo: XXXXX

---

## ✅ Cumplimiento de Consignas (Proyecto #2)

Este proyecto satisface todos los requerimientos técnicos y funcionales planteados en la consigna del Seminario de Lenguajes II:

### [cite_start]1. API RESTful (Backend) [cite: 10, 11, 12]
Se implementó el **CRUD Completo** para las 3 entidades solicitadas, persistiendo en **SQLite**:
- [x] **Artistas:** Creación, Lectura, Edición y Eliminación (con borrado en cascada).
- [x] **Discos:** Gestión completa vinculada al Artista.
- [x] **Canciones:** Gestión completa vinculada al Disco.

**Aspectos Técnicos:**
* [cite_start]**Verbos HTTP:** Uso estricto de `GET`, `POST`, `PUT` y `DELETE`[cite: 14].
* [cite_start]**Respuestas JSON:** Códigos de estado consistentes (`200 OK`, `201 Created`, `404 Not Found`, `204 No Content`)[cite: 15].
* [cite_start]**Manejo de Errores:** Respuestas consistentes ante fallos o datos inválidos[cite: 16].

### [cite_start]2. Interfaz de Usuario (Frontend) [cite: 17]
Desarrollado en **Blazor Web App (Interactive Server)** consumiendo la API.
- [x] Interfaz visual completa para ejecutar el CRUD.
- [x] Formularios con validaciones de datos (`DataAnnotations`).
- [x] Diseño responsivo y moderno con **Tailwind CSS**.

---

## 📸 Capturas de Pantalla

| Dashboard | Detalle Artista |
|-----------|-----------------|
| ![Dashboard](ScreenShots/Captura%20de%20pantalla%202025-12-02%20180107.png) | ![Artista](ScreenShots/Captura%20de%20pantalla%202025-12-02%20180210.png) |

| Discos del Artista | Lista de Canciones |
|-----------|-----------------|
| ![Discos](ScreenShots/Captura%20de%20pantalla%202025-12-02%20180250.png)| ![Canciones](ScreenShots/Captura%20de%20pantalla%202025-12-02%20180329.png)|

---

## 🏗️ Arquitectura del Proyecto

Se implementó **Clean Architecture** para desacoplar lógica y presentación:

```text
Escuchify_Seminario
├── 📂 escuchify_api           (Backend / API)
│   ├── 📂 Core/Entities       (Modelos de dominio puro)
│   ├── 📂 Data                (Contexto de BD y Migraciones)
│   ├── 📂 Services            (Lógica de negocio)
│   ├── 📂 Controllers         (Endpoints)
│   └── 📂 DTOs                (Transferencia de datos)
│
├── 📂 escuchify_app           (Frontend / Blazor)
│   ├── 📂 Services            (Cliente HTTP)
│   ├── 📂 Components/Pages    (Vistas Razor)
│   └── 📂 Components/UI       (Componentes reutilizables)
│
└── 📂 Escuchify.Test          (Testing)
    └── Pruebas unitarias con xUnit y BD en Memoria.
````

## ✨ Funcionalidades Extra (Valor Agregado)

1.  **Inteligencia Automática:** Al crear un artista o disco, el sistema busca automáticamente su foto/portada en alta resolución usando la API pública de **Deezer** y **iTunes**.
2.  **Autocompletado:** Integración con Wikipedia para biografías.
3.  **UX Avanzada:** Feedback visual con **SweetAlert2** (alertas animadas) y "Skeleton Loading" durante la carga de datos.
4.  **Dashboard Vivo:** Estadísticas en tiempo real en la pantalla de inicio.

-----

## 📡 Documentación de Endpoints Principales

| Método | Endpoint | Descripción |
| :--- | :--- | :--- |
| `GET` | `/api/artistas` | Listado general. |
| `GET` | `/api/artistas/{id}` | Detalle de artista + Discos. |
| `POST` | `/api/artistas` | Crear Artista. |
| `PUT` | `/api/artistas/{id}` | Editar Artista. |
| `DELETE`| `/api/artistas/{id}` | Eliminar (Cascada). |
| `POST` | `/api/discos/{artistaId}` | Agregar Disco a Artista. |
| `POST` | `/api/discos/{discoId}/canciones`| Agregar Canción a Disco. |

-----

## 🛠️ Cómo Ejecutar el Proyecto

### Requisitos

  * .NET 9 SDK
  * Git

### Pasos

1.  **Clonar el repositorio:**

    ```bash
    git clone [https://github.com/Taikaros/Escuchify_Seminario.git](https://github.com/Taikaros/Escuchify_Seminario.git)
    ```

2.  **Iniciar el Backend (API):**

    ```bash
    cd escuchify_api
    dotnet run
    ```

    *(Puerto por defecto: 5232)*

3.  **Iniciar el Frontend (App):**
    En otra terminal:

    ```bash
    cd escuchify_app
    dotnet watch
    ```

4.  **Tests (Opcional):**

    ```bash
    dotnet test
    ```

-----
Desarrollado por **Codigo Universitario** *Seminario de Lenguajes II - 2025*


