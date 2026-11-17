---
applyTo: ".github/plans/**"
description: Formato y estructura de los planes de trabajo del proyecto
---
# Planes

## Ubicación
- Guardar cada plan como archivo `.md` en `.github/plans/`.

## Estructura
1. Título (`#`), sin emojis.
2. `**Descripción**:` una línea con el objetivo del plan.
3. Encabezado `## Pasos del plan` seguido de la lista numerada.
4. `**Última actualización**:` fecha y hora en formato `AAAA-MM-DD HH:mm` ubicado al final del archivo.

## Formato de los pasos
- Lista numerada (`1.`, `2.`, `3.`...), nunca con guiones. Un paso por línea.
- Cada paso: `N. [ ] **Título breve** — descripción`. Al completarlo, `[ ]` pasa a `[x]`.
- Solo el título (2-20 palabras) va en negrita, y la negrita termina antes del guion largo `—`.
- La descripción va en texto normal, sin negrita. El código en línea va entre backticks.
- Ordenar los pasos de forma que cada uno se pueda ejecutar y validar antes del siguiente.

Correcto:

```markdown
1. [ ] **Obtener la línea base** — ejecutar `dotnet build` y registrar errores y duración.
```

Incorrecto (guion simple y todo en negrita):

```markdown
- **Obtener la línea base — ejecutar `dotnet build` y registrar errores y duración.**
```

## Al actualizar un plan
- Mantener este formato en los pasos existentes, aunque estén escritos de otra forma.
- Actualizar la fecha de `Última actualización`.