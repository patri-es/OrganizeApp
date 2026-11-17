# Plan de mejora de mantenibilidad y reducción de CRAP code

**Descripción**: Establecer una línea base de calidad y mejorar incrementalmente la mantenibilidad del backend .NET, sus pruebas y el frontend React/Vite, priorizando los métodos de mayor riesgo CRAP.

## Pasos del plan

1. [x] **Restaurar y compilar backend** — ejecutar `dotnet restore` y `dotnet build`. Ambos comandos finalizaron correctamente.

2. [ ] **Ejecutar pruebas backend** — , guardando los resultados en `test/TestResults`; en frontend, ejecutar `npm ci`, `npm run lint` y `npm run build`. Registrar errores, duración y entorno. No se encontraron informes ejecutados `.trx` ni de cobertura en las rutas revisadas; no se asumirán resultados previos.

3. [ ] **Completar línea base frontend** — `npm ci` finalizó correctamente. La ejecución de `npm run lint` fue cancelada y `npm run build` aún está pendiente; ejecutar ambos y registrar sus resultados.

4. [ ] **Medir y priorizar CRAP** — recopilar cobertura de líneas y ramas, junto con complejidad por método, usando las herramientas disponibles. Calcular o estimar CRAP score y priorizar métodos con alta complejidad y baja cobertura, diferenciando hallazgos confirmados de hipótesis.

5. [ ] **Completar inventario de calidad** — revisar capas, features, configuración de Entity Framework, contratos DTO/API y estructura frontend; contrastar código, README y documentación de tests. Identificar duplicación, responsabilidades mezcladas y discrepancias antes de refactorizar.

6. [ ] **Cubrir comportamiento antes de refactorizar** — añadir o ajustar pruebas existentes para los métodos de mayor CRAP, reglas de dominio, casos de uso y respuestas API. Comprobar el aislamiento y determinismo de las pruebas de infraestructura e integración, y verificar la lista de tests del README frente a los casos implementados.

7. [ ] **Reducir duplicación incrementalmente** — consolidar el mapeo de `Product` a DTO; comprobar el uso de los métodos `ChangeCategory*` respecto a `UpdateCategory` y retirar únicamente los que no tengan consumidores; eliminar código comentado y actualizar comentarios obsoletos.

8. [ ] **Normalizar async y nulabilidad** — revisar tareas `async` que solo devuelven `Task.CompletedTask`, valorar la propagación de `CancellationToken` hasta EF Core en cambios acotados y sustituir anulaciones de nulabilidad con `!` por contratos y validaciones coherentes. Confirmar con pruebas si se duplica la configuración de relaciones EF antes de modificarla.

9. [ ] **Verificar coherencia entre capas** — contrastar códigos HTTP, respuestas y serialización con los consumidores actuales. Mantener el contrato existente salvo que se coordinen cambios en controladores, servicios React y pruebas. Revisar manejo de errores, carga y validación en la SPA; como `package.json` no define un script de tests frontend, usar lint y build y documentar esa limitación.

10. [ ] **Alinear documentación y calidad** — actualizar README e instrucciones para reflejar la estructura y los comandos reales; documentar la generación de resultados y la interpretación de cobertura/CRAP. Evaluar umbrales graduales en CI solo si la configuración lo permite sin añadir paquetes no autorizados.

11. [ ] **Validar y comparar resultados** — ejecutar las pruebas pertinentes tras cada cambio y, al finalizar, `dotnet test`, build backend y lint/build frontend. Comparar cobertura y CRAP con la línea base, comprobar que no haya regresiones contractuales y resumir riesgos y deuda pendiente.

**Última actualización**: 2026-11-05 14:35
