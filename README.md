\# ✅ Prueba Técnica ONOFF - Angular 17 + .NET 9



\## 📌 Objetivo del Proyecto



Desarrollar una aplicación web tipo To-Do List que permita:



\- Autenticación de usuarios con JWT

\- Gestión completa de tareas

\- Visualización de métricas

\- Buenas prácticas de arquitectura, seguridad y documentación



Este proyecto fue desarrollado como solución completa a la Prueba Técnica ONOFF.



---



\## 🧰 Tecnologías Utilizadas



\### Frontend

\- Angular 17 (Standalone Components)

\- Angular Router

\- HttpClient

\- Interceptor HTTP

\- Karma + Jasmine (pruebas)

\- SCSS



\### Backend

\- .NET 9 Web API

\- Entity Framework Core (Code First, sin Stored Procedures)

\- SQLite

\- JWT (JSON Web Token)

\- Swagger



---



\## ✅ Funcionalidades Implementadas



\- Login con autenticación JWT

\- Crear tareas

\- Editar tareas

\- Eliminar tareas

\- Marcar como completadas o pendientes

\- Filtros:

&nbsp; - Todas

&nbsp; - Completadas

&nbsp; - Pendientes

\- Dashboard con métricas:

&nbsp; - Total de tareas

&nbsp; - Tareas completadas

&nbsp; - Tareas pendientes

\- Protección de rutas

\- Interceptor para envío automático del token



---



\## 🧠 Decisiones Técnicas



\- Se eligió SQLite por ser liviana, portable y fácil de auditar (archivo TodoDb.db).

\- Se implementó JWT para una autenticación segura y desacoplada.

\- Angular se desarrolló usando componentes standalone, siguiendo la arquitectura moderna recomendada.

\- Se usaron servicios y observables para el manejo de estado.

\- Se implementó un Interceptor HTTP para manejar tokens automáticamente.

\- Se utilizó un proxy Angular para evitar problemas de CORS durante desarrollo.

\- Arquitectura separada por dominios:

&nbsp; - auth → autenticación

&nbsp; - todo → lógica de tareas

&nbsp; - core → servicios, interceptor



---



\## 🗂️ Estructura del Proyecto



ONOFF-angular-dotnet  

backend/ApiTodo  

frontend/todo-app  

README.md  



---



\## ⚙️ Backend (.NET 9)



\### Cómo ejecutar el Backend



cd backend/ApiTodo  

dotnet restore  

dotnet ef database update  

dotnet run  



\### Endpoints Principales



POST   /api/Auth/login   Autenticación de usuario  

GET    /api/Todo         Listar tareas  

POST   /api/Todo         Crear tarea  

PUT    /api/Todo/{id}    Editar tarea  

DELETE /api/Todo/{id}    Eliminar tarea  



\### Usuario de Prueba



Email: user@test.com  

Password: Password123!



\### Swagger



http://localhost:5297/swagger



---



\## 💻 Frontend (Angular 17)



\### Cómo ejecutar el Frontend



cd frontend/todo-app  

npm install  

npm start  



Aplicación disponible en:



http://localhost:4200



---



\## 🔄 Flujo de Uso



1\. El usuario accede a http://localhost:4200  

2\. Se muestra el formulario de login  

3\. Las credenciales se validan contra /api/Auth/login  

4\. El backend retorna un token JWT  

5\. El token se guarda en localStorage  

6\. Se redirige al módulo /todo  

7\. El usuario puede crear, editar, eliminar y filtrar tareas  

8\. El dashboard muestra métricas en tiempo real  



---



\## 🧪 Pruebas Automatizadas



\### Frontend



Ejecutar pruebas:



ng test



Pruebas incluidas en:



auth.service.spec.ts  

todo.service.spec.ts  

login.component.spec.ts  

todo-page.component.spec.ts  



\### Backend



Proyecto preparado para pruebas unitarias con xUnit (controladores y servicios).



---



\## 🔒 Seguridad



\- Autenticación por JWT

\- Envío de token por header Authorization: Bearer

\- Contraseñas cifradas con SHA256

\- Rutas protegidas en backend



---



\## 👤 Autor



David Armando Guevara Rucero  

Prueba Técnica – ONOFF  

Angular 17 | .NET 9 | FullStack



